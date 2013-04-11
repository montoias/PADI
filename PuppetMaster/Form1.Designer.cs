namespace PuppetMaster
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LaunchClientServerButton = new System.Windows.Forms.Button();
            this.OpenFileButton = new System.Windows.Forms.Button();
            this.ClientEventBox = new System.Windows.Forms.TextBox();
            this.LaunchMetadataServerButton = new System.Windows.Forms.Button();
            this.CreateFileButton = new System.Windows.Forms.Button();
            this.FilenameBox = new System.Windows.Forms.TextBox();
            this.FilenameLabel = new System.Windows.Forms.Label();
            this.DeleteFileButton = new System.Windows.Forms.Button();
            this.CloseFileButton = new System.Windows.Forms.Button();
            this.MetadataOption1 = new System.Windows.Forms.RadioButton();
            this.MetadataOption2 = new System.Windows.Forms.RadioButton();
            this.MetadataOption3 = new System.Windows.Forms.RadioButton();
            this.FailMetadataServerButton = new System.Windows.Forms.Button();
            this.RecoverMetadataServerButton = new System.Windows.Forms.Button();
            this.ClientListBox = new System.Windows.Forms.ListBox();
            this.ClientGroupBox = new System.Windows.Forms.GroupBox();
            this.DumpClientButton = new System.Windows.Forms.Button();
            this.UseTextFileOption = new System.Windows.Forms.RadioButton();
            this.ByteRegisterOption = new System.Windows.Forms.RadioButton();
            this.FileRegisterTextBox = new System.Windows.Forms.TextBox();
            this.ByteRegisterTextBox = new System.Windows.Forms.TextBox();
            this.ByteRegisterLabel = new System.Windows.Forms.Label();
            this.FileRegisterLabel = new System.Windows.Forms.Label();
            this.SemanticsLabel = new System.Windows.Forms.Label();
            this.FileTextbox = new System.Windows.Forms.TextBox();
            this.WriteQuorumTextBox = new System.Windows.Forms.TextBox();
            this.ReadQuorumTextBox = new System.Windows.Forms.TextBox();
            this.NServersTextBox = new System.Windows.Forms.TextBox();
            this.ReadQuorumLabel = new System.Windows.Forms.Label();
            this.WriteQuorumLabel = new System.Windows.Forms.Label();
            this.NServersLabel = new System.Windows.Forms.Label();
            this.WriteFileButton = new System.Windows.Forms.Button();
            this.ReadFileButton = new System.Windows.Forms.Button();
            this.MetadataServerGroupBox = new System.Windows.Forms.GroupBox();
            this.DumpMetadataButton = new System.Windows.Forms.Button();
            this.DataServerGroupBox = new System.Windows.Forms.GroupBox();
            this.DumpDataServerButton = new System.Windows.Forms.Button();
            this.LaunchDataServerButton = new System.Windows.Forms.Button();
            this.DataServerListBox = new System.Windows.Forms.ListBox();
            this.UnfreezeDataServerButton = new System.Windows.Forms.Button();
            this.FreezeDataServerButton = new System.Windows.Forms.Button();
            this.RecoverDataServerButton = new System.Windows.Forms.Button();
            this.FailDataServerButton = new System.Windows.Forms.Button();
            this.MetadataEventBox = new System.Windows.Forms.TextBox();
            this.KillProcessesbutton = new System.Windows.Forms.Button();
            this.ScriptBox = new System.Windows.Forms.GroupBox();
            this.NextStepButton = new System.Windows.Forms.Button();
            this.RunScriptButton = new System.Windows.Forms.Button();
            this.ScriptFileBox = new System.Windows.Forms.TextBox();
            this.ScriptFileLabel = new System.Windows.Forms.Label();
            this.LoadScriptButton = new System.Windows.Forms.Button();
            this.DataServerEventBox = new System.Windows.Forms.TextBox();
            this.ClientEventsLabel = new System.Windows.Forms.Label();
            this.MetadataEventsLabel = new System.Windows.Forms.Label();
            this.DataServerEventsLabel = new System.Windows.Forms.Label();
            this.SemanticsBox = new System.Windows.Forms.ComboBox();
            this.ClientGroupBox.SuspendLayout();
            this.MetadataServerGroupBox.SuspendLayout();
            this.DataServerGroupBox.SuspendLayout();
            this.ScriptBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // LaunchClientServerButton
            // 
            this.LaunchClientServerButton.Location = new System.Drawing.Point(18, 284);
            this.LaunchClientServerButton.Name = "LaunchClientServerButton";
            this.LaunchClientServerButton.Size = new System.Drawing.Size(85, 29);
            this.LaunchClientServerButton.TabIndex = 0;
            this.LaunchClientServerButton.Text = "Launch Client";
            this.LaunchClientServerButton.UseVisualStyleBackColor = true;
            this.LaunchClientServerButton.Click += new System.EventHandler(this.LaunchClient_Click);
            // 
            // OpenFileButton
            // 
            this.OpenFileButton.Location = new System.Drawing.Point(118, 166);
            this.OpenFileButton.Name = "OpenFileButton";
            this.OpenFileButton.Size = new System.Drawing.Size(85, 30);
            this.OpenFileButton.TabIndex = 1;
            this.OpenFileButton.Text = "Open File";
            this.OpenFileButton.UseVisualStyleBackColor = true;
            this.OpenFileButton.Click += new System.EventHandler(this.OpenFile_Click);
            // 
            // ClientEventBox
            // 
            this.ClientEventBox.Location = new System.Drawing.Point(12, 391);
            this.ClientEventBox.Multiline = true;
            this.ClientEventBox.Name = "ClientEventBox";
            this.ClientEventBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ClientEventBox.Size = new System.Drawing.Size(270, 168);
            this.ClientEventBox.TabIndex = 2;
            // 
            // LaunchMetadataServerButton
            // 
            this.LaunchMetadataServerButton.Location = new System.Drawing.Point(156, 260);
            this.LaunchMetadataServerButton.Name = "LaunchMetadataServerButton";
            this.LaunchMetadataServerButton.Size = new System.Drawing.Size(106, 30);
            this.LaunchMetadataServerButton.TabIndex = 3;
            this.LaunchMetadataServerButton.Text = "Launch Metadata ";
            this.LaunchMetadataServerButton.UseVisualStyleBackColor = true;
            this.LaunchMetadataServerButton.Click += new System.EventHandler(this.LaunchMetadataButton_Click);
            // 
            // CreateFileButton
            // 
            this.CreateFileButton.Location = new System.Drawing.Point(18, 166);
            this.CreateFileButton.Name = "CreateFileButton";
            this.CreateFileButton.Size = new System.Drawing.Size(85, 30);
            this.CreateFileButton.TabIndex = 4;
            this.CreateFileButton.Text = "Create File";
            this.CreateFileButton.UseVisualStyleBackColor = true;
            this.CreateFileButton.Click += new System.EventHandler(this.CreateFileButton_Click);
            // 
            // FilenameBox
            // 
            this.FilenameBox.Location = new System.Drawing.Point(202, 19);
            this.FilenameBox.Multiline = true;
            this.FilenameBox.Name = "FilenameBox";
            this.FilenameBox.Size = new System.Drawing.Size(124, 27);
            this.FilenameBox.TabIndex = 5;
            // 
            // FilenameLabel
            // 
            this.FilenameLabel.AutoSize = true;
            this.FilenameLabel.Location = new System.Drawing.Point(142, 22);
            this.FilenameLabel.Name = "FilenameLabel";
            this.FilenameLabel.Size = new System.Drawing.Size(49, 13);
            this.FilenameLabel.TabIndex = 6;
            this.FilenameLabel.Text = "Filename";
            // 
            // DeleteFileButton
            // 
            this.DeleteFileButton.Location = new System.Drawing.Point(18, 213);
            this.DeleteFileButton.Name = "DeleteFileButton";
            this.DeleteFileButton.Size = new System.Drawing.Size(85, 30);
            this.DeleteFileButton.TabIndex = 7;
            this.DeleteFileButton.Text = "Delete File";
            this.DeleteFileButton.UseVisualStyleBackColor = true;
            this.DeleteFileButton.Click += new System.EventHandler(this.DeleteFileButton_Click);
            // 
            // CloseFileButton
            // 
            this.CloseFileButton.Location = new System.Drawing.Point(116, 213);
            this.CloseFileButton.Name = "CloseFileButton";
            this.CloseFileButton.Size = new System.Drawing.Size(85, 30);
            this.CloseFileButton.TabIndex = 8;
            this.CloseFileButton.Text = "Close File";
            this.CloseFileButton.UseVisualStyleBackColor = true;
            this.CloseFileButton.Click += new System.EventHandler(this.CloseFileButton_Click);
            // 
            // MetadataOption1
            // 
            this.MetadataOption1.AutoSize = true;
            this.MetadataOption1.Checked = true;
            this.MetadataOption1.Location = new System.Drawing.Point(149, 23);
            this.MetadataOption1.Name = "MetadataOption1";
            this.MetadataOption1.Size = new System.Drawing.Size(113, 17);
            this.MetadataOption1.TabIndex = 13;
            this.MetadataOption1.TabStop = true;
            this.MetadataOption1.Text = "Metadata Server 0";
            this.MetadataOption1.UseVisualStyleBackColor = true;
            this.MetadataOption1.CheckedChanged += new System.EventHandler(this.MetadataOption1_CheckedChanged);
            // 
            // MetadataOption2
            // 
            this.MetadataOption2.AutoSize = true;
            this.MetadataOption2.Location = new System.Drawing.Point(149, 57);
            this.MetadataOption2.Name = "MetadataOption2";
            this.MetadataOption2.Size = new System.Drawing.Size(113, 17);
            this.MetadataOption2.TabIndex = 14;
            this.MetadataOption2.Text = "Metadata Server 1";
            this.MetadataOption2.UseVisualStyleBackColor = true;
            this.MetadataOption2.CheckedChanged += new System.EventHandler(this.MetadataOption2_CheckedChanged);
            // 
            // MetadataOption3
            // 
            this.MetadataOption3.AutoSize = true;
            this.MetadataOption3.Location = new System.Drawing.Point(149, 92);
            this.MetadataOption3.Name = "MetadataOption3";
            this.MetadataOption3.Size = new System.Drawing.Size(113, 17);
            this.MetadataOption3.TabIndex = 15;
            this.MetadataOption3.Text = "Metadata Server 2";
            this.MetadataOption3.UseVisualStyleBackColor = true;
            this.MetadataOption3.CheckedChanged += new System.EventHandler(this.MetadataOption3_CheckedChanged);
            // 
            // FailMetadataServerButton
            // 
            this.FailMetadataServerButton.Location = new System.Drawing.Point(19, 28);
            this.FailMetadataServerButton.Name = "FailMetadataServerButton";
            this.FailMetadataServerButton.Size = new System.Drawing.Size(98, 30);
            this.FailMetadataServerButton.TabIndex = 16;
            this.FailMetadataServerButton.Text = "Fail Server";
            this.FailMetadataServerButton.UseVisualStyleBackColor = true;
            this.FailMetadataServerButton.Click += new System.EventHandler(this.FailMetadataButton_Click);
            // 
            // RecoverMetadataServerButton
            // 
            this.RecoverMetadataServerButton.Location = new System.Drawing.Point(19, 67);
            this.RecoverMetadataServerButton.Name = "RecoverMetadataServerButton";
            this.RecoverMetadataServerButton.Size = new System.Drawing.Size(98, 30);
            this.RecoverMetadataServerButton.TabIndex = 17;
            this.RecoverMetadataServerButton.Text = "Recover Server";
            this.RecoverMetadataServerButton.UseVisualStyleBackColor = true;
            this.RecoverMetadataServerButton.Click += new System.EventHandler(this.RecoverMetadataServerButton_Click);
            // 
            // ClientListBox
            // 
            this.ClientListBox.FormattingEnabled = true;
            this.ClientListBox.Location = new System.Drawing.Point(18, 29);
            this.ClientListBox.Name = "ClientListBox";
            this.ClientListBox.Size = new System.Drawing.Size(81, 134);
            this.ClientListBox.TabIndex = 18;
            // 
            // ClientGroupBox
            // 
            this.ClientGroupBox.Controls.Add(this.SemanticsBox);
            this.ClientGroupBox.Controls.Add(this.DumpClientButton);
            this.ClientGroupBox.Controls.Add(this.UseTextFileOption);
            this.ClientGroupBox.Controls.Add(this.ByteRegisterOption);
            this.ClientGroupBox.Controls.Add(this.FileRegisterTextBox);
            this.ClientGroupBox.Controls.Add(this.ByteRegisterTextBox);
            this.ClientGroupBox.Controls.Add(this.ByteRegisterLabel);
            this.ClientGroupBox.Controls.Add(this.FileRegisterLabel);
            this.ClientGroupBox.Controls.Add(this.SemanticsLabel);
            this.ClientGroupBox.Controls.Add(this.FileTextbox);
            this.ClientGroupBox.Controls.Add(this.WriteQuorumTextBox);
            this.ClientGroupBox.Controls.Add(this.ReadQuorumTextBox);
            this.ClientGroupBox.Controls.Add(this.NServersTextBox);
            this.ClientGroupBox.Controls.Add(this.ReadQuorumLabel);
            this.ClientGroupBox.Controls.Add(this.WriteQuorumLabel);
            this.ClientGroupBox.Controls.Add(this.NServersLabel);
            this.ClientGroupBox.Controls.Add(this.WriteFileButton);
            this.ClientGroupBox.Controls.Add(this.ClientListBox);
            this.ClientGroupBox.Controls.Add(this.ReadFileButton);
            this.ClientGroupBox.Controls.Add(this.FilenameBox);
            this.ClientGroupBox.Controls.Add(this.FilenameLabel);
            this.ClientGroupBox.Controls.Add(this.OpenFileButton);
            this.ClientGroupBox.Controls.Add(this.CreateFileButton);
            this.ClientGroupBox.Controls.Add(this.CloseFileButton);
            this.ClientGroupBox.Controls.Add(this.DeleteFileButton);
            this.ClientGroupBox.Controls.Add(this.LaunchClientServerButton);
            this.ClientGroupBox.Location = new System.Drawing.Point(12, 12);
            this.ClientGroupBox.Name = "ClientGroupBox";
            this.ClientGroupBox.Size = new System.Drawing.Size(577, 333);
            this.ClientGroupBox.TabIndex = 19;
            this.ClientGroupBox.TabStop = false;
            this.ClientGroupBox.Text = "Client Server";
            // 
            // DumpClientButton
            // 
            this.DumpClientButton.Location = new System.Drawing.Point(116, 284);
            this.DumpClientButton.Name = "DumpClientButton";
            this.DumpClientButton.Size = new System.Drawing.Size(87, 29);
            this.DumpClientButton.TabIndex = 35;
            this.DumpClientButton.Text = "Dump Client";
            this.DumpClientButton.UseVisualStyleBackColor = true;
            this.DumpClientButton.Click += new System.EventHandler(this.DumpClientButton_Click);
            // 
            // UseTextFileOption
            // 
            this.UseTextFileOption.AutoSize = true;
            this.UseTextFileOption.Checked = true;
            this.UseTextFileOption.Location = new System.Drawing.Point(450, 18);
            this.UseTextFileOption.Name = "UseTextFileOption";
            this.UseTextFileOption.Size = new System.Drawing.Size(87, 17);
            this.UseTextFileOption.TabIndex = 34;
            this.UseTextFileOption.TabStop = true;
            this.UseTextFileOption.Text = "Use Text File";
            this.UseTextFileOption.UseVisualStyleBackColor = true;
            this.UseTextFileOption.CheckedChanged += new System.EventHandler(this.UseTextFileOption_CheckedChanged);
            // 
            // ByteRegisterOption
            // 
            this.ByteRegisterOption.AutoSize = true;
            this.ByteRegisterOption.Location = new System.Drawing.Point(450, 48);
            this.ByteRegisterOption.Name = "ByteRegisterOption";
            this.ByteRegisterOption.Size = new System.Drawing.Size(110, 17);
            this.ByteRegisterOption.TabIndex = 33;
            this.ByteRegisterOption.Text = "Use Byte Register";
            this.ByteRegisterOption.UseVisualStyleBackColor = true;
            this.ByteRegisterOption.CheckedChanged += new System.EventHandler(this.ByteRegisterOption_CheckedChanged);
            // 
            // FileRegisterTextBox
            // 
            this.FileRegisterTextBox.Location = new System.Drawing.Point(515, 121);
            this.FileRegisterTextBox.Multiline = true;
            this.FileRegisterTextBox.Name = "FileRegisterTextBox";
            this.FileRegisterTextBox.Size = new System.Drawing.Size(34, 20);
            this.FileRegisterTextBox.TabIndex = 32;
            // 
            // ByteRegisterTextBox
            // 
            this.ByteRegisterTextBox.Location = new System.Drawing.Point(516, 92);
            this.ByteRegisterTextBox.Multiline = true;
            this.ByteRegisterTextBox.Name = "ByteRegisterTextBox";
            this.ByteRegisterTextBox.Size = new System.Drawing.Size(33, 20);
            this.ByteRegisterTextBox.TabIndex = 31;
            // 
            // ByteRegisterLabel
            // 
            this.ByteRegisterLabel.AutoSize = true;
            this.ByteRegisterLabel.Location = new System.Drawing.Point(384, 92);
            this.ByteRegisterLabel.Name = "ByteRegisterLabel";
            this.ByteRegisterLabel.Size = new System.Drawing.Size(70, 13);
            this.ByteRegisterLabel.TabIndex = 30;
            this.ByteRegisterLabel.Text = "Byte Register";
            // 
            // FileRegisterLabel
            // 
            this.FileRegisterLabel.AutoSize = true;
            this.FileRegisterLabel.Location = new System.Drawing.Point(384, 121);
            this.FileRegisterLabel.Name = "FileRegisterLabel";
            this.FileRegisterLabel.Size = new System.Drawing.Size(65, 13);
            this.FileRegisterLabel.TabIndex = 29;
            this.FileRegisterLabel.Text = "File Register";
            // 
            // SemanticsLabel
            // 
            this.SemanticsLabel.AutoSize = true;
            this.SemanticsLabel.Location = new System.Drawing.Point(384, 150);
            this.SemanticsLabel.Name = "SemanticsLabel";
            this.SemanticsLabel.Size = new System.Drawing.Size(56, 13);
            this.SemanticsLabel.TabIndex = 27;
            this.SemanticsLabel.Text = "Semantics";
            // 
            // FileTextbox
            // 
            this.FileTextbox.Location = new System.Drawing.Point(374, 200);
            this.FileTextbox.Multiline = true;
            this.FileTextbox.Name = "FileTextbox";
            this.FileTextbox.Size = new System.Drawing.Size(186, 113);
            this.FileTextbox.TabIndex = 26;
            // 
            // WriteQuorumTextBox
            // 
            this.WriteQuorumTextBox.Location = new System.Drawing.Point(202, 125);
            this.WriteQuorumTextBox.MaxLength = 2;
            this.WriteQuorumTextBox.Name = "WriteQuorumTextBox";
            this.WriteQuorumTextBox.Size = new System.Drawing.Size(35, 20);
            this.WriteQuorumTextBox.TabIndex = 25;
            this.WriteQuorumTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.checkNumeric);
            // 
            // ReadQuorumTextBox
            // 
            this.ReadQuorumTextBox.Location = new System.Drawing.Point(203, 89);
            this.ReadQuorumTextBox.MaxLength = 2;
            this.ReadQuorumTextBox.Name = "ReadQuorumTextBox";
            this.ReadQuorumTextBox.Size = new System.Drawing.Size(34, 20);
            this.ReadQuorumTextBox.TabIndex = 24;
            this.ReadQuorumTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.checkNumeric);
            // 
            // NServersTextBox
            // 
            this.NServersTextBox.Location = new System.Drawing.Point(202, 55);
            this.NServersTextBox.MaxLength = 2;
            this.NServersTextBox.Name = "NServersTextBox";
            this.NServersTextBox.Size = new System.Drawing.Size(35, 20);
            this.NServersTextBox.TabIndex = 23;
            this.NServersTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.checkNumeric);
            // 
            // ReadQuorumLabel
            // 
            this.ReadQuorumLabel.AutoSize = true;
            this.ReadQuorumLabel.Location = new System.Drawing.Point(168, 92);
            this.ReadQuorumLabel.Name = "ReadQuorumLabel";
            this.ReadQuorumLabel.Size = new System.Drawing.Size(23, 13);
            this.ReadQuorumLabel.TabIndex = 22;
            this.ReadQuorumLabel.Text = "RQ";
            // 
            // WriteQuorumLabel
            // 
            this.WriteQuorumLabel.AutoSize = true;
            this.WriteQuorumLabel.Location = new System.Drawing.Point(165, 128);
            this.WriteQuorumLabel.Name = "WriteQuorumLabel";
            this.WriteQuorumLabel.Size = new System.Drawing.Size(26, 13);
            this.WriteQuorumLabel.TabIndex = 21;
            this.WriteQuorumLabel.Text = "WQ";
            // 
            // NServersLabel
            // 
            this.NServersLabel.AutoSize = true;
            this.NServersLabel.Location = new System.Drawing.Point(140, 58);
            this.NServersLabel.Name = "NServersLabel";
            this.NServersLabel.Size = new System.Drawing.Size(51, 13);
            this.NServersLabel.TabIndex = 20;
            this.NServersLabel.Text = "NServers";
            // 
            // WriteFileButton
            // 
            this.WriteFileButton.Location = new System.Drawing.Point(220, 213);
            this.WriteFileButton.Name = "WriteFileButton";
            this.WriteFileButton.Size = new System.Drawing.Size(85, 30);
            this.WriteFileButton.TabIndex = 19;
            this.WriteFileButton.Text = "Write File";
            this.WriteFileButton.UseVisualStyleBackColor = true;
            this.WriteFileButton.Click += new System.EventHandler(this.WriteFileButton_Click);
            // 
            // ReadFileButton
            // 
            this.ReadFileButton.Location = new System.Drawing.Point(220, 166);
            this.ReadFileButton.Name = "ReadFileButton";
            this.ReadFileButton.Size = new System.Drawing.Size(85, 30);
            this.ReadFileButton.TabIndex = 19;
            this.ReadFileButton.Text = "Read File";
            this.ReadFileButton.UseVisualStyleBackColor = true;
            this.ReadFileButton.Click += new System.EventHandler(this.ReadFileButton_Click);
            // 
            // MetadataServerGroupBox
            // 
            this.MetadataServerGroupBox.Controls.Add(this.DumpMetadataButton);
            this.MetadataServerGroupBox.Controls.Add(this.MetadataOption2);
            this.MetadataServerGroupBox.Controls.Add(this.MetadataOption1);
            this.MetadataServerGroupBox.Controls.Add(this.RecoverMetadataServerButton);
            this.MetadataServerGroupBox.Controls.Add(this.MetadataOption3);
            this.MetadataServerGroupBox.Controls.Add(this.FailMetadataServerButton);
            this.MetadataServerGroupBox.Controls.Add(this.LaunchMetadataServerButton);
            this.MetadataServerGroupBox.Location = new System.Drawing.Point(611, 17);
            this.MetadataServerGroupBox.Name = "MetadataServerGroupBox";
            this.MetadataServerGroupBox.Size = new System.Drawing.Size(293, 328);
            this.MetadataServerGroupBox.TabIndex = 20;
            this.MetadataServerGroupBox.TabStop = false;
            this.MetadataServerGroupBox.Text = "Metadata Server";
            // 
            // DumpMetadataButton
            // 
            this.DumpMetadataButton.Location = new System.Drawing.Point(38, 260);
            this.DumpMetadataButton.Name = "DumpMetadataButton";
            this.DumpMetadataButton.Size = new System.Drawing.Size(106, 30);
            this.DumpMetadataButton.TabIndex = 18;
            this.DumpMetadataButton.Text = "Dump Metadata";
            this.DumpMetadataButton.UseVisualStyleBackColor = true;
            this.DumpMetadataButton.Click += new System.EventHandler(this.DumpMetadataButton_Click);
            // 
            // DataServerGroupBox
            // 
            this.DataServerGroupBox.Controls.Add(this.DumpDataServerButton);
            this.DataServerGroupBox.Controls.Add(this.LaunchDataServerButton);
            this.DataServerGroupBox.Controls.Add(this.DataServerListBox);
            this.DataServerGroupBox.Controls.Add(this.UnfreezeDataServerButton);
            this.DataServerGroupBox.Controls.Add(this.FreezeDataServerButton);
            this.DataServerGroupBox.Controls.Add(this.RecoverDataServerButton);
            this.DataServerGroupBox.Controls.Add(this.FailDataServerButton);
            this.DataServerGroupBox.Location = new System.Drawing.Point(934, 13);
            this.DataServerGroupBox.Name = "DataServerGroupBox";
            this.DataServerGroupBox.Size = new System.Drawing.Size(294, 332);
            this.DataServerGroupBox.TabIndex = 21;
            this.DataServerGroupBox.TabStop = false;
            this.DataServerGroupBox.Text = "Data Server";
            // 
            // DumpDataServerButton
            // 
            this.DumpDataServerButton.Location = new System.Drawing.Point(157, 264);
            this.DumpDataServerButton.Name = "DumpDataServerButton";
            this.DumpDataServerButton.Size = new System.Drawing.Size(108, 30);
            this.DumpDataServerButton.TabIndex = 24;
            this.DumpDataServerButton.Text = "Dump Data Server";
            this.DumpDataServerButton.UseVisualStyleBackColor = true;
            this.DumpDataServerButton.Click += new System.EventHandler(this.DumpDataServerButton_Click);
            // 
            // LaunchDataServerButton
            // 
            this.LaunchDataServerButton.Location = new System.Drawing.Point(22, 264);
            this.LaunchDataServerButton.Name = "LaunchDataServerButton";
            this.LaunchDataServerButton.Size = new System.Drawing.Size(113, 31);
            this.LaunchDataServerButton.TabIndex = 23;
            this.LaunchDataServerButton.Text = "Launch Data Server";
            this.LaunchDataServerButton.UseVisualStyleBackColor = true;
            this.LaunchDataServerButton.Click += new System.EventHandler(this.LaunchDataServerButton_Click);
            // 
            // DataServerListBox
            // 
            this.DataServerListBox.FormattingEnabled = true;
            this.DataServerListBox.Location = new System.Drawing.Point(164, 32);
            this.DataServerListBox.Name = "DataServerListBox";
            this.DataServerListBox.Size = new System.Drawing.Size(101, 147);
            this.DataServerListBox.TabIndex = 22;
            // 
            // UnfreezeDataServerButton
            // 
            this.UnfreezeDataServerButton.Location = new System.Drawing.Point(23, 165);
            this.UnfreezeDataServerButton.Name = "UnfreezeDataServerButton";
            this.UnfreezeDataServerButton.Size = new System.Drawing.Size(94, 30);
            this.UnfreezeDataServerButton.TabIndex = 21;
            this.UnfreezeDataServerButton.Text = "Unfreeze Server";
            this.UnfreezeDataServerButton.UseVisualStyleBackColor = true;
            // 
            // FreezeDataServerButton
            // 
            this.FreezeDataServerButton.Location = new System.Drawing.Point(23, 118);
            this.FreezeDataServerButton.Name = "FreezeDataServerButton";
            this.FreezeDataServerButton.Size = new System.Drawing.Size(97, 30);
            this.FreezeDataServerButton.TabIndex = 20;
            this.FreezeDataServerButton.Text = "Freeze Server";
            this.FreezeDataServerButton.UseVisualStyleBackColor = true;
            // 
            // RecoverDataServerButton
            // 
            this.RecoverDataServerButton.Location = new System.Drawing.Point(23, 76);
            this.RecoverDataServerButton.Name = "RecoverDataServerButton";
            this.RecoverDataServerButton.Size = new System.Drawing.Size(98, 30);
            this.RecoverDataServerButton.TabIndex = 19;
            this.RecoverDataServerButton.Text = "Recover Server";
            this.RecoverDataServerButton.UseVisualStyleBackColor = true;
            this.RecoverDataServerButton.Click += new System.EventHandler(this.RecoverDataServerButton_Click);
            // 
            // FailDataServerButton
            // 
            this.FailDataServerButton.Location = new System.Drawing.Point(22, 40);
            this.FailDataServerButton.Name = "FailDataServerButton";
            this.FailDataServerButton.Size = new System.Drawing.Size(98, 30);
            this.FailDataServerButton.TabIndex = 18;
            this.FailDataServerButton.Text = "Fail Server";
            this.FailDataServerButton.UseVisualStyleBackColor = true;
            this.FailDataServerButton.Click += new System.EventHandler(this.FailDataServerButton_Click);
            // 
            // MetadataEventBox
            // 
            this.MetadataEventBox.Location = new System.Drawing.Point(302, 391);
            this.MetadataEventBox.Multiline = true;
            this.MetadataEventBox.Name = "MetadataEventBox";
            this.MetadataEventBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.MetadataEventBox.Size = new System.Drawing.Size(270, 168);
            this.MetadataEventBox.TabIndex = 2;
            // 
            // KillProcessesbutton
            // 
            this.KillProcessesbutton.Location = new System.Drawing.Point(976, 584);
            this.KillProcessesbutton.Name = "KillProcessesbutton";
            this.KillProcessesbutton.Size = new System.Drawing.Size(96, 35);
            this.KillProcessesbutton.TabIndex = 22;
            this.KillProcessesbutton.Text = "Kill Processes";
            this.KillProcessesbutton.UseVisualStyleBackColor = true;
            this.KillProcessesbutton.Click += new System.EventHandler(this.KillProcessesButton_Click);
            // 
            // ScriptBox
            // 
            this.ScriptBox.Controls.Add(this.NextStepButton);
            this.ScriptBox.Controls.Add(this.RunScriptButton);
            this.ScriptBox.Controls.Add(this.ScriptFileBox);
            this.ScriptBox.Controls.Add(this.ScriptFileLabel);
            this.ScriptBox.Controls.Add(this.LoadScriptButton);
            this.ScriptBox.Location = new System.Drawing.Point(870, 372);
            this.ScriptBox.Name = "ScriptBox";
            this.ScriptBox.Size = new System.Drawing.Size(368, 187);
            this.ScriptBox.TabIndex = 23;
            this.ScriptBox.TabStop = false;
            this.ScriptBox.Text = "Script Options";
            // 
            // NextStepButton
            // 
            this.NextStepButton.Location = new System.Drawing.Point(19, 116);
            this.NextStepButton.Name = "NextStepButton";
            this.NextStepButton.Size = new System.Drawing.Size(88, 31);
            this.NextStepButton.TabIndex = 4;
            this.NextStepButton.Text = "Next Step";
            this.NextStepButton.UseVisualStyleBackColor = true;
            this.NextStepButton.Click += new System.EventHandler(this.NextStepButton_Click);
            // 
            // RunScriptButton
            // 
            this.RunScriptButton.Location = new System.Drawing.Point(19, 78);
            this.RunScriptButton.Name = "RunScriptButton";
            this.RunScriptButton.Size = new System.Drawing.Size(88, 32);
            this.RunScriptButton.TabIndex = 3;
            this.RunScriptButton.Text = "Run Script";
            this.RunScriptButton.UseVisualStyleBackColor = true;
            this.RunScriptButton.Click += new System.EventHandler(this.RunScriptButton_Click);
            // 
            // ScriptFileBox
            // 
            this.ScriptFileBox.Location = new System.Drawing.Point(225, 47);
            this.ScriptFileBox.Multiline = true;
            this.ScriptFileBox.Name = "ScriptFileBox";
            this.ScriptFileBox.Size = new System.Drawing.Size(109, 25);
            this.ScriptFileBox.TabIndex = 2;
            // 
            // ScriptFileLabel
            // 
            this.ScriptFileLabel.AutoSize = true;
            this.ScriptFileLabel.Location = new System.Drawing.Point(166, 50);
            this.ScriptFileLabel.Name = "ScriptFileLabel";
            this.ScriptFileLabel.Size = new System.Drawing.Size(53, 13);
            this.ScriptFileLabel.TabIndex = 1;
            this.ScriptFileLabel.Text = "Script File";
            // 
            // LoadScriptButton
            // 
            this.LoadScriptButton.Location = new System.Drawing.Point(19, 40);
            this.LoadScriptButton.Name = "LoadScriptButton";
            this.LoadScriptButton.Size = new System.Drawing.Size(88, 32);
            this.LoadScriptButton.TabIndex = 0;
            this.LoadScriptButton.Text = "Load Script";
            this.LoadScriptButton.UseVisualStyleBackColor = true;
            this.LoadScriptButton.Click += new System.EventHandler(this.LoadScriptButton_Click);
            // 
            // DataServerEventBox
            // 
            this.DataServerEventBox.Location = new System.Drawing.Point(592, 391);
            this.DataServerEventBox.Multiline = true;
            this.DataServerEventBox.Name = "DataServerEventBox";
            this.DataServerEventBox.Size = new System.Drawing.Size(263, 168);
            this.DataServerEventBox.TabIndex = 24;
            // 
            // ClientEventsLabel
            // 
            this.ClientEventsLabel.AutoSize = true;
            this.ClientEventsLabel.Location = new System.Drawing.Point(12, 375);
            this.ClientEventsLabel.Name = "ClientEventsLabel";
            this.ClientEventsLabel.Size = new System.Drawing.Size(72, 13);
            this.ClientEventsLabel.TabIndex = 25;
            this.ClientEventsLabel.Text = "Client Events:";
            // 
            // MetadataEventsLabel
            // 
            this.MetadataEventsLabel.AutoSize = true;
            this.MetadataEventsLabel.Location = new System.Drawing.Point(303, 372);
            this.MetadataEventsLabel.Name = "MetadataEventsLabel";
            this.MetadataEventsLabel.Size = new System.Drawing.Size(91, 13);
            this.MetadataEventsLabel.TabIndex = 26;
            this.MetadataEventsLabel.Text = "Metadata Events:";
            // 
            // DataServerEventsLabel
            // 
            this.DataServerEventsLabel.AutoSize = true;
            this.DataServerEventsLabel.Location = new System.Drawing.Point(596, 372);
            this.DataServerEventsLabel.Name = "DataServerEventsLabel";
            this.DataServerEventsLabel.Size = new System.Drawing.Size(103, 13);
            this.DataServerEventsLabel.TabIndex = 27;
            this.DataServerEventsLabel.Text = "Data Server Events:";
            // 
            // SemanticsBox
            // 
            this.SemanticsBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SemanticsBox.FormattingEnabled = true;
            this.SemanticsBox.Items.AddRange(new object[] {
            "default",
            "monotonic"});
            this.SemanticsBox.Location = new System.Drawing.Point(458, 147);
            this.SemanticsBox.Name = "SemanticsBox";
            this.SemanticsBox.Size = new System.Drawing.Size(91, 21);
            this.SemanticsBox.TabIndex = 36;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1250, 629);
            this.Controls.Add(this.DataServerEventsLabel);
            this.Controls.Add(this.MetadataEventsLabel);
            this.Controls.Add(this.ClientEventsLabel);
            this.Controls.Add(this.DataServerEventBox);
            this.Controls.Add(this.ScriptBox);
            this.Controls.Add(this.KillProcessesbutton);
            this.Controls.Add(this.DataServerGroupBox);
            this.Controls.Add(this.MetadataServerGroupBox);
            this.Controls.Add(this.ClientGroupBox);
            this.Controls.Add(this.MetadataEventBox);
            this.Controls.Add(this.ClientEventBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ClientGroupBox.ResumeLayout(false);
            this.ClientGroupBox.PerformLayout();
            this.MetadataServerGroupBox.ResumeLayout(false);
            this.MetadataServerGroupBox.PerformLayout();
            this.DataServerGroupBox.ResumeLayout(false);
            this.ScriptBox.ResumeLayout(false);
            this.ScriptBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LaunchClientServerButton;
        private System.Windows.Forms.Button OpenFileButton;
        private System.Windows.Forms.TextBox ClientEventBox;
        private System.Windows.Forms.Button LaunchMetadataServerButton;
        private System.Windows.Forms.Button CreateFileButton;
        private System.Windows.Forms.TextBox FilenameBox;
        private System.Windows.Forms.Label FilenameLabel;
        private System.Windows.Forms.Button DeleteFileButton;
        private System.Windows.Forms.Button CloseFileButton;
        private System.Windows.Forms.RadioButton MetadataOption1;
        private System.Windows.Forms.RadioButton MetadataOption2;
        private System.Windows.Forms.RadioButton MetadataOption3;
        private System.Windows.Forms.Button FailMetadataServerButton;
        private System.Windows.Forms.Button RecoverMetadataServerButton;
        private System.Windows.Forms.ListBox ClientListBox;
        private System.Windows.Forms.GroupBox ClientGroupBox;
        private System.Windows.Forms.Button WriteFileButton;
        private System.Windows.Forms.Button ReadFileButton;
        private System.Windows.Forms.GroupBox MetadataServerGroupBox;
        private System.Windows.Forms.GroupBox DataServerGroupBox;
        private System.Windows.Forms.TextBox MetadataEventBox;
        private System.Windows.Forms.Button DumpMetadataButton;
        private System.Windows.Forms.ListBox DataServerListBox;
        private System.Windows.Forms.Button UnfreezeDataServerButton;
        private System.Windows.Forms.Button FreezeDataServerButton;
        private System.Windows.Forms.Button RecoverDataServerButton;
        private System.Windows.Forms.Button FailDataServerButton;
        private System.Windows.Forms.Button LaunchDataServerButton;
        private System.Windows.Forms.Label WriteQuorumLabel;
        private System.Windows.Forms.Label NServersLabel;
        private System.Windows.Forms.TextBox WriteQuorumTextBox;
        private System.Windows.Forms.TextBox ReadQuorumTextBox;
        private System.Windows.Forms.TextBox NServersTextBox;
        private System.Windows.Forms.Label ReadQuorumLabel;
        private System.Windows.Forms.TextBox FileTextbox;
        private System.Windows.Forms.Button KillProcessesbutton;
        private System.Windows.Forms.Label SemanticsLabel;
        private System.Windows.Forms.GroupBox ScriptBox;
        private System.Windows.Forms.Button NextStepButton;
        private System.Windows.Forms.Button RunScriptButton;
        private System.Windows.Forms.TextBox ScriptFileBox;
        private System.Windows.Forms.Label ScriptFileLabel;
        private System.Windows.Forms.Button LoadScriptButton;
        private System.Windows.Forms.RadioButton UseTextFileOption;
        private System.Windows.Forms.RadioButton ByteRegisterOption;
        private System.Windows.Forms.TextBox FileRegisterTextBox;
        private System.Windows.Forms.TextBox ByteRegisterTextBox;
        private System.Windows.Forms.Label ByteRegisterLabel;
        private System.Windows.Forms.Label FileRegisterLabel;
        private System.Windows.Forms.Button DumpDataServerButton;
        private System.Windows.Forms.Button DumpClientButton;
        private System.Windows.Forms.TextBox DataServerEventBox;
        private System.Windows.Forms.Label ClientEventsLabel;
        private System.Windows.Forms.Label MetadataEventsLabel;
        private System.Windows.Forms.Label DataServerEventsLabel;
        private System.Windows.Forms.ComboBox SemanticsBox;
    }
}

