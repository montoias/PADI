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
            this.EventBox = new System.Windows.Forms.TextBox();
            this.LaunchMetadataServerButton = new System.Windows.Forms.Button();
            this.CreateFileButton = new System.Windows.Forms.Button();
            this.FilenameBox = new System.Windows.Forms.TextBox();
            this.FilenameLabel = new System.Windows.Forms.Label();
            this.DeleteFileButton = new System.Windows.Forms.Button();
            this.CloseFileButton = new System.Windows.Forms.Button();
            this.PortBox = new System.Windows.Forms.TextBox();
            this.PortLabel = new System.Windows.Forms.Label();
            this.MetadataOption1 = new System.Windows.Forms.RadioButton();
            this.MetadataOption2 = new System.Windows.Forms.RadioButton();
            this.MetadataOption3 = new System.Windows.Forms.RadioButton();
            this.FailMetadataButton = new System.Windows.Forms.Button();
            this.RecoverMetadataServerButton = new System.Windows.Forms.Button();
            this.ClientListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // LaunchClientServerButton
            // 
            this.LaunchClientServerButton.Location = new System.Drawing.Point(25, 347);
            this.LaunchClientServerButton.Name = "LaunchClientServerButton";
            this.LaunchClientServerButton.Size = new System.Drawing.Size(93, 28);
            this.LaunchClientServerButton.TabIndex = 0;
            this.LaunchClientServerButton.Text = "Launch Client";
            this.LaunchClientServerButton.UseVisualStyleBackColor = true;
            this.LaunchClientServerButton.Click += new System.EventHandler(this.LaunchClient_Click);
            // 
            // OpenFileButton
            // 
            this.OpenFileButton.Location = new System.Drawing.Point(21, 62);
            this.OpenFileButton.Name = "OpenFileButton";
            this.OpenFileButton.Size = new System.Drawing.Size(85, 30);
            this.OpenFileButton.TabIndex = 1;
            this.OpenFileButton.Text = "Open File";
            this.OpenFileButton.UseVisualStyleBackColor = true;
            this.OpenFileButton.Click += new System.EventHandler(this.OpenFile_Click);
            // 
            // EventBox
            // 
            this.EventBox.Location = new System.Drawing.Point(270, 12);
            this.EventBox.Multiline = true;
            this.EventBox.Name = "EventBox";
            this.EventBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.EventBox.Size = new System.Drawing.Size(369, 168);
            this.EventBox.TabIndex = 2;
            // 
            // LaunchMetadataServerButton
            // 
            this.LaunchMetadataServerButton.Location = new System.Drawing.Point(414, 220);
            this.LaunchMetadataServerButton.Name = "LaunchMetadataServerButton";
            this.LaunchMetadataServerButton.Size = new System.Drawing.Size(105, 40);
            this.LaunchMetadataServerButton.TabIndex = 3;
            this.LaunchMetadataServerButton.Text = "Launch Metadata Server";
            this.LaunchMetadataServerButton.UseVisualStyleBackColor = true;
            this.LaunchMetadataServerButton.Click += new System.EventHandler(this.LaunchMetadataButton_Click);
            // 
            // CreateFileButton
            // 
            this.CreateFileButton.Location = new System.Drawing.Point(129, 62);
            this.CreateFileButton.Name = "CreateFileButton";
            this.CreateFileButton.Size = new System.Drawing.Size(85, 30);
            this.CreateFileButton.TabIndex = 4;
            this.CreateFileButton.Text = "Create File";
            this.CreateFileButton.UseVisualStyleBackColor = true;
            this.CreateFileButton.Click += new System.EventHandler(this.CreateFileButton_Click);
            // 
            // FilenameBox
            // 
            this.FilenameBox.Location = new System.Drawing.Point(73, 12);
            this.FilenameBox.Multiline = true;
            this.FilenameBox.Name = "FilenameBox";
            this.FilenameBox.Size = new System.Drawing.Size(124, 26);
            this.FilenameBox.TabIndex = 5;
            // 
            // FilenameLabel
            // 
            this.FilenameLabel.AutoSize = true;
            this.FilenameLabel.Location = new System.Drawing.Point(18, 15);
            this.FilenameLabel.Name = "FilenameLabel";
            this.FilenameLabel.Size = new System.Drawing.Size(49, 13);
            this.FilenameLabel.TabIndex = 6;
            this.FilenameLabel.Text = "Filename";
            // 
            // DeleteFileButton
            // 
            this.DeleteFileButton.Location = new System.Drawing.Point(129, 112);
            this.DeleteFileButton.Name = "DeleteFileButton";
            this.DeleteFileButton.Size = new System.Drawing.Size(85, 30);
            this.DeleteFileButton.TabIndex = 7;
            this.DeleteFileButton.Text = "Delete File";
            this.DeleteFileButton.UseVisualStyleBackColor = true;
            this.DeleteFileButton.Click += new System.EventHandler(this.DeleteFileButton_Click);
            // 
            // CloseFileButton
            // 
            this.CloseFileButton.Location = new System.Drawing.Point(25, 116);
            this.CloseFileButton.Name = "CloseFileButton";
            this.CloseFileButton.Size = new System.Drawing.Size(81, 26);
            this.CloseFileButton.TabIndex = 8;
            this.CloseFileButton.Text = "Close File";
            this.CloseFileButton.UseVisualStyleBackColor = true;
            this.CloseFileButton.Click += new System.EventHandler(this.CloseFileButton_Click);
            // 
            // PortBox
            // 
            this.PortBox.Location = new System.Drawing.Point(207, 347);
            this.PortBox.Name = "PortBox";
            this.PortBox.Size = new System.Drawing.Size(133, 20);
            this.PortBox.TabIndex = 9;
            // 
            // PortLabel
            // 
            this.PortLabel.AutoSize = true;
            this.PortLabel.Location = new System.Drawing.Point(164, 350);
            this.PortLabel.Name = "PortLabel";
            this.PortLabel.Size = new System.Drawing.Size(26, 13);
            this.PortLabel.TabIndex = 11;
            this.PortLabel.Text = "Port";
            // 
            // MetadataOption1
            // 
            this.MetadataOption1.AutoSize = true;
            this.MetadataOption1.Checked = true;
            this.MetadataOption1.Location = new System.Drawing.Point(543, 268);
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
            this.MetadataOption2.Location = new System.Drawing.Point(543, 302);
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
            this.MetadataOption3.Location = new System.Drawing.Point(543, 337);
            this.MetadataOption3.Name = "MetadataOption3";
            this.MetadataOption3.Size = new System.Drawing.Size(113, 17);
            this.MetadataOption3.TabIndex = 15;
            this.MetadataOption3.Text = "Metadata Server 2";
            this.MetadataOption3.UseVisualStyleBackColor = true;
            this.MetadataOption3.CheckedChanged += new System.EventHandler(this.MetadataOption3_CheckedChanged);
            // 
            // FailMetadataButton
            // 
            this.FailMetadataButton.Location = new System.Drawing.Point(414, 280);
            this.FailMetadataButton.Name = "FailMetadataButton";
            this.FailMetadataButton.Size = new System.Drawing.Size(105, 39);
            this.FailMetadataButton.TabIndex = 16;
            this.FailMetadataButton.Text = "Metadata Server Fail";
            this.FailMetadataButton.UseVisualStyleBackColor = true;
            this.FailMetadataButton.Click += new System.EventHandler(this.FailMetadataButton_Click);
            // 
            // RecoverMetadataServerButton
            // 
            this.RecoverMetadataServerButton.Location = new System.Drawing.Point(417, 337);
            this.RecoverMetadataServerButton.Name = "RecoverMetadataServerButton";
            this.RecoverMetadataServerButton.Size = new System.Drawing.Size(101, 38);
            this.RecoverMetadataServerButton.TabIndex = 17;
            this.RecoverMetadataServerButton.Text = "Metadata Server Recover";
            this.RecoverMetadataServerButton.UseVisualStyleBackColor = true;
            this.RecoverMetadataServerButton.Click += new System.EventHandler(this.RecoverMetadataServerButton_Click);
            // 
            // ClientListBox
            // 
            this.ClientListBox.FormattingEnabled = true;
            this.ClientListBox.Location = new System.Drawing.Point(25, 163);
            this.ClientListBox.Name = "ClientListBox";
            this.ClientListBox.Size = new System.Drawing.Size(81, 134);
            this.ClientListBox.TabIndex = 18;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 401);
            this.Controls.Add(this.ClientListBox);
            this.Controls.Add(this.RecoverMetadataServerButton);
            this.Controls.Add(this.FailMetadataButton);
            this.Controls.Add(this.MetadataOption3);
            this.Controls.Add(this.MetadataOption2);
            this.Controls.Add(this.MetadataOption1);
            this.Controls.Add(this.PortLabel);
            this.Controls.Add(this.PortBox);
            this.Controls.Add(this.CloseFileButton);
            this.Controls.Add(this.DeleteFileButton);
            this.Controls.Add(this.FilenameLabel);
            this.Controls.Add(this.FilenameBox);
            this.Controls.Add(this.CreateFileButton);
            this.Controls.Add(this.LaunchMetadataServerButton);
            this.Controls.Add(this.EventBox);
            this.Controls.Add(this.OpenFileButton);
            this.Controls.Add(this.LaunchClientServerButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LaunchClientServerButton;
        private System.Windows.Forms.Button OpenFileButton;
        private System.Windows.Forms.TextBox EventBox;
        private System.Windows.Forms.Button LaunchMetadataServerButton;
        private System.Windows.Forms.Button CreateFileButton;
        private System.Windows.Forms.TextBox FilenameBox;
        private System.Windows.Forms.Label FilenameLabel;
        private System.Windows.Forms.Button DeleteFileButton;
        private System.Windows.Forms.Button CloseFileButton;
        private System.Windows.Forms.TextBox PortBox;
        private System.Windows.Forms.Label PortLabel;
        private System.Windows.Forms.RadioButton MetadataOption1;
        private System.Windows.Forms.RadioButton MetadataOption2;
        private System.Windows.Forms.RadioButton MetadataOption3;
        private System.Windows.Forms.Button FailMetadataButton;
        private System.Windows.Forms.Button RecoverMetadataServerButton;
        private System.Windows.Forms.ListBox ClientListBox;
    }
}

