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
            this.EventBox = new System.Windows.Forms.TextBox();
            this.LaunchMetadataServerButton = new System.Windows.Forms.Button();
            this.FailMetadataServerButton = new System.Windows.Forms.Button();
            this.RecoverMetadataServerButton = new System.Windows.Forms.Button();
            this.ClientGroupBox = new System.Windows.Forms.GroupBox();
            this.ClientLabel = new System.Windows.Forms.Label();
            this.ClientBox = new System.Windows.Forms.ComboBox();
            this.MetadataServerGroupBox = new System.Windows.Forms.GroupBox();
            this.DumpMetadataButton = new System.Windows.Forms.Button();
            this.MetadataBox = new System.Windows.Forms.ComboBox();
            this.MetadataLabel = new System.Windows.Forms.Label();
            this.DataServerGroupBox = new System.Windows.Forms.GroupBox();
            this.DataServerLabel = new System.Windows.Forms.Label();
            this.DataServerBox = new System.Windows.Forms.ComboBox();
            this.DumpDataServerButton = new System.Windows.Forms.Button();
            this.LaunchDataServerButton = new System.Windows.Forms.Button();
            this.UnfreezeDataServerButton = new System.Windows.Forms.Button();
            this.FreezeDataServerButton = new System.Windows.Forms.Button();
            this.RecoverDataServerButton = new System.Windows.Forms.Button();
            this.FailDataServerButton = new System.Windows.Forms.Button();
            this.KillProcessesbutton = new System.Windows.Forms.Button();
            this.ScriptBox = new System.Windows.Forms.GroupBox();
            this.RunInstructionLineBox = new System.Windows.Forms.ComboBox();
            this.GoToLabel = new System.Windows.Forms.Label();
            this.NextInstructionLabel = new System.Windows.Forms.Label();
            this.CurrentInstructionLabel = new System.Windows.Forms.Label();
            this.NextStepButton = new System.Windows.Forms.Button();
            this.RunScriptButton = new System.Windows.Forms.Button();
            this.ScriptFileBox = new System.Windows.Forms.TextBox();
            this.ScriptFileLabel = new System.Windows.Forms.Label();
            this.LoadScriptButton = new System.Windows.Forms.Button();
            this.SemanticsBox = new System.Windows.Forms.ComboBox();
            this.ByteRegisterLabel = new System.Windows.Forms.Label();
            this.FileRegisterLabel = new System.Windows.Forms.Label();
            this.SemanticsLabel = new System.Windows.Forms.Label();
            this.FileTextbox = new System.Windows.Forms.TextBox();
            this.ReadQuorumLabel = new System.Windows.Forms.Label();
            this.WriteQuorumLabel = new System.Windows.Forms.Label();
            this.NServersLabel = new System.Windows.Forms.Label();
            this.FilenameBox = new System.Windows.Forms.TextBox();
            this.FileLabel = new System.Windows.Forms.Label();
            this.MetadataInfoGroupBox = new System.Windows.Forms.GroupBox();
            this.WriteQuorumBox = new System.Windows.Forms.ComboBox();
            this.ReadQuorumBox = new System.Windows.Forms.ComboBox();
            this.NServersBox = new System.Windows.Forms.ComboBox();
            this.FileInfoGroupBox = new System.Windows.Forms.GroupBox();
            this.WriteModeBox = new System.Windows.Forms.ComboBox();
            this.WriteModeLabel = new System.Windows.Forms.Label();
            this.FileRegisterBox = new System.Windows.Forms.ComboBox();
            this.ByteRegisterBox = new System.Windows.Forms.ComboBox();
            this.CreateFileButton = new System.Windows.Forms.Button();
            this.DeleteFileButton = new System.Windows.Forms.Button();
            this.CloseFileButton = new System.Windows.Forms.Button();
            this.OpenFileButton = new System.Windows.Forms.Button();
            this.ReadFileButton = new System.Windows.Forms.Button();
            this.DumpClientButton = new System.Windows.Forms.Button();
            this.LaunchClientButton = new System.Windows.Forms.Button();
            this.WriteFileButton = new System.Windows.Forms.Button();
            this.ContinueScriptButton = new System.Windows.Forms.Button();
            this.ClientGroupBox.SuspendLayout();
            this.MetadataServerGroupBox.SuspendLayout();
            this.DataServerGroupBox.SuspendLayout();
            this.ScriptBox.SuspendLayout();
            this.MetadataInfoGroupBox.SuspendLayout();
            this.FileInfoGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // EventBox
            // 
            this.EventBox.Location = new System.Drawing.Point(339, 19);
            this.EventBox.Multiline = true;
            this.EventBox.Name = "EventBox";
            this.EventBox.ReadOnly = true;
            this.EventBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.EventBox.Size = new System.Drawing.Size(184, 107);
            this.EventBox.TabIndex = 2;
            // 
            // LaunchMetadataServerButton
            // 
            this.LaunchMetadataServerButton.Location = new System.Drawing.Point(93, 108);
            this.LaunchMetadataServerButton.Name = "LaunchMetadataServerButton";
            this.LaunchMetadataServerButton.Size = new System.Drawing.Size(67, 30);
            this.LaunchMetadataServerButton.TabIndex = 3;
            this.LaunchMetadataServerButton.Text = "Launch";
            this.LaunchMetadataServerButton.UseVisualStyleBackColor = true;
            this.LaunchMetadataServerButton.Click += new System.EventHandler(this.LaunchMetadataButton_Click);
            // 
            // FailMetadataServerButton
            // 
            this.FailMetadataServerButton.Location = new System.Drawing.Point(14, 66);
            this.FailMetadataServerButton.Name = "FailMetadataServerButton";
            this.FailMetadataServerButton.Size = new System.Drawing.Size(67, 30);
            this.FailMetadataServerButton.TabIndex = 16;
            this.FailMetadataServerButton.Text = "Fail";
            this.FailMetadataServerButton.UseVisualStyleBackColor = true;
            this.FailMetadataServerButton.Click += new System.EventHandler(this.FailMetadataButton_Click);
            // 
            // RecoverMetadataServerButton
            // 
            this.RecoverMetadataServerButton.Location = new System.Drawing.Point(93, 66);
            this.RecoverMetadataServerButton.Name = "RecoverMetadataServerButton";
            this.RecoverMetadataServerButton.Size = new System.Drawing.Size(67, 30);
            this.RecoverMetadataServerButton.TabIndex = 17;
            this.RecoverMetadataServerButton.Text = "Recover ";
            this.RecoverMetadataServerButton.UseVisualStyleBackColor = true;
            this.RecoverMetadataServerButton.Click += new System.EventHandler(this.RecoverMetadataServerButton_Click);
            // 
            // ClientGroupBox
            // 
            this.ClientGroupBox.Controls.Add(this.LaunchClientButton);
            this.ClientGroupBox.Controls.Add(this.WriteFileButton);
            this.ClientGroupBox.Controls.Add(this.CloseFileButton);
            this.ClientGroupBox.Controls.Add(this.DeleteFileButton);
            this.ClientGroupBox.Controls.Add(this.DumpClientButton);
            this.ClientGroupBox.Controls.Add(this.OpenFileButton);
            this.ClientGroupBox.Controls.Add(this.ReadFileButton);
            this.ClientGroupBox.Controls.Add(this.CreateFileButton);
            this.ClientGroupBox.Controls.Add(this.ClientLabel);
            this.ClientGroupBox.Controls.Add(this.ClientBox);
            this.ClientGroupBox.Location = new System.Drawing.Point(12, 12);
            this.ClientGroupBox.Name = "ClientGroupBox";
            this.ClientGroupBox.Size = new System.Drawing.Size(177, 214);
            this.ClientGroupBox.TabIndex = 19;
            this.ClientGroupBox.TabStop = false;
            this.ClientGroupBox.Text = "Client";
            // 
            // ClientLabel
            // 
            this.ClientLabel.AutoSize = true;
            this.ClientLabel.Location = new System.Drawing.Point(22, 30);
            this.ClientLabel.Name = "ClientLabel";
            this.ClientLabel.Size = new System.Drawing.Size(36, 13);
            this.ClientLabel.TabIndex = 63;
            this.ClientLabel.Text = "Client:";
            // 
            // ClientBox
            // 
            this.ClientBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ClientBox.FormattingEnabled = true;
            this.ClientBox.Location = new System.Drawing.Point(79, 27);
            this.ClientBox.Name = "ClientBox";
            this.ClientBox.Size = new System.Drawing.Size(70, 21);
            this.ClientBox.TabIndex = 63;
            // 
            // MetadataServerGroupBox
            // 
            this.MetadataServerGroupBox.Controls.Add(this.DumpMetadataButton);
            this.MetadataServerGroupBox.Controls.Add(this.MetadataBox);
            this.MetadataServerGroupBox.Controls.Add(this.MetadataLabel);
            this.MetadataServerGroupBox.Controls.Add(this.FailMetadataServerButton);
            this.MetadataServerGroupBox.Controls.Add(this.LaunchMetadataServerButton);
            this.MetadataServerGroupBox.Controls.Add(this.RecoverMetadataServerButton);
            this.MetadataServerGroupBox.Location = new System.Drawing.Point(591, 12);
            this.MetadataServerGroupBox.Name = "MetadataServerGroupBox";
            this.MetadataServerGroupBox.Size = new System.Drawing.Size(186, 155);
            this.MetadataServerGroupBox.TabIndex = 20;
            this.MetadataServerGroupBox.TabStop = false;
            this.MetadataServerGroupBox.Text = "Metadata Server";
            // 
            // DumpMetadataButton
            // 
            this.DumpMetadataButton.Location = new System.Drawing.Point(14, 108);
            this.DumpMetadataButton.Name = "DumpMetadataButton";
            this.DumpMetadataButton.Size = new System.Drawing.Size(67, 30);
            this.DumpMetadataButton.TabIndex = 18;
            this.DumpMetadataButton.Text = "Dump";
            this.DumpMetadataButton.UseVisualStyleBackColor = true;
            this.DumpMetadataButton.Click += new System.EventHandler(this.DumpMetadataButton_Click);
            // 
            // MetadataBox
            // 
            this.MetadataBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MetadataBox.FormattingEnabled = true;
            this.MetadataBox.Location = new System.Drawing.Point(89, 29);
            this.MetadataBox.Name = "MetadataBox";
            this.MetadataBox.Size = new System.Drawing.Size(72, 21);
            this.MetadataBox.TabIndex = 1;
            // 
            // MetadataLabel
            // 
            this.MetadataLabel.AutoSize = true;
            this.MetadataLabel.Location = new System.Drawing.Point(14, 32);
            this.MetadataLabel.Name = "MetadataLabel";
            this.MetadataLabel.Size = new System.Drawing.Size(55, 13);
            this.MetadataLabel.TabIndex = 0;
            this.MetadataLabel.Text = "Metadata:";
            // 
            // DataServerGroupBox
            // 
            this.DataServerGroupBox.Controls.Add(this.DataServerLabel);
            this.DataServerGroupBox.Controls.Add(this.DataServerBox);
            this.DataServerGroupBox.Controls.Add(this.DumpDataServerButton);
            this.DataServerGroupBox.Controls.Add(this.LaunchDataServerButton);
            this.DataServerGroupBox.Controls.Add(this.UnfreezeDataServerButton);
            this.DataServerGroupBox.Controls.Add(this.FreezeDataServerButton);
            this.DataServerGroupBox.Controls.Add(this.RecoverDataServerButton);
            this.DataServerGroupBox.Controls.Add(this.FailDataServerButton);
            this.DataServerGroupBox.Location = new System.Drawing.Point(591, 185);
            this.DataServerGroupBox.Name = "DataServerGroupBox";
            this.DataServerGroupBox.Size = new System.Drawing.Size(200, 214);
            this.DataServerGroupBox.TabIndex = 21;
            this.DataServerGroupBox.TabStop = false;
            this.DataServerGroupBox.Text = "Data Server";
            // 
            // DataServerLabel
            // 
            this.DataServerLabel.AutoSize = true;
            this.DataServerLabel.Location = new System.Drawing.Point(19, 37);
            this.DataServerLabel.Name = "DataServerLabel";
            this.DataServerLabel.Size = new System.Drawing.Size(67, 13);
            this.DataServerLabel.TabIndex = 26;
            this.DataServerLabel.Text = "Data Server:";
            // 
            // DataServerBox
            // 
            this.DataServerBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DataServerBox.FormattingEnabled = true;
            this.DataServerBox.Location = new System.Drawing.Point(94, 34);
            this.DataServerBox.Name = "DataServerBox";
            this.DataServerBox.Size = new System.Drawing.Size(70, 21);
            this.DataServerBox.TabIndex = 25;
            // 
            // DumpDataServerButton
            // 
            this.DumpDataServerButton.Location = new System.Drawing.Point(99, 163);
            this.DumpDataServerButton.Name = "DumpDataServerButton";
            this.DumpDataServerButton.Size = new System.Drawing.Size(68, 30);
            this.DumpDataServerButton.TabIndex = 24;
            this.DumpDataServerButton.Text = "Dump ";
            this.DumpDataServerButton.UseVisualStyleBackColor = true;
            this.DumpDataServerButton.Click += new System.EventHandler(this.DumpDataServerButton_Click);
            // 
            // LaunchDataServerButton
            // 
            this.LaunchDataServerButton.Location = new System.Drawing.Point(22, 162);
            this.LaunchDataServerButton.Name = "LaunchDataServerButton";
            this.LaunchDataServerButton.Size = new System.Drawing.Size(69, 31);
            this.LaunchDataServerButton.TabIndex = 23;
            this.LaunchDataServerButton.Text = "Launch ";
            this.LaunchDataServerButton.UseVisualStyleBackColor = true;
            this.LaunchDataServerButton.Click += new System.EventHandler(this.LaunchDataServerButton_Click);
            // 
            // UnfreezeDataServerButton
            // 
            this.UnfreezeDataServerButton.Location = new System.Drawing.Point(99, 120);
            this.UnfreezeDataServerButton.Name = "UnfreezeDataServerButton";
            this.UnfreezeDataServerButton.Size = new System.Drawing.Size(68, 30);
            this.UnfreezeDataServerButton.TabIndex = 21;
            this.UnfreezeDataServerButton.Text = "Unfreeze ";
            this.UnfreezeDataServerButton.UseVisualStyleBackColor = true;
            this.UnfreezeDataServerButton.Click += new System.EventHandler(this.UnfreezeDataServerButton_Click);
            // 
            // FreezeDataServerButton
            // 
            this.FreezeDataServerButton.Location = new System.Drawing.Point(22, 120);
            this.FreezeDataServerButton.Name = "FreezeDataServerButton";
            this.FreezeDataServerButton.Size = new System.Drawing.Size(69, 30);
            this.FreezeDataServerButton.TabIndex = 20;
            this.FreezeDataServerButton.Text = "Freeze ";
            this.FreezeDataServerButton.UseVisualStyleBackColor = true;
            this.FreezeDataServerButton.Click += new System.EventHandler(this.FreezeDataServerButton_Click);
            // 
            // RecoverDataServerButton
            // 
            this.RecoverDataServerButton.Location = new System.Drawing.Point(99, 78);
            this.RecoverDataServerButton.Name = "RecoverDataServerButton";
            this.RecoverDataServerButton.Size = new System.Drawing.Size(68, 30);
            this.RecoverDataServerButton.TabIndex = 19;
            this.RecoverDataServerButton.Text = "Recover ";
            this.RecoverDataServerButton.UseVisualStyleBackColor = true;
            this.RecoverDataServerButton.Click += new System.EventHandler(this.RecoverDataServerButton_Click);
            // 
            // FailDataServerButton
            // 
            this.FailDataServerButton.Location = new System.Drawing.Point(22, 78);
            this.FailDataServerButton.Name = "FailDataServerButton";
            this.FailDataServerButton.Size = new System.Drawing.Size(69, 30);
            this.FailDataServerButton.TabIndex = 18;
            this.FailDataServerButton.Text = "Fail ";
            this.FailDataServerButton.UseVisualStyleBackColor = true;
            this.FailDataServerButton.Click += new System.EventHandler(this.FailDataServerButton_Click);
            // 
            // KillProcessesbutton
            // 
            this.KillProcessesbutton.Location = new System.Drawing.Point(227, 206);
            this.KillProcessesbutton.Name = "KillProcessesbutton";
            this.KillProcessesbutton.Size = new System.Drawing.Size(96, 35);
            this.KillProcessesbutton.TabIndex = 22;
            this.KillProcessesbutton.Text = "Kill Processes";
            this.KillProcessesbutton.UseVisualStyleBackColor = true;
            this.KillProcessesbutton.Click += new System.EventHandler(this.KillProcessesButton_Click);
            // 
            // ScriptBox
            // 
            this.ScriptBox.Controls.Add(this.ContinueScriptButton);
            this.ScriptBox.Controls.Add(this.RunInstructionLineBox);
            this.ScriptBox.Controls.Add(this.GoToLabel);
            this.ScriptBox.Controls.Add(this.NextInstructionLabel);
            this.ScriptBox.Controls.Add(this.CurrentInstructionLabel);
            this.ScriptBox.Controls.Add(this.NextStepButton);
            this.ScriptBox.Controls.Add(this.RunScriptButton);
            this.ScriptBox.Controls.Add(this.ScriptFileBox);
            this.ScriptBox.Controls.Add(this.EventBox);
            this.ScriptBox.Controls.Add(this.ScriptFileLabel);
            this.ScriptBox.Controls.Add(this.LoadScriptButton);
            this.ScriptBox.Location = new System.Drawing.Point(10, 261);
            this.ScriptBox.Name = "ScriptBox";
            this.ScriptBox.Size = new System.Drawing.Size(561, 138);
            this.ScriptBox.TabIndex = 23;
            this.ScriptBox.TabStop = false;
            this.ScriptBox.Text = "Script Options";
            // 
            // RunInstructionLineBox
            // 
            this.RunInstructionLineBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RunInstructionLineBox.FormattingEnabled = true;
            this.RunInstructionLineBox.Location = new System.Drawing.Point(267, 24);
            this.RunInstructionLineBox.Name = "RunInstructionLineBox";
            this.RunInstructionLineBox.Size = new System.Drawing.Size(50, 21);
            this.RunInstructionLineBox.TabIndex = 12;
            // 
            // GoToLabel
            // 
            this.GoToLabel.AutoSize = true;
            this.GoToLabel.Location = new System.Drawing.Point(176, 27);
            this.GoToLabel.Name = "GoToLabel";
            this.GoToLabel.Size = new System.Drawing.Size(80, 13);
            this.GoToLabel.TabIndex = 9;
            this.GoToLabel.Text = "Run Until Line: ";
            // 
            // NextInstructionLabel
            // 
            this.NextInstructionLabel.AutoSize = true;
            this.NextInstructionLabel.Location = new System.Drawing.Point(203, 83);
            this.NextInstructionLabel.Name = "NextInstructionLabel";
            this.NextInstructionLabel.Size = new System.Drawing.Size(52, 13);
            this.NextInstructionLabel.TabIndex = 6;
            this.NextInstructionLabel.Text = "Next Inst.";
            // 
            // CurrentInstructionLabel
            // 
            this.CurrentInstructionLabel.AutoSize = true;
            this.CurrentInstructionLabel.Location = new System.Drawing.Point(192, 55);
            this.CurrentInstructionLabel.Name = "CurrentInstructionLabel";
            this.CurrentInstructionLabel.Size = new System.Drawing.Size(64, 13);
            this.CurrentInstructionLabel.TabIndex = 5;
            this.CurrentInstructionLabel.Text = "Current Inst.";
            // 
            // NextStepButton
            // 
            this.NextStepButton.Location = new System.Drawing.Point(84, 57);
            this.NextStepButton.Name = "NextStepButton";
            this.NextStepButton.Size = new System.Drawing.Size(67, 30);
            this.NextStepButton.TabIndex = 4;
            this.NextStepButton.Text = "Next Step";
            this.NextStepButton.UseVisualStyleBackColor = true;
            this.NextStepButton.Click += new System.EventHandler(this.NextStepButton_Click);
            // 
            // RunScriptButton
            // 
            this.RunScriptButton.Location = new System.Drawing.Point(84, 19);
            this.RunScriptButton.Name = "RunScriptButton";
            this.RunScriptButton.Size = new System.Drawing.Size(67, 29);
            this.RunScriptButton.TabIndex = 3;
            this.RunScriptButton.Text = "Run ";
            this.RunScriptButton.UseVisualStyleBackColor = true;
            this.RunScriptButton.Click += new System.EventHandler(this.RunScriptButton_Click);
            // 
            // ScriptFileBox
            // 
            this.ScriptFileBox.Location = new System.Drawing.Point(71, 102);
            this.ScriptFileBox.Name = "ScriptFileBox";
            this.ScriptFileBox.Size = new System.Drawing.Size(77, 20);
            this.ScriptFileBox.TabIndex = 2;
            // 
            // ScriptFileLabel
            // 
            this.ScriptFileLabel.AutoSize = true;
            this.ScriptFileLabel.Location = new System.Drawing.Point(7, 104);
            this.ScriptFileLabel.Name = "ScriptFileLabel";
            this.ScriptFileLabel.Size = new System.Drawing.Size(53, 13);
            this.ScriptFileLabel.TabIndex = 1;
            this.ScriptFileLabel.Text = "Script File";
            // 
            // LoadScriptButton
            // 
            this.LoadScriptButton.Location = new System.Drawing.Point(6, 19);
            this.LoadScriptButton.Name = "LoadScriptButton";
            this.LoadScriptButton.Size = new System.Drawing.Size(67, 29);
            this.LoadScriptButton.TabIndex = 0;
            this.LoadScriptButton.Text = "Load ";
            this.LoadScriptButton.UseVisualStyleBackColor = true;
            this.LoadScriptButton.Click += new System.EventHandler(this.LoadScriptButton_Click);
            // 
            // SemanticsBox
            // 
            this.SemanticsBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SemanticsBox.FormattingEnabled = true;
            this.SemanticsBox.Items.AddRange(new object[] {
            "default",
            "monotonic"});
            this.SemanticsBox.Location = new System.Drawing.Point(95, 78);
            this.SemanticsBox.Name = "SemanticsBox";
            this.SemanticsBox.Size = new System.Drawing.Size(70, 21);
            this.SemanticsBox.TabIndex = 62;
            // 
            // ByteRegisterLabel
            // 
            this.ByteRegisterLabel.AutoSize = true;
            this.ByteRegisterLabel.Location = new System.Drawing.Point(21, 23);
            this.ByteRegisterLabel.Name = "ByteRegisterLabel";
            this.ByteRegisterLabel.Size = new System.Drawing.Size(54, 13);
            this.ByteRegisterLabel.TabIndex = 56;
            this.ByteRegisterLabel.Text = "Byte Reg.";
            // 
            // FileRegisterLabel
            // 
            this.FileRegisterLabel.AutoSize = true;
            this.FileRegisterLabel.Location = new System.Drawing.Point(26, 52);
            this.FileRegisterLabel.Name = "FileRegisterLabel";
            this.FileRegisterLabel.Size = new System.Drawing.Size(49, 13);
            this.FileRegisterLabel.TabIndex = 55;
            this.FileRegisterLabel.Text = "File Reg.";
            // 
            // SemanticsLabel
            // 
            this.SemanticsLabel.AutoSize = true;
            this.SemanticsLabel.Location = new System.Drawing.Point(21, 81);
            this.SemanticsLabel.Name = "SemanticsLabel";
            this.SemanticsLabel.Size = new System.Drawing.Size(56, 13);
            this.SemanticsLabel.TabIndex = 54;
            this.SemanticsLabel.Text = "Semantics";
            // 
            // FileTextbox
            // 
            this.FileTextbox.Location = new System.Drawing.Point(18, 137);
            this.FileTextbox.Multiline = true;
            this.FileTextbox.Name = "FileTextbox";
            this.FileTextbox.Size = new System.Drawing.Size(138, 57);
            this.FileTextbox.TabIndex = 53;
            // 
            // ReadQuorumLabel
            // 
            this.ReadQuorumLabel.AutoSize = true;
            this.ReadQuorumLabel.Location = new System.Drawing.Point(38, 101);
            this.ReadQuorumLabel.Name = "ReadQuorumLabel";
            this.ReadQuorumLabel.Size = new System.Drawing.Size(23, 13);
            this.ReadQuorumLabel.TabIndex = 49;
            this.ReadQuorumLabel.Text = "RQ";
            // 
            // WriteQuorumLabel
            // 
            this.WriteQuorumLabel.AutoSize = true;
            this.WriteQuorumLabel.Location = new System.Drawing.Point(35, 137);
            this.WriteQuorumLabel.Name = "WriteQuorumLabel";
            this.WriteQuorumLabel.Size = new System.Drawing.Size(26, 13);
            this.WriteQuorumLabel.TabIndex = 48;
            this.WriteQuorumLabel.Text = "WQ";
            // 
            // NServersLabel
            // 
            this.NServersLabel.AutoSize = true;
            this.NServersLabel.Location = new System.Drawing.Point(10, 67);
            this.NServersLabel.Name = "NServersLabel";
            this.NServersLabel.Size = new System.Drawing.Size(51, 13);
            this.NServersLabel.TabIndex = 47;
            this.NServersLabel.Text = "NServers";
            // 
            // FilenameBox
            // 
            this.FilenameBox.Location = new System.Drawing.Point(67, 29);
            this.FilenameBox.Name = "FilenameBox";
            this.FilenameBox.Size = new System.Drawing.Size(74, 20);
            this.FilenameBox.TabIndex = 40;
            // 
            // FileLabel
            // 
            this.FileLabel.AutoSize = true;
            this.FileLabel.Location = new System.Drawing.Point(38, 32);
            this.FileLabel.Name = "FileLabel";
            this.FileLabel.Size = new System.Drawing.Size(23, 13);
            this.FileLabel.TabIndex = 41;
            this.FileLabel.Text = "File";
            // 
            // MetadataInfoGroupBox
            // 
            this.MetadataInfoGroupBox.Controls.Add(this.WriteQuorumBox);
            this.MetadataInfoGroupBox.Controls.Add(this.ReadQuorumBox);
            this.MetadataInfoGroupBox.Controls.Add(this.NServersBox);
            this.MetadataInfoGroupBox.Controls.Add(this.FilenameBox);
            this.MetadataInfoGroupBox.Controls.Add(this.FileLabel);
            this.MetadataInfoGroupBox.Controls.Add(this.NServersLabel);
            this.MetadataInfoGroupBox.Controls.Add(this.WriteQuorumLabel);
            this.MetadataInfoGroupBox.Controls.Add(this.ReadQuorumLabel);
            this.MetadataInfoGroupBox.Location = new System.Drawing.Point(205, 12);
            this.MetadataInfoGroupBox.Name = "MetadataInfoGroupBox";
            this.MetadataInfoGroupBox.Size = new System.Drawing.Size(155, 174);
            this.MetadataInfoGroupBox.TabIndex = 63;
            this.MetadataInfoGroupBox.TabStop = false;
            this.MetadataInfoGroupBox.Text = "MetadataInfo";
            // 
            // WriteQuorumBox
            // 
            this.WriteQuorumBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.WriteQuorumBox.FormattingEnabled = true;
            this.WriteQuorumBox.Location = new System.Drawing.Point(67, 134);
            this.WriteQuorumBox.Name = "WriteQuorumBox";
            this.WriteQuorumBox.Size = new System.Drawing.Size(50, 21);
            this.WriteQuorumBox.TabIndex = 52;
            // 
            // ReadQuorumBox
            // 
            this.ReadQuorumBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ReadQuorumBox.FormattingEnabled = true;
            this.ReadQuorumBox.Location = new System.Drawing.Point(67, 101);
            this.ReadQuorumBox.Name = "ReadQuorumBox";
            this.ReadQuorumBox.Size = new System.Drawing.Size(50, 21);
            this.ReadQuorumBox.TabIndex = 51;
            // 
            // NServersBox
            // 
            this.NServersBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.NServersBox.FormattingEnabled = true;
            this.NServersBox.Location = new System.Drawing.Point(67, 66);
            this.NServersBox.Name = "NServersBox";
            this.NServersBox.Size = new System.Drawing.Size(50, 21);
            this.NServersBox.TabIndex = 50;
            // 
            // FileInfoGroupBox
            // 
            this.FileInfoGroupBox.Controls.Add(this.WriteModeBox);
            this.FileInfoGroupBox.Controls.Add(this.WriteModeLabel);
            this.FileInfoGroupBox.Controls.Add(this.FileRegisterBox);
            this.FileInfoGroupBox.Controls.Add(this.FileTextbox);
            this.FileInfoGroupBox.Controls.Add(this.ByteRegisterBox);
            this.FileInfoGroupBox.Controls.Add(this.SemanticsBox);
            this.FileInfoGroupBox.Controls.Add(this.SemanticsLabel);
            this.FileInfoGroupBox.Controls.Add(this.ByteRegisterLabel);
            this.FileInfoGroupBox.Controls.Add(this.FileRegisterLabel);
            this.FileInfoGroupBox.Location = new System.Drawing.Point(377, 12);
            this.FileInfoGroupBox.Name = "FileInfoGroupBox";
            this.FileInfoGroupBox.Size = new System.Drawing.Size(179, 214);
            this.FileInfoGroupBox.TabIndex = 64;
            this.FileInfoGroupBox.TabStop = false;
            this.FileInfoGroupBox.Text = "File Info";
            // 
            // WriteModeBox
            // 
            this.WriteModeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.WriteModeBox.FormattingEnabled = true;
            this.WriteModeBox.Items.AddRange(new object[] {
            "Text",
            "Byte Reg."});
            this.WriteModeBox.Location = new System.Drawing.Point(95, 108);
            this.WriteModeBox.Name = "WriteModeBox";
            this.WriteModeBox.Size = new System.Drawing.Size(70, 21);
            this.WriteModeBox.TabIndex = 68;
            // 
            // WriteModeLabel
            // 
            this.WriteModeLabel.AutoSize = true;
            this.WriteModeLabel.Location = new System.Drawing.Point(15, 111);
            this.WriteModeLabel.Name = "WriteModeLabel";
            this.WriteModeLabel.Size = new System.Drawing.Size(62, 13);
            this.WriteModeLabel.TabIndex = 67;
            this.WriteModeLabel.Text = "Write Mode";
            // 
            // FileRegisterBox
            // 
            this.FileRegisterBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FileRegisterBox.FormattingEnabled = true;
            this.FileRegisterBox.Location = new System.Drawing.Point(95, 49);
            this.FileRegisterBox.Name = "FileRegisterBox";
            this.FileRegisterBox.Size = new System.Drawing.Size(43, 21);
            this.FileRegisterBox.TabIndex = 66;
            // 
            // ByteRegisterBox
            // 
            this.ByteRegisterBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ByteRegisterBox.FormattingEnabled = true;
            this.ByteRegisterBox.Location = new System.Drawing.Point(95, 20);
            this.ByteRegisterBox.Name = "ByteRegisterBox";
            this.ByteRegisterBox.Size = new System.Drawing.Size(43, 21);
            this.ByteRegisterBox.TabIndex = 65;
            // 
            // CreateFileButton
            // 
            this.CreateFileButton.Location = new System.Drawing.Point(17, 66);
            this.CreateFileButton.Name = "CreateFileButton";
            this.CreateFileButton.Size = new System.Drawing.Size(67, 30);
            this.CreateFileButton.TabIndex = 65;
            this.CreateFileButton.Text = "Create";
            this.CreateFileButton.UseVisualStyleBackColor = true;
            this.CreateFileButton.Click += new System.EventHandler(this.CreateFileButton_Click);
            // 
            // DeleteFileButton
            // 
            this.DeleteFileButton.Location = new System.Drawing.Point(92, 66);
            this.DeleteFileButton.Name = "DeleteFileButton";
            this.DeleteFileButton.Size = new System.Drawing.Size(67, 30);
            this.DeleteFileButton.TabIndex = 65;
            this.DeleteFileButton.Text = "Delete";
            this.DeleteFileButton.UseVisualStyleBackColor = true;
            this.DeleteFileButton.Click += new System.EventHandler(this.DeleteFileButton_Click);
            // 
            // CloseFileButton
            // 
            this.CloseFileButton.Location = new System.Drawing.Point(92, 104);
            this.CloseFileButton.Name = "CloseFileButton";
            this.CloseFileButton.Size = new System.Drawing.Size(67, 30);
            this.CloseFileButton.TabIndex = 13;
            this.CloseFileButton.Text = "Close";
            this.CloseFileButton.UseVisualStyleBackColor = true;
            this.CloseFileButton.Click += new System.EventHandler(this.CloseFileButton_Click);
            // 
            // OpenFileButton
            // 
            this.OpenFileButton.Location = new System.Drawing.Point(17, 104);
            this.OpenFileButton.Name = "OpenFileButton";
            this.OpenFileButton.Size = new System.Drawing.Size(67, 30);
            this.OpenFileButton.TabIndex = 66;
            this.OpenFileButton.Text = "Open";
            this.OpenFileButton.UseVisualStyleBackColor = true;
            this.OpenFileButton.Click += new System.EventHandler(this.OpenFileButton_Click);
            // 
            // ReadFileButton
            // 
            this.ReadFileButton.Location = new System.Drawing.Point(17, 144);
            this.ReadFileButton.Name = "ReadFileButton";
            this.ReadFileButton.Size = new System.Drawing.Size(67, 30);
            this.ReadFileButton.TabIndex = 65;
            this.ReadFileButton.Text = "Read";
            this.ReadFileButton.UseVisualStyleBackColor = true;
            this.ReadFileButton.Click += new System.EventHandler(this.ReadFileButton_Click);
            // 
            // DumpClientButton
            // 
            this.DumpClientButton.Location = new System.Drawing.Point(17, 178);
            this.DumpClientButton.Name = "DumpClientButton";
            this.DumpClientButton.Size = new System.Drawing.Size(67, 30);
            this.DumpClientButton.TabIndex = 66;
            this.DumpClientButton.Text = "Dump";
            this.DumpClientButton.UseVisualStyleBackColor = true;
            this.DumpClientButton.Click += new System.EventHandler(this.DumpClientButton_Click);
            // 
            // LaunchClientButton
            // 
            this.LaunchClientButton.Location = new System.Drawing.Point(91, 178);
            this.LaunchClientButton.Name = "LaunchClientButton";
            this.LaunchClientButton.Size = new System.Drawing.Size(68, 30);
            this.LaunchClientButton.TabIndex = 67;
            this.LaunchClientButton.Text = "Launch";
            this.LaunchClientButton.UseVisualStyleBackColor = true;
            this.LaunchClientButton.Click += new System.EventHandler(this.LaunchClientButton_Click);
            // 
            // WriteFileButton
            // 
            this.WriteFileButton.Location = new System.Drawing.Point(92, 142);
            this.WriteFileButton.Name = "WriteFileButton";
            this.WriteFileButton.Size = new System.Drawing.Size(67, 30);
            this.WriteFileButton.TabIndex = 68;
            this.WriteFileButton.Text = "Write";
            this.WriteFileButton.UseVisualStyleBackColor = true;
            this.WriteFileButton.Click += new System.EventHandler(this.WriteFileButton_Click);
            // 
            // ContinueScriptButton
            // 
            this.ContinueScriptButton.Location = new System.Drawing.Point(6, 57);
            this.ContinueScriptButton.Name = "ContinueScriptButton";
            this.ContinueScriptButton.Size = new System.Drawing.Size(67, 30);
            this.ContinueScriptButton.TabIndex = 13;
            this.ContinueScriptButton.Text = "Continue";
            this.ContinueScriptButton.UseVisualStyleBackColor = true;
            this.ContinueScriptButton.Click += new System.EventHandler(this.ContinueScriptButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 415);
            this.Controls.Add(this.FileInfoGroupBox);
            this.Controls.Add(this.MetadataInfoGroupBox);
            this.Controls.Add(this.ScriptBox);
            this.Controls.Add(this.KillProcessesbutton);
            this.Controls.Add(this.DataServerGroupBox);
            this.Controls.Add(this.MetadataServerGroupBox);
            this.Controls.Add(this.ClientGroupBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ClientGroupBox.ResumeLayout(false);
            this.ClientGroupBox.PerformLayout();
            this.MetadataServerGroupBox.ResumeLayout(false);
            this.MetadataServerGroupBox.PerformLayout();
            this.DataServerGroupBox.ResumeLayout(false);
            this.DataServerGroupBox.PerformLayout();
            this.ScriptBox.ResumeLayout(false);
            this.ScriptBox.PerformLayout();
            this.MetadataInfoGroupBox.ResumeLayout(false);
            this.MetadataInfoGroupBox.PerformLayout();
            this.FileInfoGroupBox.ResumeLayout(false);
            this.FileInfoGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox EventBox;
        private System.Windows.Forms.Button LaunchMetadataServerButton;
        private System.Windows.Forms.Button FailMetadataServerButton;
        private System.Windows.Forms.Button RecoverMetadataServerButton;
        private System.Windows.Forms.GroupBox ClientGroupBox;
        private System.Windows.Forms.GroupBox MetadataServerGroupBox;
        private System.Windows.Forms.GroupBox DataServerGroupBox;
        private System.Windows.Forms.Button DumpMetadataButton;
        private System.Windows.Forms.Button UnfreezeDataServerButton;
        private System.Windows.Forms.Button FreezeDataServerButton;
        private System.Windows.Forms.Button RecoverDataServerButton;
        private System.Windows.Forms.Button FailDataServerButton;
        private System.Windows.Forms.Button LaunchDataServerButton;
        private System.Windows.Forms.Button KillProcessesbutton;
        private System.Windows.Forms.GroupBox ScriptBox;
        private System.Windows.Forms.Button NextStepButton;
        private System.Windows.Forms.Button RunScriptButton;
        private System.Windows.Forms.TextBox ScriptFileBox;
        private System.Windows.Forms.Label ScriptFileLabel;
        private System.Windows.Forms.Button LoadScriptButton;
        private System.Windows.Forms.Button DumpDataServerButton;
        private System.Windows.Forms.Label GoToLabel;
        private System.Windows.Forms.Label NextInstructionLabel;
        private System.Windows.Forms.Label CurrentInstructionLabel;
        private System.Windows.Forms.Label ClientLabel;
        private System.Windows.Forms.ComboBox ClientBox;
        private System.Windows.Forms.ComboBox SemanticsBox;
        private System.Windows.Forms.Label ByteRegisterLabel;
        private System.Windows.Forms.Label FileRegisterLabel;
        private System.Windows.Forms.Label SemanticsLabel;
        private System.Windows.Forms.TextBox FileTextbox;
        private System.Windows.Forms.Label ReadQuorumLabel;
        private System.Windows.Forms.Label WriteQuorumLabel;
        private System.Windows.Forms.Label NServersLabel;
        private System.Windows.Forms.TextBox FilenameBox;
        private System.Windows.Forms.Label FileLabel;
        private System.Windows.Forms.GroupBox MetadataInfoGroupBox;
        private System.Windows.Forms.ComboBox WriteQuorumBox;
        private System.Windows.Forms.ComboBox ReadQuorumBox;
        private System.Windows.Forms.ComboBox NServersBox;
        private System.Windows.Forms.GroupBox FileInfoGroupBox;
        private System.Windows.Forms.ComboBox FileRegisterBox;
        private System.Windows.Forms.ComboBox ByteRegisterBox;
        private System.Windows.Forms.ComboBox MetadataBox;
        private System.Windows.Forms.Label MetadataLabel;
        private System.Windows.Forms.Label DataServerLabel;
        private System.Windows.Forms.ComboBox DataServerBox;
        private System.Windows.Forms.ComboBox RunInstructionLineBox;
        private System.Windows.Forms.ComboBox WriteModeBox;
        private System.Windows.Forms.Label WriteModeLabel;
        private System.Windows.Forms.Button LaunchClientButton;
        private System.Windows.Forms.Button WriteFileButton;
        private System.Windows.Forms.Button CloseFileButton;
        private System.Windows.Forms.Button DeleteFileButton;
        private System.Windows.Forms.Button DumpClientButton;
        private System.Windows.Forms.Button OpenFileButton;
        private System.Windows.Forms.Button ReadFileButton;
        private System.Windows.Forms.Button CreateFileButton;
        private System.Windows.Forms.Button ContinueScriptButton;
    }
}

