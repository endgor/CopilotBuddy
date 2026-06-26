using System;
using System.Drawing;
using System.Windows.Forms;
using PallyPilot.Config;

namespace PallyPilot.GUI
{
    /// <summary>
    /// PallyPilot settings window. Plain WinForms controls themed via Theme.cs (no owner-draw / GDI+,
    /// no Padding/Margin — see memory routine-gui-no-gdiplus). Explicit Location/Size layout. The
    /// Retribution and Holy tabs are fully wired; Protection is a greyed-out stub note.
    /// </summary>
    public class PallyPilotForm : Form
    {
        private const int LeftX = 18;
        private const int RowH = 30;
        private const int CtrlX = LeftX + 340;

        private PallyPilotSettings _s;

        // General
        private CheckBox _enableMovement, _enableTargeting, _keepBlessing, _keepAura,
                         _autoCleanse, _cleanseMagic, _useLoH, _racials, _trinkets;
        private ComboBox _blessing, _aura;
        private NumericUpDown _cleanseMana, _lohHealth;

        // Retribution
        private ComboBox _retSeal, _retJudgement;
        private CheckBox _retCS, _retDS, _retJudge, _retConsec, _retExo, _retHolyWrath, _retHoW,
                         _retAW, _retAoE, _retSelfHeal, _retDivProt, _retDivShield, _retDivPlea;
        private NumericUpDown _retConsecMana, _retHoWHealth, _retAWHealth, _retAoECount,
                              _retSelfHealHp, _retDivProtHp, _retDivShieldHp, _retDivPleaMana;

        // Holy
        private CheckBox _holySmart, _holyDownrank, _holyShock, _holyShockMove, _holyBeacon,
                         _holyBeaconSelf, _holySacred, _holyDivFavor, _holyDivPlea, _holyDivIllum, _holyJudge;
        private NumericUpDown _holyRange, _holyStart, _holyFlashHp, _holyLightHp, _holyDivFavorHp,
                              _holyDivPleaMana, _holyDivPleaSafe, _holyDivIllumMana;

        public PallyPilotForm()
        {
            _s = PallyPilotSettings.Instance;

            Text = "PallyPilot";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(600, 640);
            Theme.StyleForm(this);

            var tabs = new TabControl();
            tabs.Location = new Point(12, 12);
            tabs.Size = new Size(576, 560);
            tabs.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            Theme.StyleTabs(tabs);
            tabs.TabPages.Add(BuildGeneral());
            tabs.TabPages.Add(BuildRet());
            tabs.TabPages.Add(BuildHoly());
            tabs.TabPages.Add(BuildProtection());
            Controls.Add(tabs);

            var save = new Button();
            save.Text = "Save && Close";
            save.Location = new Point(428, 585);
            save.Size = new Size(160, 34);
            save.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            Theme.StyleButton(save);
            save.Click += OnSave;
            Controls.Add(save);

            var cancel = new Button();
            cancel.Text = "Cancel";
            cancel.Location = new Point(330, 585);
            cancel.Size = new Size(90, 34);
            cancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            cancel.BackColor = Theme.Panel;
            cancel.ForeColor = Theme.Text;
            cancel.FlatStyle = FlatStyle.Flat;
            cancel.FlatAppearance.BorderSize = 0;
            cancel.Click += (s, e) => Close();
            Controls.Add(cancel);
        }

        // ---------- General ----------

        private TabPage BuildGeneral()
        {
            var p = NewPage("General");
            int y = 14;

            Header(p, ref y, "Movement & targeting");
            _enableMovement  = Check(p, ref y, "Enable movement (move into melee / heal range)", _s.EnableMovement, true);
            _enableTargeting = Check(p, ref y, "Enable targeting (Ret: pick a target when you have none)", _s.EnableTargeting, true);

            y += 8;
            Header(p, ref y, "Blessing & aura (auto-managed)");
            _keepBlessing = Check(p, ref y, "Keep a Blessing up", _s.KeepBlessing, true);
            _blessing     = Combo(p, ref y, "Blessing (Auto = Kings, else Might)", _s.Blessing, true);
            _keepAura     = Check(p, ref y, "Keep an Aura up", _s.KeepAura, true);
            _aura         = Combo(p, ref y, "Aura (Auto adapts to spec)", _s.Aura, true);

            y += 8;
            Header(p, ref y, "Cleanse (both specs)");
            _autoCleanse  = Check(p, ref y, "Auto Cleanse / Purify (Poison & Disease)", _s.AutoCleanse, true);
            _cleanseMagic = Check(p, ref y, "Also cleanse Magic (Cleanse only)", _s.CleanseMagic, true);
            _cleanseMana  = Number(p, ref y, "Cleanse minimum mana %", _s.CleanseMinMana, 0, 100, true);

            y += 8;
            Header(p, ref y, "Emergency");
            _useLoH    = Check(p, ref y, "Use Lay on Hands (full heal when critical)", _s.UseLayOnHands, true);
            _lohHealth = Number(p, ref y, "Lay on Hands health %", _s.LayOnHandsHealth, 0, 100, true);

            y += 8;
            Header(p, ref y, "Placeholder (not wired yet)");
            _racials  = Check(p, ref y, "Use racials", _s.UseRacials, false);
            _trinkets = Check(p, ref y, "Use on-use trinkets", _s.UseTrinkets, false);

            return p;
        }

        // ---------- Retribution ----------

        private TabPage BuildRet()
        {
            var p = NewPage("Retribution");
            int y = 14;

            Header(p, ref y, "Seal & judgement");
            _retSeal      = Combo(p, ref y, "Seal (Auto = Command, else Righteousness)", _s.RetSeal, true);
            _retJudgement = Combo(p, ref y, "Judgement (Wisdom returns mana)", _s.RetJudgement, true);

            y += 8;
            Header(p, ref y, "Core rotation");
            _retCS    = Check(p, ref y, "Use Crusader Strike", _s.RetUseCrusaderStrike, true);
            _retJudge = Check(p, ref y, "Use Judgement on cooldown", _s.RetUseJudgement, true);
            _retDS    = Check(p, ref y, "Use Divine Storm", _s.RetUseDivineStorm, true);
            _retExo   = Check(p, ref y, "Use Exorcism (only on Art of War proc)", _s.RetUseExorcism, true);
            _retHolyWrath = Check(p, ref y, "Use Holy Wrath (vs Undead/Demon)", _s.RetUseHolyWrath, true);
            _retConsec     = Check(p, ref y, "Use Consecration", _s.RetUseConsecration, true);
            _retConsecMana = Number(p, ref y, "Consecration mana floor %", _s.RetConsecrationMana, 0, 100, true);

            y += 8;
            Header(p, ref y, "Execute & cooldowns");
            _retHoW       = Check(p, ref y, "Use Hammer of Wrath (execute)", _s.RetUseHammerOfWrath, true);
            _retHoWHealth = Number(p, ref y, "Hammer of Wrath health %", _s.RetHammerOfWrathHealth, 0, 100, true);
            _retAW        = Check(p, ref y, "Use Avenging Wrath", _s.RetUseAvengingWrath, true);
            _retAWHealth  = Number(p, ref y, "Avenging Wrath min target health %", _s.RetAvengingWrathHealth, 0, 100, true);

            y += 8;
            Header(p, ref y, "AoE");
            _retAoE      = Check(p, ref y, "Use AoE rotation on packs", _s.RetUseAoE, true);
            _retAoECount = Number(p, ref y, "AoE enemy count", _s.RetAoECount, 2, 10, true);

            y += 8;
            Header(p, ref y, "Self-sustain & defensives");
            _retSelfHeal   = Check(p, ref y, "Self-heal with Flash of Light", _s.RetSelfHeal, true);
            _retSelfHealHp = Number(p, ref y, "Self-heal health %", _s.RetSelfHealHealth, 0, 100, true);
            _retDivProt    = Check(p, ref y, "Use Divine Protection", _s.RetUseDivineProtection, true);
            _retDivProtHp  = Number(p, ref y, "Divine Protection health %", _s.RetDivineProtectionHealth, 0, 100, true);
            _retDivShield  = Check(p, ref y, "Use Divine Shield (panic bubble)", _s.RetUseDivineShield, true);
            _retDivShieldHp = Number(p, ref y, "Divine Shield health %", _s.RetDivineShieldHealth, 0, 100, true);
            _retDivPlea    = Check(p, ref y, "Use Divine Plea for mana", _s.RetUseDivinePlea, true);
            _retDivPleaMana = Number(p, ref y, "Divine Plea mana %", _s.RetDivinePleaMana, 0, 100, true);

            return p;
        }

        // ---------- Holy ----------

        private TabPage BuildHoly()
        {
            var p = NewPage("Holy");
            int y = 14;

            Header(p, ref y, "Heal targeting & spell selection");
            _holyRange    = Number(p, ref y, "Heal range (yards)", _s.HolyHealRange, 5, 60, true);
            _holyStart    = Number(p, ref y, "Start healing below health %", _s.HolyStartHealHealth, 1, 100, true);
            _holySmart    = Check(p, ref y, "Smart spell selection (FoL / HL / Holy Shock by deficit)", _s.HolySmartSelect, true);
            _holyFlashHp  = Number(p, ref y, "Flash of Light below health %", _s.HolyFlashHealth, 1, 100, true);
            _holyLightHp  = Number(p, ref y, "Holy Light below health %", _s.HolyLightHealth, 1, 100, true);
            _holyDownrank = Check(p, ref y, "Use spell downranking (mana saver)", _s.HolyDownrank, true);

            y += 8;
            Header(p, ref y, "Holy Shock");
            _holyShock     = Check(p, ref y, "Use Holy Shock", _s.HolyUseHolyShock, true);
            _holyShockMove = Check(p, ref y, "Allow Holy Shock while moving", _s.HolyShockOnMove, true);

            y += 8;
            Header(p, ref y, "Tank upkeep");
            _holyBeacon     = Check(p, ref y, "Keep Beacon of Light on tank", _s.HolyUseBeacon, true);
            _holyBeaconSelf = Check(p, ref y, "Beacon self if no tank (solo)", _s.HolyBeaconSelfIfSolo, true);
            _holySacred     = Check(p, ref y, "Keep Sacred Shield on tank", _s.HolyUseSacredShield, true);

            y += 8;
            Header(p, ref y, "Crit cooldown & mana management");
            _holyDivFavor    = Check(p, ref y, "Use Divine Favor on critical target", _s.HolyUseDivineFavor, true);
            _holyDivFavorHp  = Number(p, ref y, "Divine Favor health %", _s.HolyDivineFavorHealth, 1, 100, true);
            _holyDivPlea     = Check(p, ref y, "Use Divine Plea (safe windows only)", _s.HolyUseDivinePlea, true);
            _holyDivPleaMana = Number(p, ref y, "Divine Plea mana %", _s.HolyDivinePleaMana, 0, 100, true);
            _holyDivPleaSafe = Number(p, ref y, "Divine Plea safe health %", _s.HolyDivinePleaSafeHealth, 1, 100, true);
            _holyDivIllum    = Check(p, ref y, "Use Divine Illumination", _s.HolyUseDivineIllumination, true);
            _holyDivIllumMana = Number(p, ref y, "Divine Illumination mana %", _s.HolyDivineIlluminationMana, 0, 100, true);
            _holyJudge       = Check(p, ref y, "Judge for mana when safe", _s.HolyJudgeForMana, true);

            return p;
        }

        // ---------- Protection ----------

        private TabPage BuildProtection()
        {
            var p = NewPage("Protection");
            int y = 14;

            Header(p, ref y, "Stub — not implemented");
            var note = new Label();
            note.Text = "Protection is a minimal fallback only. A Prot-specced character gets\n" +
                        "Righteous Fury + seal upkeep + basic strikes so it still fights, but\n" +
                        "real tanking (threat priority, AoE threat, taunt, defensive cooldowns)\n" +
                        "is not implemented. PallyPilot is built for Retribution (leveling) and\n" +
                        "Holy (dungeon healing).";
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
            c.Width = 500;
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
            n.Location = new Point(CtrlX, y);
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

        private ComboBox Combo(TabPage p, ref int y, string text, Enum value, bool enabled)
        {
            var caption = new Label();
            caption.Text = text;
            caption.Location = new Point(LeftX, y + 4);
            Theme.StyleLabel(caption, !enabled);
            p.Controls.Add(caption);

            var c = new ComboBox();
            c.Location = new Point(CtrlX, y);
            c.Width = 170;
            c.Enabled = enabled;
            Theme.StyleCombo(c);
            foreach (var v in Enum.GetValues(value.GetType()))
                c.Items.Add(v);
            c.SelectedItem = value;
            p.Controls.Add(c);
            y += RowH;
            return c;
        }

        // ---------- save ----------

        private void OnSave(object sender, EventArgs e)
        {
            // General
            _s.EnableMovement  = _enableMovement.Checked;
            _s.EnableTargeting = _enableTargeting.Checked;
            _s.KeepBlessing    = _keepBlessing.Checked;
            _s.Blessing        = (BlessingChoice)_blessing.SelectedItem;
            _s.KeepAura        = _keepAura.Checked;
            _s.Aura            = (AuraChoice)_aura.SelectedItem;
            _s.AutoCleanse     = _autoCleanse.Checked;
            _s.CleanseMagic    = _cleanseMagic.Checked;
            _s.CleanseMinMana  = (int)_cleanseMana.Value;
            _s.UseLayOnHands   = _useLoH.Checked;
            _s.LayOnHandsHealth = (int)_lohHealth.Value;
            _s.UseRacials      = _racials.Checked;
            _s.UseTrinkets     = _trinkets.Checked;

            // Retribution
            _s.RetSeal         = (RetSealChoice)_retSeal.SelectedItem;
            _s.RetJudgement    = (JudgementChoice)_retJudgement.SelectedItem;
            _s.RetUseCrusaderStrike = _retCS.Checked;
            _s.RetUseJudgement = _retJudge.Checked;
            _s.RetUseDivineStorm = _retDS.Checked;
            _s.RetUseExorcism  = _retExo.Checked;
            _s.RetUseHolyWrath = _retHolyWrath.Checked;
            _s.RetUseConsecration = _retConsec.Checked;
            _s.RetConsecrationMana = (int)_retConsecMana.Value;
            _s.RetUseHammerOfWrath = _retHoW.Checked;
            _s.RetHammerOfWrathHealth = (int)_retHoWHealth.Value;
            _s.RetUseAvengingWrath = _retAW.Checked;
            _s.RetAvengingWrathHealth = (int)_retAWHealth.Value;
            _s.RetUseAoE       = _retAoE.Checked;
            _s.RetAoECount     = (int)_retAoECount.Value;
            _s.RetSelfHeal     = _retSelfHeal.Checked;
            _s.RetSelfHealHealth = (int)_retSelfHealHp.Value;
            _s.RetUseDivineProtection = _retDivProt.Checked;
            _s.RetDivineProtectionHealth = (int)_retDivProtHp.Value;
            _s.RetUseDivineShield = _retDivShield.Checked;
            _s.RetDivineShieldHealth = (int)_retDivShieldHp.Value;
            _s.RetUseDivinePlea = _retDivPlea.Checked;
            _s.RetDivinePleaMana = (int)_retDivPleaMana.Value;

            // Holy
            _s.HolyHealRange   = (int)_holyRange.Value;
            _s.HolyStartHealHealth = (int)_holyStart.Value;
            _s.HolySmartSelect = _holySmart.Checked;
            _s.HolyFlashHealth = (int)_holyFlashHp.Value;
            _s.HolyLightHealth = (int)_holyLightHp.Value;
            _s.HolyDownrank    = _holyDownrank.Checked;
            _s.HolyUseHolyShock = _holyShock.Checked;
            _s.HolyShockOnMove = _holyShockMove.Checked;
            _s.HolyUseBeacon   = _holyBeacon.Checked;
            _s.HolyBeaconSelfIfSolo = _holyBeaconSelf.Checked;
            _s.HolyUseSacredShield = _holySacred.Checked;
            _s.HolyUseDivineFavor = _holyDivFavor.Checked;
            _s.HolyDivineFavorHealth = (int)_holyDivFavorHp.Value;
            _s.HolyUseDivinePlea = _holyDivPlea.Checked;
            _s.HolyDivinePleaMana = (int)_holyDivPleaMana.Value;
            _s.HolyDivinePleaSafeHealth = (int)_holyDivPleaSafe.Value;
            _s.HolyUseDivineIllumination = _holyDivIllum.Checked;
            _s.HolyDivineIlluminationMana = (int)_holyDivIllumMana.Value;
            _s.HolyJudgeForMana = _holyJudge.Checked;

            _s.Save();
            Close();
        }
    }
}
