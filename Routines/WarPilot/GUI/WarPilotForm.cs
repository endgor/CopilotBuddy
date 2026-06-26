using System;
using System.Drawing;
using System.Windows.Forms;
using WarPilot.Config;

namespace WarPilot.GUI
{
    /// <summary>
    /// WarPilot settings window. Plain WinForms controls themed via Theme.cs (no owner-draw / GDI+).
    /// Layout is done with explicit Location/Size (no Padding/Margin) to stay clear of the bot
    /// runtime-compiler quirks documented in Theme.cs. Placeholder (unwired) settings are greyed-out.
    /// </summary>
    public class WarPilotForm : Form
    {
        private const int LeftX = 18;
        private const int RowH = 30;

        private WarPilotSettings _s;

        // General
        private CheckBox _keepStance, _enableMovement, _enableTargeting, _victoryRush, _commandingShout, _enragedRegen, _racials, _trinkets;
        private NumericUpDown _enragedHealth;

        // Arms
        private CheckBox _rend, _overpower, _mortalStrike, _bladestorm, _slam, _execute,
                         _heroicThrow, _hamstring, _bloodrage, _berserkerRage, _cooldowns, _aoe, _interrupts;
        private NumericUpDown _slamRage, _executeHealth, _hsRage, _recklessHealth, _aoeCount;

        public WarPilotForm()
        {
            _s = WarPilotSettings.Instance;

            Text = "WarPilot";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(560, 600);
            Theme.StyleForm(this);

            var tabs = new TabControl();
            tabs.Location = new Point(12, 12);
            tabs.Size = new Size(536, 520);
            tabs.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            Theme.StyleTabs(tabs);
            tabs.TabPages.Add(BuildGeneral());
            tabs.TabPages.Add(BuildArms());
            tabs.TabPages.Add(BuildProtection());
            tabs.TabPages.Add(BuildFury());
            Controls.Add(tabs);

            var save = new Button();
            save.Text = "Save && Close";
            save.Location = new Point(388, 545);
            save.Size = new Size(160, 34);
            save.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            Theme.StyleButton(save);
            save.Click += OnSave;
            Controls.Add(save);

            var cancel = new Button();
            cancel.Text = "Cancel";
            cancel.Location = new Point(290, 545);
            cancel.Size = new Size(90, 34);
            cancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            cancel.BackColor = Theme.Panel;
            cancel.ForeColor = Theme.Text;
            cancel.FlatStyle = FlatStyle.Flat;
            cancel.FlatAppearance.BorderSize = 0;
            cancel.Click += (s, e) => Close();
            Controls.Add(cancel);
        }

        // ---------- builders ----------

        private TabPage BuildGeneral()
        {
            var p = NewPage("General");
            int y = 14;

            Header(p, ref y, "Wired");
            _keepStance      = Check(p, ref y, "Keep proper stance", _s.KeepStance, true);
            _enableMovement  = Check(p, ref y, "Enable movement (move into melee for you)", _s.EnableMovement, true);
            _enableTargeting = Check(p, ref y, "Enable targeting (pick a target when you have none)", _s.EnableTargeting, true);
            _victoryRush     = Check(p, ref y, "Use Victory Rush (leveling self-heal)", _s.UseVictoryRush, true);
            _commandingShout = Check(p, ref y, "Use Commanding Shout (else Battle Shout)", _s.UseCommandingShout, true);
            _enragedRegen    = Check(p, ref y, "Use Enraged Regeneration", _s.UseEnragedRegen, true);
            _enragedHealth   = Number(p, ref y, "Enraged Regeneration health %", _s.EnragedRegenHealth, 0, 100, true);

            y += 8;
            Header(p, ref y, "Placeholder (not wired yet)");
            _racials  = Check(p, ref y, "Use racials", _s.UseRacials, false);
            _trinkets = Check(p, ref y, "Use on-use trinkets", _s.UseTrinkets, false);

            return p;
        }

        private TabPage BuildArms()
        {
            var p = NewPage("Arms");
            int y = 14;

            Header(p, ref y, "Strikes");
            _rend         = Check(p, ref y, "Keep Rend up (feeds Overpower)", _s.ArmsUseRend, true);
            _overpower    = Check(p, ref y, "Use Overpower on proc", _s.ArmsUseOverpower, true);
            _mortalStrike = Check(p, ref y, "Use Mortal Strike on cooldown", _s.ArmsUseMortalStrike, true);
            _bladestorm   = Check(p, ref y, "Use Bladestorm on cooldown", _s.ArmsUseBladestorm, true);
            _slam         = Check(p, ref y, "Use Slam filler", _s.ArmsUseSlam, true);
            _slamRage     = Number(p, ref y, "Slam minimum rage %", _s.ArmsSlamRage, 0, 100, true);

            y += 8;
            Header(p, ref y, "Execute & rage dump");
            _execute       = Check(p, ref y, "Use Execute (low health / Sudden Death)", _s.ArmsUseExecute, true);
            _executeHealth = Number(p, ref y, "Execute health %", _s.ArmsExecuteHealth, 0, 100, true);
            _hsRage        = Number(p, ref y, "Heroic Strike rage dump %", _s.ArmsHeroicStrikeRage, 0, 100, true);
            _heroicThrow   = Check(p, ref y, "Use Heroic Throw when out of melee range", _s.ArmsUseHeroicThrow, true);

            y += 8;
            Header(p, ref y, "Utility & cooldowns");
            _hamstring      = Check(p, ref y, "Use Hamstring on fleeing targets", _s.ArmsUseHamstring, true);
            _bloodrage      = Check(p, ref y, "Use Bloodrage when low on rage", _s.ArmsUseBloodrage, true);
            _berserkerRage  = Check(p, ref y, "Use Berserker Rage to break fear", _s.ArmsUseBerserkerRage, true);
            _cooldowns      = Check(p, ref y, "Use Recklessness", _s.ArmsUseCooldowns, true);
            _recklessHealth = Number(p, ref y, "Recklessness min target health %", _s.ArmsCooldownMinHealth, 0, 100, true);

            y += 8;
            Header(p, ref y, "AoE & interrupts");
            _aoe        = Check(p, ref y, "Use AoE rotation on packs", _s.ArmsUseAoE, true);
            _aoeCount   = Number(p, ref y, "AoE enemy count", _s.ArmsAoECount, 2, 10, true);
            _interrupts = Check(p, ref y, "Interrupt casts (Pummel — stance-dances to Berserker)", _s.ArmsUseInterrupts, true);

            return p;
        }

        private TabPage BuildProtection()
        {
            var p = NewPage("Protection");
            int y = 14;

            Header(p, ref y, "Phase 2 — not wired yet");
            var note = new Label();
            note.Text = "Protection tanking is a stub. The bot will basic-attack only.\n" +
                        "969 threat, AoE threat, auto-taunt and defensive cooldowns are\n" +
                        "scheduled for Phase 2. These toggles are inactive for now.";
            note.Location = new Point(LeftX, y);
            Theme.StyleLabel(note, true);
            p.Controls.Add(note);
            y += 64;

            Check(p, ref y, "Auto-taunt mobs off party members", _s.ProtAutoTaunt, false);
            Check(p, ref y, "969 threat rotation", _s.ProtUse969, false);
            Check(p, ref y, "Use defensive cooldowns (Shield Wall / Last Stand)", _s.ProtUseDefensives, false);

            return p;
        }

        private TabPage BuildFury()
        {
            var p = NewPage("Fury");
            int y = 14;

            Header(p, ref y, "Not planned");
            var note = new Label();
            note.Text = "Fury is not on the roadmap (leveling = Arms, dungeons = Prot).\n" +
                        "A Fury-specced character gets a minimal attack fallback only.";
            note.Location = new Point(LeftX, y);
            Theme.StyleLabel(note, true);
            p.Controls.Add(note);

            return p;
        }

        // ---------- control helpers ----------

        private TabPage NewPage(string title)
        {
            var p = new TabPage(title);
            Theme.StylePage(p);
            return p;
        }

        private void Header(TabPage p, ref int y, string text)
        {
            var l = new Label();
            l.Text = text;
            l.Location = new Point(LeftX, y);
            Theme.StyleHeader(l);
            p.Controls.Add(l);
            y += 26;
        }

        private CheckBox Check(TabPage p, ref int y, string text, bool value, bool enabled)
        {
            var c = new CheckBox();
            c.Text = text;
            c.Checked = value;
            c.Location = new Point(LeftX, y);
            c.Width = 480;
            Theme.StyleCheck(c, enabled);
            p.Controls.Add(c);
            y += RowH;
            return c;
        }

        private NumericUpDown Number(TabPage p, ref int y, string text, int value, int min, int max, bool enabled)
        {
            var caption = new Label();
            caption.Text = text;
            caption.Location = new Point(LeftX, y + 2);
            Theme.StyleLabel(caption, !enabled);
            p.Controls.Add(caption);

            var n = new NumericUpDown();
            n.Location = new Point(LeftX + 330, y);
            n.Width = 70;
            n.Minimum = min;
            n.Maximum = max;
            n.Value = Math.Max(min, Math.Min(max, value));
            n.Enabled = enabled;
            Theme.StyleNumeric(n);
            p.Controls.Add(n);
            y += RowH;
            return n;
        }

        // ---------- save ----------

        private void OnSave(object sender, EventArgs e)
        {
            _s.KeepStance         = _keepStance.Checked;
            _s.EnableMovement     = _enableMovement.Checked;
            _s.EnableTargeting    = _enableTargeting.Checked;
            _s.UseVictoryRush     = _victoryRush.Checked;
            _s.UseCommandingShout = _commandingShout.Checked;
            _s.UseEnragedRegen    = _enragedRegen.Checked;
            _s.EnragedRegenHealth = (int)_enragedHealth.Value;
            _s.UseRacials         = _racials.Checked;
            _s.UseTrinkets        = _trinkets.Checked;

            _s.ArmsUseRend          = _rend.Checked;
            _s.ArmsUseOverpower     = _overpower.Checked;
            _s.ArmsUseMortalStrike  = _mortalStrike.Checked;
            _s.ArmsUseBladestorm    = _bladestorm.Checked;
            _s.ArmsUseSlam          = _slam.Checked;
            _s.ArmsSlamRage         = (int)_slamRage.Value;
            _s.ArmsUseExecute       = _execute.Checked;
            _s.ArmsExecuteHealth    = (int)_executeHealth.Value;
            _s.ArmsHeroicStrikeRage = (int)_hsRage.Value;
            _s.ArmsUseHeroicThrow   = _heroicThrow.Checked;
            _s.ArmsUseHamstring     = _hamstring.Checked;
            _s.ArmsUseBloodrage     = _bloodrage.Checked;
            _s.ArmsUseBerserkerRage = _berserkerRage.Checked;
            _s.ArmsUseCooldowns     = _cooldowns.Checked;
            _s.ArmsCooldownMinHealth = (int)_recklessHealth.Value;
            _s.ArmsUseAoE           = _aoe.Checked;
            _s.ArmsAoECount         = (int)_aoeCount.Value;
            _s.ArmsUseInterrupts    = _interrupts.Checked;

            _s.Save();
            Close();
        }
    }
}
