using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace PuppetMaster
{
    public partial class Form1 : Form
    {
        private PuppetMaster puppetMaster;
        private string currentFile;
        int visibleItems; 

        /*
         * Class constructor. Passes a puppetMaster as an argument, which is the one used
         * to execute the user input/commands received in the form.
         */
        public Form1(PuppetMaster puppetMaster)
        {
            this.puppetMaster = puppetMaster;
            InitializeComponent();
            visibleItems = MessageBox.ClientSize.Height / MessageBox.ItemHeight;
        }

        /*********************************
         ********* PUPPET MASTER *********
         *********************************/

        private void LoadScriptButton_Click(object sender, EventArgs e)
        {
            puppetMaster.loadScript(currentFile);
        }

        private void RunScriptButton_Click(object sender, EventArgs e)
        {
            CurrentInstructionBox.Text = puppetMaster.runScript(RunInstructionLineBox.SelectedIndex).ToString();
        }

        private void NextStepButton_Click(object sender, EventArgs e)
        {
            CurrentInstructionBox.Text = puppetMaster.nextStep().ToString();
        }

        /***********************
         ********* GUI *********
         ***********************/

        public void updateMessageBox(string msg)
        {
            MessageBox.Items.Add(msg);
            MessageBox.TopIndex = Math.Max(MessageBox.Items.Count - visibleItems + 1, 0);
        }

        public void updateScriptText(string msg, int numInst)
        {
            int i;
            EventBox.Clear();
            EventBox.Text += msg;
            RunInstructionLineBox.Items.Clear();

            for (i = 0; i < numInst; i++)
                RunInstructionLineBox.Items.Add(i);

            RunInstructionLineBox.SelectedIndex = i-1;
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                currentFile = openFileDialog.FileName;
                string[] paths = currentFile.Split('\\');
                ScriptLabel.Text = "File: " + paths[paths.Length-1];
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            foreach (Process proc in Process.GetProcessesByName("DataServer"))
                proc.Kill();

            foreach (Process proc in Process.GetProcessesByName("Client"))
                proc.Kill();

            foreach (Process proc in Process.GetProcessesByName("MetadataServer"))
                proc.Kill();
        }

        private void CommandBoxKeyPressed(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                try
                {
                    puppetMaster.interpretInstruction(CommandBox.Text);
                    updateMessageBox(CommandBox.Text);
                }
                catch (Exception)
                {
                    updateMessageBox(CommandBox.Text + " is not a valid instruction.");
                }
                CommandBox.Text = string.Empty;
            }
        }

        public void showDumpMessage(string dump)
        {
            System.Windows.Forms.MessageBox.Show("-----DUMPING MESSAGE-----\r\n" + dump);
        }
    }
}
