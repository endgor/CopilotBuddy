using System.Drawing;
using System.Windows.Forms;

namespace WarPilot.GUI
{
    /// <summary>
    /// Dark theme applied via control PROPERTIES only.
    ///
    /// IMPORTANT (see memory routine-gui-no-gdiplus): the bot's runtime Roslyn compiler does NOT
    /// reference System.Private.Windows.GdiPlus, so any owner-draw / custom paint (e.Graphics.*,
    /// DrawMode=OwnerDrawFixed + DrawItem, new SolidBrush/Pen) fails to LOAD even though it
    /// type-checks fine locally. So: NO custom painting anywhere — colors come from BackColor/
    /// ForeColor/FlatStyle only. Also do not set Padding/Margin (forces the unreferenced
    /// System.Windows.Forms.Primitives assembly at bot-compile time).
    /// </summary>
    internal static class Theme
    {
        public static readonly Color Background = Color.FromArgb(30, 30, 30);
        public static readonly Color Panel      = Color.FromArgb(37, 37, 38);
        public static readonly Color Accent      = Color.FromArgb(200, 64, 47);   // warrior brick-red
        public static readonly Color Text        = Color.FromArgb(224, 224, 224);
        public static readonly Color TextDim      = Color.FromArgb(150, 150, 150);

        public static void StyleForm(Form f)
        {
            f.BackColor = Background;
            f.ForeColor = Text;
            f.Font = new Font("Segoe UI", 9f);
        }

        public static void StyleTabs(TabControl tc)
        {
            tc.BackColor = Background;
            tc.ForeColor = Text;
        }

        public static void StylePage(TabPage p)
        {
            p.BackColor = Panel;
            p.ForeColor = Text;
            p.AutoScroll = true;
        }

        public static void StyleCheck(CheckBox c, bool enabled)
        {
            // FlatStyle.Standard renders the native checkbox (WHITE box + dark check) which is clearly
            // visible, while the label area keeps the dark BackColor with light text. A Flat checkbox
            // on a dark panel is near-invisible (its box shares the dark BackColor), so we avoid it.
            c.FlatStyle = FlatStyle.Standard;
            c.BackColor = Panel;
            c.ForeColor = enabled ? Text : TextDim;
            c.AutoSize = true;
            c.Enabled = enabled;
        }

        public static void StyleLabel(Label l, bool dim)
        {
            l.BackColor = Color.Transparent;
            l.ForeColor = dim ? TextDim : Text;
            l.AutoSize = true;
        }

        public static void StyleHeader(Label l)
        {
            l.BackColor = Color.Transparent;
            l.ForeColor = Accent;
            l.AutoSize = true;
            l.Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold);
        }

        public static void StyleButton(Button b)
        {
            b.BackColor = Accent;
            b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
        }

        public static void StyleNumeric(NumericUpDown n)
        {
            n.BackColor = Panel;
            n.ForeColor = Text;
            n.BorderStyle = BorderStyle.FixedSingle;
        }
    }
}
