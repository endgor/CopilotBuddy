using System;

namespace Styx.Helpers
{
    public class FlagCheckedListBoxItem
    {
        public FlagCheckedListBoxItem(int v, string c)
        {
            this.Value = v;
            this.Caption = c;
        }

        public override string ToString()
        {
            return this.Caption;
        }

        public bool IsFlag
        {
            get
            {
                return (this.Value & (this.Value - 1)) == 0;
            }
        }

        public bool IsMemberFlag(FlagCheckedListBoxItem composite)
        {
            if (this.IsFlag)
            {
                return (this.Value & composite.Value) == this.Value;
            }
            return false;
        }

        public int Value;
        public string Caption;
    }
}
