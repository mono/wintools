using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Mfconsulting.Vsprj2make
{
	/// <summary>
	/// Summary description for OptionsDlg.
	/// </summary>
	public class OptionsDlg : System.Windows.Forms.Form
	{
		private Mfconsulting.Vsprj2make.RegistryHelper m_regH = null;
		private string m_strMonoBasePath;

		private System.Windows.Forms.Panel m_PanelTabs;
		private System.Windows.Forms.Panel m_panelButtons;
		private System.Windows.Forms.TabControl m_tabControl;
		private System.Windows.Forms.TabPage m_tabPage1;
		private System.Windows.Forms.TabPage m_tabPage2;
		private System.Windows.Forms.Button m_btnOK;
		private System.Windows.Forms.Button m_btnCancel;
		private System.Windows.Forms.TabPage m_tabPage3;
		private System.Windows.Forms.TabPage m_tabPage4;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox m_edtMonoBasePath;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox m_edtDirectories;
		private System.Windows.Forms.TextBox m_edtExtensions;
		private System.Windows.Forms.NumericUpDown m_nupdwnCompressionLevel;
		private System.Windows.Forms.TextBox m_edtLibPath;
		private System.Windows.Forms.NumericUpDown m_nupdwnPort;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.RadioButton radioButton2;

		public string MonoBasePath
		{
			get { return m_strMonoBasePath; }
			set { m_strMonoBasePath = value; }
		}
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public OptionsDlg()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Read from the Mono Path from the registry
			try
			{
				m_regH = new RegistryHelper();
			}
			catch(Exception exc)
			{
				MessageBox.Show(this, exc.Message,"Prj2make", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			UpdateData(false);

		}

		protected void UpdateData(bool bIsSaving)
		{
			if(bIsSaving)
			{
				// Saving values to registry
				m_regH.CompressionLevel = (int)m_nupdwnCompressionLevel.Value;
				m_regH.IgnoredDirectories = m_edtDirectories.Text;
				m_regH.IgnoredExtensions = m_edtExtensions.Text;

				m_regH.MonoLibPath = m_edtLibPath.Text;
				
				m_regH.Port = (int)m_nupdwnPort.Value;
				m_regH.XspExeSelection = (radioButton1.Checked == true ? 1 : 2);
			}
			else
			{
				// Loading values from registry
				try
				{
					// Read from the Mono Path from the registry
					m_edtMonoBasePath.Text = m_regH.GetMonoBasePath();
					
					m_edtDirectories.Text = m_regH.IgnoredDirectories;
					m_edtExtensions.Text = m_regH.IgnoredExtensions;
					m_nupdwnCompressionLevel.Value = (decimal)m_regH.CompressionLevel;
					
					m_edtLibPath.Text = m_regH.MonoLibPath;

					m_nupdwnPort.Value = (decimal)m_regH.Port;
					if(m_regH.XspExeSelection == 1)
						radioButton1.Checked = true;
					else
						radioButton2.Checked = true;
				}
				catch(Exception exc)
				{
					MessageBox.Show(this, exc.Message,"Prj2make", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.m_PanelTabs = new System.Windows.Forms.Panel();
			this.m_tabControl = new System.Windows.Forms.TabControl();
			this.m_tabPage3 = new System.Windows.Forms.TabPage();
			this.m_edtMonoBasePath = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.m_tabPage1 = new System.Windows.Forms.TabPage();
			this.button2 = new System.Windows.Forms.Button();
			this.m_edtDirectories = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.m_edtExtensions = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.m_nupdwnCompressionLevel = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.m_tabPage4 = new System.Windows.Forms.TabPage();
			this.m_edtLibPath = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.m_tabPage2 = new System.Windows.Forms.TabPage();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.m_nupdwnPort = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.m_panelButtons = new System.Windows.Forms.Panel();
			this.m_btnCancel = new System.Windows.Forms.Button();
			this.m_btnOK = new System.Windows.Forms.Button();
			this.m_PanelTabs.SuspendLayout();
			this.m_tabControl.SuspendLayout();
			this.m_tabPage3.SuspendLayout();
			this.m_tabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_nupdwnCompressionLevel)).BeginInit();
			this.m_tabPage4.SuspendLayout();
			this.m_tabPage2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_nupdwnPort)).BeginInit();
			this.m_panelButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_PanelTabs
			// 
			this.m_PanelTabs.Controls.Add(this.m_tabControl);
			this.m_PanelTabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_PanelTabs.Location = new System.Drawing.Point(0, 0);
			this.m_PanelTabs.Name = "m_PanelTabs";
			this.m_PanelTabs.Size = new System.Drawing.Size(456, 246);
			this.m_PanelTabs.TabIndex = 0;
			// 
			// m_tabControl
			// 
			this.m_tabControl.Controls.Add(this.m_tabPage3);
			this.m_tabControl.Controls.Add(this.m_tabPage1);
			this.m_tabControl.Controls.Add(this.m_tabPage4);
			this.m_tabControl.Controls.Add(this.m_tabPage2);
			this.m_tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_tabControl.Location = new System.Drawing.Point(0, 0);
			this.m_tabControl.Name = "m_tabControl";
			this.m_tabControl.SelectedIndex = 0;
			this.m_tabControl.Size = new System.Drawing.Size(456, 246);
			this.m_tabControl.TabIndex = 0;
			// 
			// m_tabPage3
			// 
			this.m_tabPage3.Controls.Add(this.m_edtMonoBasePath);
			this.m_tabPage3.Controls.Add(this.label1);
			this.m_tabPage3.Location = new System.Drawing.Point(4, 22);
			this.m_tabPage3.Name = "m_tabPage3";
			this.m_tabPage3.Size = new System.Drawing.Size(448, 220);
			this.m_tabPage3.TabIndex = 2;
			this.m_tabPage3.Text = "Mono";
			// 
			// m_edtMonoBasePath
			// 
			this.m_edtMonoBasePath.Location = new System.Drawing.Point(16, 48);
			this.m_edtMonoBasePath.Name = "m_edtMonoBasePath";
			this.m_edtMonoBasePath.ReadOnly = true;
			this.m_edtMonoBasePath.Size = new System.Drawing.Size(264, 20);
			this.m_edtMonoBasePath.TabIndex = 1;
			this.m_edtMonoBasePath.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Mono Base Path";
			// 
			// m_tabPage1
			// 
			this.m_tabPage1.Controls.Add(this.button2);
			this.m_tabPage1.Controls.Add(this.m_edtDirectories);
			this.m_tabPage1.Controls.Add(this.label4);
			this.m_tabPage1.Controls.Add(this.button1);
			this.m_tabPage1.Controls.Add(this.m_edtExtensions);
			this.m_tabPage1.Controls.Add(this.label3);
			this.m_tabPage1.Controls.Add(this.m_nupdwnCompressionLevel);
			this.m_tabPage1.Controls.Add(this.label2);
			this.m_tabPage1.Location = new System.Drawing.Point(4, 22);
			this.m_tabPage1.Name = "m_tabPage1";
			this.m_tabPage1.Size = new System.Drawing.Size(448, 220);
			this.m_tabPage1.TabIndex = 0;
			this.m_tabPage1.Text = "Packaging";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(408, 128);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(24, 23);
			this.button2.TabIndex = 8;
			this.button2.Text = "...";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// m_edtDirectories
			// 
			this.m_edtDirectories.Location = new System.Drawing.Point(16, 128);
			this.m_edtDirectories.Name = "m_edtDirectories";
			this.m_edtDirectories.Size = new System.Drawing.Size(384, 20);
			this.m_edtDirectories.TabIndex = 7;
			this.m_edtDirectories.Text = "";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 104);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(104, 23);
			this.label4.TabIndex = 6;
			this.label4.Text = "Ignored directories";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(408, 72);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(24, 23);
			this.button1.TabIndex = 5;
			this.button1.Text = "...";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// m_edtExtensions
			// 
			this.m_edtExtensions.Location = new System.Drawing.Point(16, 72);
			this.m_edtExtensions.Name = "m_edtExtensions";
			this.m_edtExtensions.Size = new System.Drawing.Size(384, 20);
			this.m_edtExtensions.TabIndex = 4;
			this.m_edtExtensions.Text = "";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 48);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(104, 23);
			this.label3.TabIndex = 3;
			this.label3.Text = "Ignored extensions";
			// 
			// m_nupdwnCompressionLevel
			// 
			this.m_nupdwnCompressionLevel.Location = new System.Drawing.Point(128, 16);
			this.m_nupdwnCompressionLevel.Maximum = new System.Decimal(new int[] {
																					 9,
																					 0,
																					 0,
																					 0});
			this.m_nupdwnCompressionLevel.Name = "m_nupdwnCompressionLevel";
			this.m_nupdwnCompressionLevel.Size = new System.Drawing.Size(40, 20);
			this.m_nupdwnCompressionLevel.TabIndex = 2;
			this.m_nupdwnCompressionLevel.Value = new System.Decimal(new int[] {
																				   4,
																				   0,
																				   0,
																				   0});
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(104, 23);
			this.label2.TabIndex = 0;
			this.label2.Text = "Compression Level";
			// 
			// m_tabPage4
			// 
			this.m_tabPage4.Controls.Add(this.m_edtLibPath);
			this.m_tabPage4.Controls.Add(this.label6);
			this.m_tabPage4.Location = new System.Drawing.Point(4, 22);
			this.m_tabPage4.Name = "m_tabPage4";
			this.m_tabPage4.Size = new System.Drawing.Size(448, 220);
			this.m_tabPage4.TabIndex = 3;
			this.m_tabPage4.Text = "Prj2Make#";
			// 
			// m_edtLibPath
			// 
			this.m_edtLibPath.Location = new System.Drawing.Point(16, 48);
			this.m_edtLibPath.Name = "m_edtLibPath";
			this.m_edtLibPath.Size = new System.Drawing.Size(384, 20);
			this.m_edtLibPath.TabIndex = 1;
			this.m_edtLibPath.Text = "";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(16, 24);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(152, 16);
			this.label6.TabIndex = 0;
			this.label6.Text = "Lib path (for gmake makefile)";
			// 
			// m_tabPage2
			// 
			this.m_tabPage2.Controls.Add(this.groupBox1);
			this.m_tabPage2.Controls.Add(this.m_nupdwnPort);
			this.m_tabPage2.Controls.Add(this.label5);
			this.m_tabPage2.Location = new System.Drawing.Point(4, 22);
			this.m_tabPage2.Name = "m_tabPage2";
			this.m_tabPage2.Size = new System.Drawing.Size(448, 220);
			this.m_tabPage2.TabIndex = 1;
			this.m_tabPage2.Text = "XSP";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radioButton2);
			this.groupBox1.Controls.Add(this.radioButton1);
			this.groupBox1.Location = new System.Drawing.Point(16, 56);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(176, 96);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "XSP selection";
			// 
			// radioButton2
			// 
			this.radioButton2.Location = new System.Drawing.Point(24, 56);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.TabIndex = 1;
			this.radioButton2.Text = "XSP&2";
			// 
			// radioButton1
			// 
			this.radioButton1.Location = new System.Drawing.Point(24, 24);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.TabIndex = 0;
			this.radioButton1.Text = "&XSP";
			// 
			// m_nupdwnPort
			// 
			this.m_nupdwnPort.Location = new System.Drawing.Point(56, 16);
			this.m_nupdwnPort.Maximum = new System.Decimal(new int[] {
																		 65534,
																		 0,
																		 0,
																		 0});
			this.m_nupdwnPort.Minimum = new System.Decimal(new int[] {
																		 8102,
																		 0,
																		 0,
																		 0});
			this.m_nupdwnPort.Name = "m_nupdwnPort";
			this.m_nupdwnPort.Size = new System.Drawing.Size(48, 20);
			this.m_nupdwnPort.TabIndex = 1;
			this.m_nupdwnPort.Value = new System.Decimal(new int[] {
																	   8189,
																	   0,
																	   0,
																	   0});
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(16, 16);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(32, 23);
			this.label5.TabIndex = 0;
			this.label5.Text = "Port";
			// 
			// m_panelButtons
			// 
			this.m_panelButtons.Controls.Add(this.m_btnCancel);
			this.m_panelButtons.Controls.Add(this.m_btnOK);
			this.m_panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.m_panelButtons.Location = new System.Drawing.Point(0, 190);
			this.m_panelButtons.Name = "m_panelButtons";
			this.m_panelButtons.Size = new System.Drawing.Size(456, 56);
			this.m_panelButtons.TabIndex = 1;
			// 
			// m_btnCancel
			// 
			this.m_btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_btnCancel.Location = new System.Drawing.Point(368, 16);
			this.m_btnCancel.Name = "m_btnCancel";
			this.m_btnCancel.TabIndex = 1;
			this.m_btnCancel.Text = "Cancel";
			// 
			// m_btnOK
			// 
			this.m_btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_btnOK.Location = new System.Drawing.Point(280, 16);
			this.m_btnOK.Name = "m_btnOK";
			this.m_btnOK.TabIndex = 0;
			this.m_btnOK.Text = "OK";
			this.m_btnOK.Click += new System.EventHandler(this.m_btnOK_Click);
			// 
			// OptionsDlg
			// 
			this.AcceptButton = this.m_btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_btnCancel;
			this.ClientSize = new System.Drawing.Size(456, 246);
			this.Controls.Add(this.m_panelButtons);
			this.Controls.Add(this.m_PanelTabs);
			this.Name = "OptionsDlg";
			this.Text = "Prj2Make# Options";
			this.Load += new System.EventHandler(this.OptionsDlg_Load);
			this.m_PanelTabs.ResumeLayout(false);
			this.m_tabControl.ResumeLayout(false);
			this.m_tabPage3.ResumeLayout(false);
			this.m_tabPage1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_nupdwnCompressionLevel)).EndInit();
			this.m_tabPage4.ResumeLayout(false);
			this.m_tabPage2.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_nupdwnPort)).EndInit();
			this.m_panelButtons.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void m_btnOK_Click(object sender, System.EventArgs e)
		{
			UpdateData(true);
			this.Close();
		}

		private void OptionsDlg_Load(object sender, System.EventArgs e)
		{
			this.UpdateData(false);
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show(
				this,
				"Not implemented yet.",
				"Prj2Make-Sharp",
				MessageBoxButtons.OK,
				MessageBoxIcon.Information
				);
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show(
				this,
				"Not implemented yet.",
				"Prj2Make-Sharp",
				MessageBoxButtons.OK,
				MessageBoxIcon.Information
				);
		}

	}
}
