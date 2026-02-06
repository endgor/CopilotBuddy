using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Styx.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

#nullable disable
namespace Styx.Logic.Profiles.Quest
{
    /// <summary>
    /// Compiles and evaluates C# condition expressions at runtime.
    /// Uses Roslyn Scripting (replaces legacy CSharpCodeProvider which is not supported on .NET Core)
    /// </summary>
    public class ConditionHelper
    {
        private static long _conditionCounter;
        private readonly string _conditionString;
        private readonly long _conditionId;
        
        // Script options with all required assemblies and imports (same as HB 4.3.4)
        private static readonly ScriptOptions _scriptOptions;
        
        static ConditionHelper()
        {
            // Build script options once - same imports as HB 4.3.4 ConditionHelper
            _scriptOptions = ScriptOptions.Default
                .AddReferences(
                    typeof(object).Assembly,                           // System.Runtime
                    typeof(Enumerable).Assembly,                       // System.Linq
                    typeof(Styx.StyxWoW).Assembly,                     // CopilotBuddy (this assembly)
                    typeof(Styx.WoWInternals.ObjectManager).Assembly,
                    typeof(Styx.Combat.CombatRoutine.WoWClass).Assembly
                )
                .AddImports(
                    "System",
                    "System.Linq",
                    "Styx",
                    "Styx.Helpers",
                    "Styx.Logic.Combat",
                    "Styx.WoWInternals",
                    "Styx.WoWInternals.WoWObjects",
                    "Styx.Combat.CombatRoutine"
                );
        }

        public ConditionHelper(string conditionString)
        {
            _conditionString = conditionString;
            _conditionId = _conditionCounter++;
        }

        /// <summary>
        /// Compile expression and return bound delegate using Roslyn
        /// </summary>
        public bool CompileAndBindExpression(out string[] buildErrors, out Func<bool> boundExpression)
        {
            try
            {
                // Create a globals object that provides Me property (like ProfileHelperFunctionsBase)
                var script = CSharpScript.Create<bool>(
                    _conditionString,
                    _scriptOptions,
                    globalsType: typeof(ConditionGlobals)
                );
                
                // Compile the script
                var compilation = script.Compile();
                
                if (compilation.Any())
                {
                    buildErrors = compilation.Select(d => d.GetMessage()).ToArray();
                    boundExpression = null;
                    return false;
                }
                
                // Create the bound expression that evaluates with current game state
                boundExpression = () =>
                {
                    try
                    {
                        var globals = new ConditionGlobals();
                        var result = script.RunAsync(globals).GetAwaiter().GetResult();
                        return result.ReturnValue;
                    }
                    catch (Exception ex)
                    {
                        Logging.WriteDebug("Error evaluating condition '{0}': {1}", _conditionString, ex.Message);
                        return false;
                    }
                };
                
                buildErrors = null;
                return true;
            }
            catch (Exception ex)
            {
                buildErrors = new[] { ex.Message };
                boundExpression = null;
                return false;
            }
        }

        public static Func<bool> ParseConditionString(string str)
        {
            Func<bool> conditionString;
            try
            {
                conditionString = new Func<bool>(new CompiledCondition(str).Evaluate);
            }
            catch (Styx.CantCompileException)
            {
                conditionString = null;
            }
            return conditionString;
        }

        internal static Func<bool> CompileCondition(XAttribute conditionAttribute)
        {
            Func<bool> func;
            try
            {
                IXmlLineInfo xmlLineInfo = (IXmlLineInfo)conditionAttribute;
                func = !xmlLineInfo.HasLineInfo()
                    ? new Func<bool>(new CompiledCondition(conditionAttribute.Value).Evaluate)
                    : new Func<bool>(new CompiledCondition(conditionAttribute.Value, xmlLineInfo.LineNumber).Evaluate);
            }
            catch (Styx.CantCompileException)
            {
                func = null;
            }
            return func;
        }

        private class CompiledCondition
        {
            private static readonly Dictionary<string, Func<bool>> _cache = new Dictionary<string, Func<bool>>();
            private static readonly object _lock = new object();
            private readonly string _expression;
            private Func<bool> _compiled;
            private bool _initialized;
            private readonly int? _lineNumber;

            public CompiledCondition(string expression, int? lineNumber = null)
            {
                _expression = expression;
                _lineNumber = lineNumber;
                if (StyxSettings.Instance.ProfileDebuggingMode)
                {
                    _compiled = Compile();
                    _initialized = true;
                    if (_compiled == null)
                        throw new Styx.CantCompileException();
                }
            }

            public bool Evaluate()
            {
                if (!_initialized)
                {
                    _compiled = Compile();
                    _initialized = true;
                }
                return _compiled != null && _compiled();
            }

            private Func<bool> Compile()
            {
                if (_cache.TryGetValue(_expression, out Func<bool> cached))
                    return cached;

                lock (_lock)
                {
                    if (_cache.TryGetValue(_expression, out cached))
                        return cached;

                    var helper = new ConditionHelper(_expression);
                    if (_lineNumber.HasValue)
                        Logging.WriteDebug("Compiling expression '{0}' @ line {1}", (object)_expression, (object)_lineNumber.Value);
                    else
                        Logging.WriteDebug("Compiling expression '{0}'", (object)_expression);

                    if (helper.CompileAndBindExpression(out string[] errors, out Func<bool> bound))
                    {
                        _cache[_expression] = bound;
                        return bound;
                    }
                    else
                    {
                        if (errors.Length > 0)
                        {
                            Logging.WriteDebug("{0} errors encountered while compiling condition '{1}'", (object)errors.Length, (object)_expression);
                            foreach (var error in errors)
                                Logging.WriteDebug(error);
                        }
                        _cache[_expression] = null;
                        return null;
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// Globals object for Roslyn script evaluation.
    /// Provides Me property (like ProfileHelperFunctionsBase in HB)
    /// </summary>
    public class ConditionGlobals
    {
        /// <summary>
        /// The local player - equivalent to StyxWoW.Me / ObjectManager.Me
        /// </summary>
        public Styx.WoWInternals.WoWObjects.LocalPlayer Me => Styx.WoWInternals.ObjectManager.Me;
    }
}