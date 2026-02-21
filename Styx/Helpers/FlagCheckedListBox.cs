using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Styx.Helpers
{
    public class FlagCheckedListBox : CheckedListBox
    {
        private Container _components;
        private bool _updating;
        private Type _enumType;
        private Enum _enumValue;

        public FlagCheckedListBox()
        {
            this.Initialize();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this._components != null)
            {
                this._components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Initialize()
        {
            base.CheckOnClick = true;
        }

        public FlagCheckedListBoxItem Add(int v, string c)
        {
            FlagCheckedListBoxItem flagCheckedListBoxItem = new FlagCheckedListBoxItem(v, c);
            base.Items.Add(flagCheckedListBoxItem);
            return flagCheckedListBoxItem;
        }

        public FlagCheckedListBoxItem Add(FlagCheckedListBoxItem item)
        {
            base.Items.Add(item);
            return item;
        }

        protected override void OnItemCheck(ItemCheckEventArgs e)
        {
            base.OnItemCheck(e);
            if (!this._updating)
            {
                FlagCheckedListBoxItem flagCheckedListBoxItem = base.Items[e.Index] as FlagCheckedListBoxItem;
                this.UpdateCheckedItems(flagCheckedListBoxItem, e.NewValue);
            }
        }

        protected void UpdateCheckedItems(int value)
        {
            this._updating = true;
            for (int i = 0; i < base.Items.Count; i++)
            {
                FlagCheckedListBoxItem flagCheckedListBoxItem = base.Items[i] as FlagCheckedListBoxItem;
                if (flagCheckedListBoxItem.Value == 0)
                {
                    base.SetItemChecked(i, value == 0);
                }
                else if ((flagCheckedListBoxItem.Value & value) == flagCheckedListBoxItem.Value && flagCheckedListBoxItem.Value != 0)
                {
                    base.SetItemChecked(i, true);
                }
                else
                {
                    base.SetItemChecked(i, false);
                }
            }
            this._updating = false;
        }

        protected void UpdateCheckedItems(FlagCheckedListBoxItem composite, CheckState cs)
        {
            if (composite.Value == 0)
            {
                this.UpdateCheckedItems(0);
            }
            int num = 0;
            for (int i = 0; i < base.Items.Count; i++)
            {
                FlagCheckedListBoxItem flagCheckedListBoxItem = base.Items[i] as FlagCheckedListBoxItem;
                if (base.GetItemChecked(i))
                {
                    num |= flagCheckedListBoxItem.Value;
                }
            }
            if (cs == CheckState.Unchecked)
            {
                num &= ~composite.Value;
            }
            else
            {
                num |= composite.Value;
            }
            this.UpdateCheckedItems(num);
        }

        public int GetCurrentValue()
        {
            int num = 0;
            for (int i = 0; i < base.Items.Count; i++)
            {
                FlagCheckedListBoxItem flagCheckedListBoxItem = base.Items[i] as FlagCheckedListBoxItem;
                if (base.GetItemChecked(i))
                {
                    num |= flagCheckedListBoxItem.Value;
                }
            }
            return num;
        }

        private void PopulateNames()
        {
            foreach (string text in Enum.GetNames(this._enumType))
            {
                object obj = Enum.Parse(this._enumType, text);
                int num = (int)Convert.ChangeType(obj, typeof(int));
                this.Add(num, text);
            }
        }

        private void UpdateFromEnumValue()
        {
            int num = (int)Convert.ChangeType(this._enumValue, typeof(int));
            this.UpdateCheckedItems(num);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Enum EnumValue
        {
            get
            {
                object obj = Enum.ToObject(this._enumType, this.GetCurrentValue());
                return (Enum)obj;
            }
            set
            {
                base.Items.Clear();
                this._enumValue = value;
                this._enumType = value.GetType();
                this.PopulateNames();
                this.UpdateFromEnumValue();
            }
        }
    }
}
