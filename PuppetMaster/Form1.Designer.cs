﻿namespace PuppetMaster
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
            this.LaunchClientButton = new System.Windows.Forms.Button();
            this.OpenFileButton = new System.Windows.Forms.Button();
            this.EventBox = new System.Windows.Forms.TextBox();
            this.LaunchMetadataButton = new System.Windows.Forms.Button();
            this.CreateFileButton = new System.Windows.Forms.Button();
            this.FilenameBox = new System.Windows.Forms.TextBox();
            this.FilenameLabel = new System.Windows.Forms.Label();
            this.DeleteFileButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LaunchClientButton
            // 
            this.LaunchClientButton.Location = new System.Drawing.Point(179, 404);
            this.LaunchClientButton.Name = "LaunchClientButton";
            this.LaunchClientButton.Size = new System.Drawing.Size(93, 28);
            this.LaunchClientButton.TabIndex = 0;
            this.LaunchClientButton.Text = "Launch Client";
            this.LaunchClientButton.UseVisualStyleBackColor = true;
            this.LaunchClientButton.Click += new System.EventHandler(this.LaunchClient_Click);
            // 
            // OpenFileButton
            // 
            this.OpenFileButton.Location = new System.Drawing.Point(575, 247);
            this.OpenFileButton.Name = "OpenFileButton";
            this.OpenFileButton.Size = new System.Drawing.Size(85, 30);
            this.OpenFileButton.TabIndex = 1;
            this.OpenFileButton.Text = "Open File";
            this.OpenFileButton.UseVisualStyleBackColor = true;
            this.OpenFileButton.Click += new System.EventHandler(this.OpenFile_Click);
            // 
            // EventBox
            // 
            this.EventBox.Location = new System.Drawing.Point(746, 40);
            this.EventBox.Multiline = true;
            this.EventBox.Name = "EventBox";
            this.EventBox.Size = new System.Drawing.Size(113, 168);
            this.EventBox.TabIndex = 2;
            // 
            // LaunchMetadataButton
            // 
            this.LaunchMetadataButton.Location = new System.Drawing.Point(48, 404);
            this.LaunchMetadataButton.Name = "LaunchMetadataButton";
            this.LaunchMetadataButton.Size = new System.Drawing.Size(103, 28);
            this.LaunchMetadataButton.TabIndex = 3;
            this.LaunchMetadataButton.Text = "Launch Metadata";
            this.LaunchMetadataButton.UseVisualStyleBackColor = true;
            this.LaunchMetadataButton.Click += new System.EventHandler(this.LaunchMetadataButton_Click);
            // 
            // CreateFileButton
            // 
            this.CreateFileButton.Location = new System.Drawing.Point(695, 247);
            this.CreateFileButton.Name = "CreateFileButton";
            this.CreateFileButton.Size = new System.Drawing.Size(85, 30);
            this.CreateFileButton.TabIndex = 4;
            this.CreateFileButton.Text = "Create File";
            this.CreateFileButton.UseVisualStyleBackColor = true;
            this.CreateFileButton.Click += new System.EventHandler(this.CreateFileButton_Click);
            // 
            // FilenameBox
            // 
            this.FilenameBox.Location = new System.Drawing.Point(497, 74);
            this.FilenameBox.Multiline = true;
            this.FilenameBox.Name = "FilenameBox";
            this.FilenameBox.Size = new System.Drawing.Size(124, 26);
            this.FilenameBox.TabIndex = 5;
            // 
            // FilenameLabel
            // 
            this.FilenameLabel.AutoSize = true;
            this.FilenameLabel.Location = new System.Drawing.Point(442, 77);
            this.FilenameLabel.Name = "FilenameLabel";
            this.FilenameLabel.Size = new System.Drawing.Size(49, 13);
            this.FilenameLabel.TabIndex = 6;
            this.FilenameLabel.Text = "Filename";
            // 
            // DeleteFileButton
            // 
            this.DeleteFileButton.Location = new System.Drawing.Point(575, 292);
            this.DeleteFileButton.Name = "DeleteFileButton";
            this.DeleteFileButton.Size = new System.Drawing.Size(85, 30);
            this.DeleteFileButton.TabIndex = 7;
            this.DeleteFileButton.Text = "Delete File";
            this.DeleteFileButton.UseVisualStyleBackColor = true;
            this.DeleteFileButton.Click += new System.EventHandler(this.DeleteFileButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(909, 472);
            this.Controls.Add(this.DeleteFileButton);
            this.Controls.Add(this.FilenameLabel);
            this.Controls.Add(this.FilenameBox);
            this.Controls.Add(this.CreateFileButton);
            this.Controls.Add(this.LaunchMetadataButton);
            this.Controls.Add(this.EventBox);
            this.Controls.Add(this.OpenFileButton);
            this.Controls.Add(this.LaunchClientButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LaunchClientButton;
        private System.Windows.Forms.Button OpenFileButton;
        private System.Windows.Forms.TextBox EventBox;
        private System.Windows.Forms.Button LaunchMetadataButton;
        private System.Windows.Forms.Button CreateFileButton;
        private System.Windows.Forms.TextBox FilenameBox;
        private System.Windows.Forms.Label FilenameLabel;
        private System.Windows.Forms.Button DeleteFileButton;
    }
}

