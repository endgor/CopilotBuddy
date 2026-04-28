using System.Collections.Generic;
using System.Linq;

namespace Bots.DungeonBuddy.Helpers
{
    public class ErrorCollection : List<Error>
    {
        public ErrorCollection()
        {
        }

        public ErrorCollection(int capacity)
            : base(capacity)
        {
        }

        public ErrorCollection(IEnumerable<Error> collection)
            : base(collection)
        {
        }

        public bool HasErrors => this.Any(e => e.Type == ErrorType.Error);

        public bool HasWarnings => this.Any(e => e.Type == ErrorType.Warning);

        public int ErrorCount => this.Count(e => e.Type == ErrorType.Error);

        public int WarningCount => this.Count(e => e.Type == ErrorType.Warning);

        public IEnumerable<Error> Errors => this.Where(e => e.Type == ErrorType.Error);

        public IEnumerable<Error> Warnings => this.Where(e => e.Type == ErrorType.Warning);
    }
}