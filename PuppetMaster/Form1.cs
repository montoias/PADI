using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Collections;

using CommonTypes;

namespace PuppetMaster
{

    public partial class Form1 : Form
    {
        private PuppetMaster puppetMaster;
        private int selectedMetadata = 0;
        private int writeMode = 0;
        private int clientCounter = 0;
        private int dataServerCounter = 0;

        /*
         * Class constructor. Passes a puppetMaster as an argument, which is the one used
         * to execute the user input/commands received in the form.
         */
        public Form1(PuppetMaster puppetMaster)
        {
            this.puppetMaster = puppetMaster;
            InitializeComponent();
        }


        /*********************************
         ********* PUPPET MASTER *********
         *********************************/

        private void LaunchClient_Click(object sender, EventArgs e)
        {
            puppetMaster.launchClient();
        }

        private void LaunchMetadataButton_Click(object sender, EventArgs e)
        {
            puppetMaster.launchMetadata(selectedMetadata);
        }

        private void LaunchDataServerButton_Click(object sender, EventArgs e)
        {
            puppetMaster.launchDataServer();
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            if (FilenameBox.Text.Equals(""))
                ClientEventBox.Text += "Please, specify a filename.\r\n";
            else
                puppetMaster.openFile(FilenameBox.Text, ClientListBox.SelectedIndex);
        }
        
        private void CloseFileButton_Click(object sender, EventArgs e)
        {
            if (FilenameBox.Text.Equals(""))
                ClientEventBox.Text += "Please, specify a filename.\r\n";
            else
                puppetMaster.closeFile(FilenameBox.Text, ClientListBox.SelectedIndex);
        }

        private void CreateFileButton_Click(object sender, EventArgs e)
        {
            if (FilenameBox.Text.Equals("") || NServersTextBox.Text.Equals("") || ReadQuorumTextBox.Text.Equals("") ||
                WriteQuorumTextBox.Text.Equals(""))
            {
                ClientEventBox.Text += "Please, fill up all the fields necessary to create a file.\r\n";
            }
            else
            {
                puppetMaster.createFile(FilenameBox.Text, Convert.ToInt32(NServersTextBox.Text),
                    Convert.ToInt32(ReadQuorumTextBox.Text), Convert.ToInt32(WriteQuorumTextBox.Text), ClientListBox.SelectedIndex);
            }
        }

        private void DeleteFileButton_Click(object sender, EventArgs e)
        {
            if (FilenameBox.Text.Equals(""))
                ClientEventBox.Text += "Please, specify a filename.\r\n";
            else
                puppetMaster.deleteFile(FilenameBox.Text, ClientListBox.SelectedIndex);
        }

        private void WriteFileButton_Click(object sender, EventArgs e)
        {
            if (writeMode == 0)
                puppetMaster.writeFile(Convert.ToInt32(FileRegisterTextBox.Text), FileTextbox.Text, ClientListBox.SelectedIndex);
            else
                puppetMaster.writeFile(Convert.ToInt32(FileRegisterTextBox.Text), Convert.ToInt32(ByteRegisterTextBox.Text), ClientListBox.SelectedIndex);
        }

        private void ReadFileButton_Click(object sender, EventArgs e)
        {
            puppetMaster.readFile(Convert.ToInt32(FileRegisterTextBox.Text), Convert.ToInt32(SemanticsTextBox.Text), ClientListBox.SelectedIndex);
        }

        private void FailMetadataButton_Click(object sender, EventArgs e)
        {
            puppetMaster.failMetadata(selectedMetadata);
        }

        private void RecoverMetadataServerButton_Click(object sender, EventArgs e)
        {
            puppetMaster.recoverMetadata(selectedMetadata);
        }

        private void DumpMetadataButton_Click(object sender, EventArgs e)
        {
            puppetMaster.dumpMetadataServer(selectedMetadata);
        }

        private void KillProcessesButton_Click(object sender, EventArgs e)
        {
            clientCounter = 0;
            dataServerCounter = 0;
            ClientListBox.Items.Clear();
            DataServerListBox.Items.Clear();
            puppetMaster.killProcesses();
        }

        private void FailDataServerButton_Click(object sender, EventArgs e)
        {
            puppetMaster.failDataServer(DataServerListBox.SelectedIndex);
        }

        private void RecoverDataServerButton_Click(object sender, EventArgs e)
        {
            puppetMaster.recoverDataServer(DataServerListBox.SelectedIndex);
        }

        private void LoadScriptButton_Click(object sender, EventArgs e)
        {
            puppetMaster.loadScript(ScriptFileBox.Text);
        }

        private void RunScriptButton_Click(object sender, EventArgs e)
        {
            puppetMaster.runScript();
        }

        private void NextStepButton_Click(object sender, EventArgs e)
        {
            puppetMaster.nextStep();
        }        

        /***********************
         ********* GUI *********
         ***********************/

        private void UseTextFileOption_CheckedChanged(object sender, EventArgs e)
        {
            writeMode = 0;
        }

        private void ByteRegisterOption_CheckedChanged(object sender, EventArgs e)
        {
            writeMode = 1;
        }

        private void MetadataOption1_CheckedChanged(object sender, EventArgs e)
        {
            selectedMetadata = 0;
        }

        private void MetadataOption2_CheckedChanged(object sender, EventArgs e)
        {
            selectedMetadata = 1;
        }

        private void MetadataOption3_CheckedChanged(object sender, EventArgs e)
        {
            selectedMetadata = 2;
        }

        public void showMetadata(MetadataInfo newMetadata)
        {
            ClientEventBox.Text += "-------------------\r\nMETADATA\r\n";
            ClientEventBox.Text += "Filename:" + newMetadata.filename + "\r\n";
            ClientEventBox.Text += "NumDataServers:" + newMetadata.numDataServers + "\r\n";
            ClientEventBox.Text += "ReadQuorum:" + newMetadata.readQuorum + "\r\n";
            ClientEventBox.Text += "WriteQuorum:" + newMetadata.writeQuorum + "\r\n";
            ClientEventBox.Text += "DataServers:" + newMetadata.dataServersToString() + "\r\n";
        }

        private void checkNumeric(object sender, KeyPressEventArgs e)
        {
            const char Delete = (char)8;
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete;
        }

        public void addClient()
        {
            ClientListBox.Items.Add("c-" + clientCounter);
            ClientListBox.SetSelected(clientCounter++, true);
        }

        public void addDataServer()
        {
            DataServerListBox.Items.Add("d-" + dataServerCounter);
            DataServerListBox.SetSelected(dataServerCounter++, true);
        }

        public void updateClientBox(string msg)
        {
            ClientEventBox.Text += msg;
        }

        public void updateMetadataBox(string msg)
        {
            MetadataEventBox.Text += msg;
        }

        public void updateFileText(string msg, int version)
        {
            FileTextbox.Clear();
            FileTextbox.Text += msg;
        }
    }
}
