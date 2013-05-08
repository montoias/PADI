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
            this.ScriptBox = new System.Windows.Forms.GroupBox();
            this.ScriptLabel = new System.Windows.Forms.Label();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.CurrentInstructionBox = new System.Windows.Forms.TextBox();
            this.RunInstructionLineBox = new System.Windows.Forms.ComboBox();
            this.GoToLabel = new System.Windows.Forms.Label();
            this.CurrentInstructionLabel = new System.Windows.Forms.Label();
            this.NextStepButton = new System.Windows.Forms.Button();
            this.RunScriptButton = new System.Windows.Forms.Button();
            this.LoadScriptButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.CommandBox = new System.Windows.Forms.TextBox();
            this.MessageBox = new System.Windows.Forms.ListBox();
            this.ScriptBox.SuspendLayout();
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
            // ScriptBox
            // 
            this.ScriptBox.Controls.Add(this.ScriptLabel);
            this.ScriptBox.Controls.Add(this.BrowseButton);
            this.ScriptBox.Controls.Add(this.CurrentInstructionBox);
            this.ScriptBox.Controls.Add(this.RunInstructionLineBox);
            this.ScriptBox.Controls.Add(this.GoToLabel);
            this.ScriptBox.Controls.Add(this.CurrentInstructionLabel);
            this.ScriptBox.Controls.Add(this.NextStepButton);
            this.ScriptBox.Controls.Add(this.RunScriptButton);
            this.ScriptBox.Controls.Add(this.EventBox);
            this.ScriptBox.Controls.Add(this.LoadScriptButton);
            this.ScriptBox.Location = new System.Drawing.Point(12, 12);
            this.ScriptBox.Name = "ScriptBox";
            this.ScriptBox.Size = new System.Drawing.Size(561, 136);
            this.ScriptBox.TabIndex = 23;
            this.ScriptBox.TabStop = false;
            this.ScriptBox.Text = "Script Options";
            // 
            // ScriptLabel
            // 
            this.ScriptLabel.AutoSize = true;
            this.ScriptLabel.Location = new System.Drawing.Point(10, 104);
            this.ScriptLabel.Name = "ScriptLabel";
            this.ScriptLabel.Size = new System.Drawing.Size(68, 13);
            this.ScriptLabel.TabIndex = 67;
            this.ScriptLabel.Text = "Choose a file";
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(86, 57);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(65, 29);
            this.BrowseButton.TabIndex = 66;
            this.BrowseButton.Text = "Browse";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // CurrentInstructionBox
            // 
            this.CurrentInstructionBox.Location = new System.Drawing.Point(267, 57);
            this.CurrentInstructionBox.Name = "CurrentInstructionBox";
            this.CurrentInstructionBox.ReadOnly = true;
            this.CurrentInstructionBox.Size = new System.Drawing.Size(31, 20);
            this.CurrentInstructionBox.TabIndex = 14;
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
            // CurrentInstructionLabel
            // 
            this.CurrentInstructionLabel.AutoSize = true;
            this.CurrentInstructionLabel.Location = new System.Drawing.Point(192, 61);
            this.CurrentInstructionLabel.Name = "CurrentInstructionLabel";
            this.CurrentInstructionLabel.Size = new System.Drawing.Size(64, 13);
            this.CurrentInstructionLabel.TabIndex = 5;
            this.CurrentInstructionLabel.Text = "Current Inst.";
            // 
            // NextStepButton
            // 
            this.NextStepButton.Location = new System.Drawing.Point(84, 19);
            this.NextStepButton.Name = "NextStepButton";
            this.NextStepButton.Size = new System.Drawing.Size(67, 29);
            this.NextStepButton.TabIndex = 4;
            this.NextStepButton.Text = "Next Step";
            this.NextStepButton.UseVisualStyleBackColor = true;
            this.NextStepButton.Click += new System.EventHandler(this.NextStepButton_Click);
            // 
            // RunScriptButton
            // 
            this.RunScriptButton.Location = new System.Drawing.Point(11, 19);
            this.RunScriptButton.Name = "RunScriptButton";
            this.RunScriptButton.Size = new System.Drawing.Size(67, 29);
            this.RunScriptButton.TabIndex = 3;
            this.RunScriptButton.Text = "Run ";
            this.RunScriptButton.UseVisualStyleBackColor = true;
            this.RunScriptButton.Click += new System.EventHandler(this.RunScriptButton_Click);
            // 
            // LoadScriptButton
            // 
            this.LoadScriptButton.Location = new System.Drawing.Point(11, 57);
            this.LoadScriptButton.Name = "LoadScriptButton";
            this.LoadScriptButton.Size = new System.Drawing.Size(67, 29);
            this.LoadScriptButton.TabIndex = 0;
            this.LoadScriptButton.Text = "Load ";
            this.LoadScriptButton.UseVisualStyleBackColor = true;
            this.LoadScriptButton.Click += new System.EventHandler(this.LoadScriptButton_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // CommandBox
            // 
            this.CommandBox.Location = new System.Drawing.Point(12, 243);
            this.CommandBox.Multiline = true;
            this.CommandBox.Name = "CommandBox";
            this.CommandBox.Size = new System.Drawing.Size(561, 28);
            this.CommandBox.TabIndex = 24;
            this.CommandBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CommandBoxKeyPressed);
            // 
            // MessageBox
            // 
            this.MessageBox.FormattingEnabled = true;
            this.MessageBox.Location = new System.Drawing.Point(12, 161);
            this.MessageBox.Name = "MessageBox";
            this.MessageBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.MessageBox.Size = new System.Drawing.Size(561, 82);
            this.MessageBox.TabIndex = 25;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 281);
            this.Controls.Add(this.MessageBox);
            this.Controls.Add(this.CommandBox);
            this.Controls.Add(this.ScriptBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ScriptBox.ResumeLayout(false);
            this.ScriptBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox EventBox;
        private System.Windows.Forms.GroupBox ScriptBox;
        private System.Windows.Forms.Button NextStepButton;
        private System.Windows.Forms.Button RunScriptButton;
        private System.Windows.Forms.Button LoadScriptButton;
        private System.Windows.Forms.Label GoToLabel;
        private System.Windows.Forms.Label CurrentInstructionLabel;
        private System.Windows.Forms.ComboBox RunInstructionLineBox;
        private System.Windows.Forms.TextBox CurrentInstructionBox;
        private System.Windows.Forms.Label ScriptLabel;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TextBox CommandBox;
        private System.Windows.Forms.ListBox MessageBox;
    }
}

