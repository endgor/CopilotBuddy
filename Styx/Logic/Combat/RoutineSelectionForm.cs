using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Styx.Combat.CombatRoutine;

namespace Styx.Logic.Combat
{
	/// <summary>
	/// Dialog to let the user pick a combat routine when multiple routines
	/// match the current class. Ported from HB 3.3.5a/4.3.4 RoutineSelectionForm.
	/// Layout matches HB 3.3.5a ns7.RoutineSelectionForm.Designer.cs exactly.
	/// </summary>
	internal sealed class RoutineSelectionForm : Form
	{
		private Label _label1;
		private GroupBox _groupBox1;
		private ListBox _lstAvailable;
		private Button _btnSelect;
		private Button _btnCancel;

		internal CombatRoutine SelectedRoutine { get; private set; }

		internal RoutineSelectionForm(IEnumerable<CombatRoutine> available)
		{
			InitializeComponent();
			_lstAvailable.SuspendLayout();
			foreach (CombatRoutine routine in available)
			{
				_lstAvailable.Items.Add(routine);
			}
			_lstAvailable.ResumeLayout();
		}

		private void lstAvailable_SelectedIndexChanged(object sender, EventArgs e)
		{
			SelectedRoutine = _lstAvailable.SelectedItem as CombatRoutine;
		}

		private void btnSelect_Click(object sender, EventArgs e)
		{
			if (SelectedRoutine == null)
			{
				MessageBox.Show(
					"You have not selected a routine to use, please do so, or hit the Cancel button!",
					"Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else
			{
				Close();
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			SelectedRoutine = null;
			Close();
		}

		/// <summary>
		/// Exact layout from HB 3.3.5a ns7.RoutineSelectionForm.Designer.cs
		/// </summary>
		private void InitializeComponent()
		{
			_label1 = new Label();
			_groupBox1 = new GroupBox();
			_lstAvailable = new ListBox();
			_btnSelect = new Button();
			_btnCancel = new Button();
			_groupBox1.SuspendLayout();
			SuspendLayout();

			// label1
			_label1.Location = new Point(12, 9);
			_label1.Name = "label1";
			_label1.Size = new Size(381, 31);
			_label1.TabIndex = 0;
			_label1.Text = "We have determined that you have multiple custom classes for your current character class, please select the one you wish to use!";

			// groupBox1
			_groupBox1.Controls.Add(_lstAvailable);
			_groupBox1.Location = new Point(12, 43);
			_groupBox1.Name = "groupBox1";
			_groupBox1.Size = new Size(381, 148);
			_groupBox1.TabIndex = 1;
			_groupBox1.TabStop = false;
			_groupBox1.Text = "Available";

			// lstAvailable
			_lstAvailable.BackColor = SystemColors.Control;
			_lstAvailable.BorderStyle = BorderStyle.None;
			_lstAvailable.Dock = DockStyle.Fill;
			_lstAvailable.FormattingEnabled = true;
			_lstAvailable.Location = new Point(3, 16);
			_lstAvailable.Name = "lstAvailable";
			_lstAvailable.Size = new Size(375, 129);
			_lstAvailable.TabIndex = 0;
			_lstAvailable.SelectedIndexChanged += lstAvailable_SelectedIndexChanged;

			// btnSelect
			_btnSelect.DialogResult = DialogResult.OK;
			_btnSelect.Location = new Point(318, 197);
			_btnSelect.Name = "btnSelect";
			_btnSelect.Size = new Size(75, 23);
			_btnSelect.TabIndex = 2;
			_btnSelect.Text = "Select";
			_btnSelect.UseVisualStyleBackColor = true;
			_btnSelect.Click += btnSelect_Click;

			// btnCancel
			_btnCancel.DialogResult = DialogResult.Cancel;
			_btnCancel.Location = new Point(237, 197);
			_btnCancel.Name = "btnCancel";
			_btnCancel.Size = new Size(75, 23);
			_btnCancel.TabIndex = 3;
			_btnCancel.Text = "Cancel";
			_btnCancel.UseVisualStyleBackColor = true;
			_btnCancel.Click += btnCancel_Click;

			// RoutineSelectionForm
			AcceptButton = _btnSelect;
			AutoScaleDimensions = new SizeF(6f, 13f);
			AutoScaleMode = AutoScaleMode.Font;
			CancelButton = _btnCancel;
			ClientSize = new Size(405, 232);
			Controls.Add(_btnCancel);
			Controls.Add(_btnSelect);
			Controls.Add(_groupBox1);
			Controls.Add(_label1);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "RoutineSelectionForm";
			ShowIcon = false;
			ShowInTaskbar = false;
			Text = "Select a custom class";
			TopMost = true;
			_groupBox1.ResumeLayout(false);
			ResumeLayout(false);
		}
	}
}
