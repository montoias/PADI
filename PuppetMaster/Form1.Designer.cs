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
            this.ClientPortBox = new System.Windows.Forms.TextBox();
            this.ClientPortLabel = new System.Windows.Forms.Label();
            this.MetadataOption1 = new System.Windows.Forms.RadioButton();
            this.MetadataOption2 = new System.Windows.Forms.RadioButton();
            this.MetadataOption3 = new System.Windows.Forms.RadioButton();
            this.FailMetadataServerButton = new System.Windows.Forms.Button();
            this.RecoverMetadataServerButton = new System.Windows.Forms.Button();
            this.ClientListBox = new System.Windows.Forms.ListBox();
            this.ClientGroupBox = new System.Windows.Forms.GroupBox();
            this.WriteFileButton = new System.Windows.Forms.Button();
            this.ReadFileButton = new System.Windows.Forms.Button();
            this.MetadataServerGroupBox = new System.Windows.Forms.GroupBox();
            this.DataServerGroupBox = new System.Windows.Forms.GroupBox();
            this.FreezeDataServerButton = new System.Windows.Forms.Button();
            this.DataServerListBox = new System.Windows.Forms.ListBox();
            this.FailDataServerButton = new System.Windows.Forms.Button();
            this.RegisterDataServer = new System.Windows.Forms.Button();
            this.UnfreezeDataServerButton = new System.Windows.Forms.Button();
            this.RecoverDataServerButton = new System.Windows.Forms.Button();
            this.LaunchDataServerButton = new System.Windows.Forms.Button();
            this.DataServerPortBox = new System.Windows.Forms.TextBox();
            this.DataServerPortLabel = new System.Windows.Forms.Label();
            this.MetadataEventBox = new System.Windows.Forms.TextBox();
            this.DumpMetadataButton = new System.Windows.Forms.Button();
            this.ClientGroupBox.SuspendLayout();
            this.MetadataServerGroupBox.SuspendLayout();
            this.DataServerGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // LaunchClientServerButton
            // 
            this.LaunchClientServerButton.Location = new System.Drawing.Point(182, 295);
            this.LaunchClientServerButton.Name = "LaunchClientServerButton";
            this.LaunchClientServerButton.Size = new System.Drawing.Size(93, 28);
            this.LaunchClientServerButton.TabIndex = 0;
            this.LaunchClientServerButton.Text = "Launch Client";
            this.LaunchClientServerButton.UseVisualStyleBackColor = true;
            this.LaunchClientServerButton.Click += new System.EventHandler(this.LaunchClient_Click);
            // 
            // OpenFileButton
            // 
            this.OpenFileButton.Location = new System.Drawing.Point(202, 82);
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
            this.ClientEventBox.Size = new System.Drawing.Size(369, 168);
            this.ClientEventBox.TabIndex = 2;
            // 
            // LaunchMetadataServerButton
            // 
            this.LaunchMetadataServerButton.Location = new System.Drawing.Point(222, 275);
            this.LaunchMetadataServerButton.Name = "LaunchMetadataServerButton";
            this.LaunchMetadataServerButton.Size = new System.Drawing.Size(106, 30);
            this.LaunchMetadataServerButton.TabIndex = 3;
            this.LaunchMetadataServerButton.Text = "Launch Metadata ";
            this.LaunchMetadataServerButton.UseVisualStyleBackColor = true;
            this.LaunchMetadataServerButton.Click += new System.EventHandler(this.LaunchMetadataButton_Click);
            // 
            // CreateFileButton
            // 
            this.CreateFileButton.Location = new System.Drawing.Point(111, 82);
            this.CreateFileButton.Name = "CreateFileButton";
            this.CreateFileButton.Size = new System.Drawing.Size(85, 30);
            this.CreateFileButton.TabIndex = 4;
            this.CreateFileButton.Text = "Create File";
            this.CreateFileButton.UseVisualStyleBackColor = true;
            this.CreateFileButton.Click += new System.EventHandler(this.CreateFileButton_Click);
            // 
            // FilenameBox
            // 
            this.FilenameBox.Location = new System.Drawing.Point(163, 40);
            this.FilenameBox.Multiline = true;
            this.FilenameBox.Name = "FilenameBox";
            this.FilenameBox.Size = new System.Drawing.Size(124, 26);
            this.FilenameBox.TabIndex = 5;
            // 
            // FilenameLabel
            // 
            this.FilenameLabel.AutoSize = true;
            this.FilenameLabel.Location = new System.Drawing.Point(108, 43);
            this.FilenameLabel.Name = "FilenameLabel";
            this.FilenameLabel.Size = new System.Drawing.Size(49, 13);
            this.FilenameLabel.TabIndex = 6;
            this.FilenameLabel.Text = "Filename";
            // 
            // DeleteFileButton
            // 
            this.DeleteFileButton.Location = new System.Drawing.Point(111, 118);
            this.DeleteFileButton.Name = "DeleteFileButton";
            this.DeleteFileButton.Size = new System.Drawing.Size(85, 30);
            this.DeleteFileButton.TabIndex = 7;
            this.DeleteFileButton.Text = "Delete File";
            this.DeleteFileButton.UseVisualStyleBackColor = true;
            this.DeleteFileButton.Click += new System.EventHandler(this.DeleteFileButton_Click);
            // 
            // CloseFileButton
            // 
            this.CloseFileButton.Location = new System.Drawing.Point(202, 118);
            this.CloseFileButton.Name = "CloseFileButton";
            this.CloseFileButton.Size = new System.Drawing.Size(85, 30);
            this.CloseFileButton.TabIndex = 8;
            this.CloseFileButton.Text = "Close File";
            this.CloseFileButton.UseVisualStyleBackColor = true;
            this.CloseFileButton.Click += new System.EventHandler(this.CloseFileButton_Click);
            // 
            // ClientPortBox
            // 
            this.ClientPortBox.Location = new System.Drawing.Point(58, 300);
            this.ClientPortBox.Name = "ClientPortBox";
            this.ClientPortBox.Size = new System.Drawing.Size(78, 20);
            this.ClientPortBox.TabIndex = 9;
            // 
            // ClientPortLabel
            // 
            this.ClientPortLabel.AutoSize = true;
            this.ClientPortLabel.Location = new System.Drawing.Point(15, 303);
            this.ClientPortLabel.Name = "ClientPortLabel";
            this.ClientPortLabel.Size = new System.Drawing.Size(26, 13);
            this.ClientPortLabel.TabIndex = 11;
            this.ClientPortLabel.Text = "Port";
            // 
            // MetadataOption1
            // 
            this.MetadataOption1.AutoSize = true;
            this.MetadataOption1.Checked = true;
            this.MetadataOption1.Location = new System.Drawing.Point(215, 38);
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
            this.MetadataOption2.Location = new System.Drawing.Point(215, 72);
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
            this.MetadataOption3.Location = new System.Drawing.Point(215, 107);
            this.MetadataOption3.Name = "MetadataOption3";
            this.MetadataOption3.Size = new System.Drawing.Size(113, 17);
            this.MetadataOption3.TabIndex = 15;
            this.MetadataOption3.Text = "Metadata Server 2";
            this.MetadataOption3.UseVisualStyleBackColor = true;
            this.MetadataOption3.CheckedChanged += new System.EventHandler(this.MetadataOption3_CheckedChanged);
            // 
            // FailMetadataServerButton
            // 
            this.FailMetadataServerButton.Location = new System.Drawing.Point(20, 43);
            this.FailMetadataServerButton.Name = "FailMetadataServerButton";
            this.FailMetadataServerButton.Size = new System.Drawing.Size(98, 30);
            this.FailMetadataServerButton.TabIndex = 16;
            this.FailMetadataServerButton.Text = "Fail Server";
            this.FailMetadataServerButton.UseVisualStyleBackColor = true;
            this.FailMetadataServerButton.Click += new System.EventHandler(this.FailMetadataButton_Click);
            // 
            // RecoverMetadataServerButton
            // 
            this.RecoverMetadataServerButton.Location = new System.Drawing.Point(20, 82);
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
            this.ClientListBox.Location = new System.Drawing.Point(22, 31);
            this.ClientListBox.Name = "ClientListBox";
            this.ClientListBox.Size = new System.Drawing.Size(81, 134);
            this.ClientListBox.TabIndex = 18;
            // 
            // ClientGroupBox
            // 
            this.ClientGroupBox.Controls.Add(this.WriteFileButton);
            this.ClientGroupBox.Controls.Add(this.ClientListBox);
            this.ClientGroupBox.Controls.Add(this.ReadFileButton);
            this.ClientGroupBox.Controls.Add(this.FilenameBox);
            this.ClientGroupBox.Controls.Add(this.FilenameLabel);
            this.ClientGroupBox.Controls.Add(this.OpenFileButton);
            this.ClientGroupBox.Controls.Add(this.CreateFileButton);
            this.ClientGroupBox.Controls.Add(this.CloseFileButton);
            this.ClientGroupBox.Controls.Add(this.ClientPortLabel);
            this.ClientGroupBox.Controls.Add(this.DeleteFileButton);
            this.ClientGroupBox.Controls.Add(this.LaunchClientServerButton);
            this.ClientGroupBox.Controls.Add(this.ClientPortBox);
            this.ClientGroupBox.Location = new System.Drawing.Point(12, 12);
            this.ClientGroupBox.Name = "ClientGroupBox";
            this.ClientGroupBox.Size = new System.Drawing.Size(307, 333);
            this.ClientGroupBox.TabIndex = 19;
            this.ClientGroupBox.TabStop = false;
            this.ClientGroupBox.Text = "Client Server";
            // 
            // WriteFileButton
            // 
            this.WriteFileButton.Location = new System.Drawing.Point(207, 165);
            this.WriteFileButton.Name = "WriteFileButton";
            this.WriteFileButton.Size = new System.Drawing.Size(80, 30);
            this.WriteFileButton.TabIndex = 19;
            this.WriteFileButton.Text = "Write File";
            this.WriteFileButton.UseVisualStyleBackColor = true;
            // 
            // ReadFileButton
            // 
            this.ReadFileButton.Location = new System.Drawing.Point(111, 165);
            this.ReadFileButton.Name = "ReadFileButton";
            this.ReadFileButton.Size = new System.Drawing.Size(80, 30);
            this.ReadFileButton.TabIndex = 19;
            this.ReadFileButton.Text = "Read File";
            this.ReadFileButton.UseVisualStyleBackColor = true;
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
            this.MetadataServerGroupBox.Location = new System.Drawing.Point(356, 12);
            this.MetadataServerGroupBox.Name = "MetadataServerGroupBox";
            this.MetadataServerGroupBox.Size = new System.Drawing.Size(356, 333);
            this.MetadataServerGroupBox.TabIndex = 20;
            this.MetadataServerGroupBox.TabStop = false;
            this.MetadataServerGroupBox.Text = "Metadata Server";
            // 
            // DataServerGroupBox
            // 
            this.DataServerGroupBox.Controls.Add(this.FreezeDataServerButton);
            this.DataServerGroupBox.Controls.Add(this.DataServerListBox);
            this.DataServerGroupBox.Controls.Add(this.FailDataServerButton);
            this.DataServerGroupBox.Controls.Add(this.RegisterDataServer);
            this.DataServerGroupBox.Controls.Add(this.UnfreezeDataServerButton);
            this.DataServerGroupBox.Controls.Add(this.RecoverDataServerButton);
            this.DataServerGroupBox.Controls.Add(this.LaunchDataServerButton);
            this.DataServerGroupBox.Controls.Add(this.DataServerPortBox);
            this.DataServerGroupBox.Controls.Add(this.DataServerPortLabel);
            this.DataServerGroupBox.Location = new System.Drawing.Point(741, 12);
            this.DataServerGroupBox.Name = "DataServerGroupBox";
            this.DataServerGroupBox.Size = new System.Drawing.Size(360, 332);
            this.DataServerGroupBox.TabIndex = 21;
            this.DataServerGroupBox.TabStop = false;
            this.DataServerGroupBox.Text = "Data Server";
            // 
            // FreezeDataServerButton
            // 
            this.FreezeDataServerButton.Location = new System.Drawing.Point(30, 126);
            this.FreezeDataServerButton.Name = "FreezeDataServerButton";
            this.FreezeDataServerButton.Size = new System.Drawing.Size(98, 30);
            this.FreezeDataServerButton.TabIndex = 16;
            this.FreezeDataServerButton.Text = "Freeze Server";
            this.FreezeDataServerButton.UseVisualStyleBackColor = true;
            this.FreezeDataServerButton.Click += new System.EventHandler(this.FailMetadataButton_Click);
            // 
            // DataServerListBox
            // 
            this.DataServerListBox.FormattingEnabled = true;
            this.DataServerListBox.Location = new System.Drawing.Point(215, 38);
            this.DataServerListBox.Name = "DataServerListBox";
            this.DataServerListBox.Size = new System.Drawing.Size(81, 134);
            this.DataServerListBox.TabIndex = 18;
            // 
            // FailDataServerButton
            // 
            this.FailDataServerButton.Location = new System.Drawing.Point(30, 43);
            this.FailDataServerButton.Name = "FailDataServerButton";
            this.FailDataServerButton.Size = new System.Drawing.Size(98, 30);
            this.FailDataServerButton.TabIndex = 16;
            this.FailDataServerButton.Text = "Fail Server";
            this.FailDataServerButton.UseVisualStyleBackColor = true;
            this.FailDataServerButton.Click += new System.EventHandler(this.FailMetadataButton_Click);
            // 
            // RegisterDataServer
            // 
            this.RegisterDataServer.Location = new System.Drawing.Point(203, 213);
            this.RegisterDataServer.Name = "RegisterDataServer";
            this.RegisterDataServer.Size = new System.Drawing.Size(98, 30);
            this.RegisterDataServer.TabIndex = 17;
            this.RegisterDataServer.Text = "Register Server";
            this.RegisterDataServer.UseVisualStyleBackColor = true;
            this.RegisterDataServer.Click += new System.EventHandler(this.RecoverMetadataServerButton_Click);
            // 
            // UnfreezeDataServerButton
            // 
            this.UnfreezeDataServerButton.Location = new System.Drawing.Point(30, 165);
            this.UnfreezeDataServerButton.Name = "UnfreezeDataServerButton";
            this.UnfreezeDataServerButton.Size = new System.Drawing.Size(98, 30);
            this.UnfreezeDataServerButton.TabIndex = 17;
            this.UnfreezeDataServerButton.Text = "Unfreeze Server";
            this.UnfreezeDataServerButton.UseVisualStyleBackColor = true;
            this.UnfreezeDataServerButton.Click += new System.EventHandler(this.RecoverMetadataServerButton_Click);
            // 
            // RecoverDataServerButton
            // 
            this.RecoverDataServerButton.Location = new System.Drawing.Point(30, 82);
            this.RecoverDataServerButton.Name = "RecoverDataServerButton";
            this.RecoverDataServerButton.Size = new System.Drawing.Size(98, 30);
            this.RecoverDataServerButton.TabIndex = 17;
            this.RecoverDataServerButton.Text = "Recover Server";
            this.RecoverDataServerButton.UseVisualStyleBackColor = true;
            this.RecoverDataServerButton.Click += new System.EventHandler(this.RecoverMetadataServerButton_Click);
            // 
            // LaunchDataServerButton
            // 
            this.LaunchDataServerButton.Location = new System.Drawing.Point(203, 288);
            this.LaunchDataServerButton.Name = "LaunchDataServerButton";
            this.LaunchDataServerButton.Size = new System.Drawing.Size(115, 28);
            this.LaunchDataServerButton.TabIndex = 0;
            this.LaunchDataServerButton.Text = "Launch Data Server";
            this.LaunchDataServerButton.UseVisualStyleBackColor = true;
            this.LaunchDataServerButton.Click += new System.EventHandler(this.LaunchClient_Click);
            // 
            // DataServerPortBox
            // 
            this.DataServerPortBox.Location = new System.Drawing.Point(79, 293);
            this.DataServerPortBox.Name = "DataServerPortBox";
            this.DataServerPortBox.Size = new System.Drawing.Size(78, 20);
            this.DataServerPortBox.TabIndex = 9;
            // 
            // DataServerPortLabel
            // 
            this.DataServerPortLabel.AutoSize = true;
            this.DataServerPortLabel.Location = new System.Drawing.Point(36, 296);
            this.DataServerPortLabel.Name = "DataServerPortLabel";
            this.DataServerPortLabel.Size = new System.Drawing.Size(26, 13);
            this.DataServerPortLabel.TabIndex = 11;
            this.DataServerPortLabel.Text = "Port";
            // 
            // MetadataEventBox
            // 
            this.MetadataEventBox.Location = new System.Drawing.Point(460, 391);
            this.MetadataEventBox.Multiline = true;
            this.MetadataEventBox.Name = "MetadataEventBox";
            this.MetadataEventBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.MetadataEventBox.Size = new System.Drawing.Size(369, 168);
            this.MetadataEventBox.TabIndex = 2;
            // 
            // DumpMetadataButton
            // 
            this.DumpMetadataButton.Location = new System.Drawing.Point(104, 275);
            this.DumpMetadataButton.Name = "DumpMetadataButton";
            this.DumpMetadataButton.Size = new System.Drawing.Size(106, 30);
            this.DumpMetadataButton.TabIndex = 18;
            this.DumpMetadataButton.Text = "Dump Metadata";
            this.DumpMetadataButton.UseVisualStyleBackColor = true;
            this.DumpMetadataButton.Click += new System.EventHandler(this.DumpMetadataButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1211, 581);
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
            this.DataServerGroupBox.PerformLayout();
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
        private System.Windows.Forms.TextBox ClientPortBox;
        private System.Windows.Forms.Label ClientPortLabel;
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
        private System.Windows.Forms.Button FreezeDataServerButton;
        private System.Windows.Forms.ListBox DataServerListBox;
        private System.Windows.Forms.Button FailDataServerButton;
        private System.Windows.Forms.Button UnfreezeDataServerButton;
        private System.Windows.Forms.Button RecoverDataServerButton;
        private System.Windows.Forms.Button LaunchDataServerButton;
        private System.Windows.Forms.TextBox DataServerPortBox;
        private System.Windows.Forms.Label DataServerPortLabel;
        private System.Windows.Forms.Button RegisterDataServer;
        private System.Windows.Forms.TextBox MetadataEventBox;
        private System.Windows.Forms.Button DumpMetadataButton;
    }
}

