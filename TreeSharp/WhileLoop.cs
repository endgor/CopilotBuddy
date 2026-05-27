using System.Collections.Generic;

namespace TreeSharp
{
    /// <summary>
    /// HB 6.2.3: Executes all children in sequence while the condition delegate returns true.
    /// When the condition returns false, yields <see cref="returnStatus"/> (default: Failure).
    /// </summary>
    public class WhileLoop : GroupComposite
    {
        private readonly CanRunDecoratorDelegate _condition;
        private readonly RunStatus _returnStatus;

        public WhileLoop(CanRunDecoratorDelegate condition, params Composite[] children)
            : base(children)
        {
            _condition = condition;
            _returnStatus = RunStatus.Failure;
        }

        public WhileLoop(RunStatus returnStatus, CanRunDecoratorDelegate condition, params Composite[] children)
            : base(children)
        {
            _condition = condition;
            _returnStatus = returnStatus;
        }

        protected override IEnumerable<RunStatus> Execute(object context)
        {
            if (ContextChanger != null)
                context = ContextChanger(context);

            while (_condition(context))
            {
                foreach (Composite child in Children)
                {
                    child.Start(context);
                    while (child.Tick(context) == RunStatus.Running)
                    {
                        Selection = child;
                        yield return RunStatus.Running;
                    }
                    Selection = null;
                    child.Stop(context);
                }
            }

            yield return _returnStatus;
            yield break;
        }
    }
}
