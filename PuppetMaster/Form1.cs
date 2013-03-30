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
        private int clientCounter = 0;
        private int dataServerCounter = 0;
        private int clientPort = 8000;
        private int dataServerPort = 9000;

        public Form1(PuppetMaster puppetMaster)
        {
            this.puppetMaster = puppetMaster;
            InitializeComponent();
        }

        private void LaunchClient_Click(object sender, EventArgs e)
        {
            puppetMaster.LaunchClient(clientPort.ToString());
            clientPort++;
        }

        private void LaunchMetadataButton_Click(object sender, EventArgs e)
        {
            puppetMaster.LaunchMetadata(selectedMetadata);
        }

        private void LaunchDataServerButton_Click(object sender, EventArgs e)
        {
            puppetMaster.LaunchDataServer(dataServerPort.ToString());
            dataServerPort++;
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

        private void DumpMetadataButton_Click(object sender, EventArgs e)
        {
            puppetMaster.dumpMetadataServer(selectedMetadata);
        }

        private void FailMetadataButton_Click(object sender, EventArgs e)
        {

        }

        private void RecoverMetadataServerButton_Click(object sender, EventArgs e)
        {

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

        private void WriteFileButton_Click(object sender, EventArgs e)
        {
            if (FilenameBox.Text.Equals(""))
                ClientEventBox.Text += "Please, specify a filename.\r\n";
            else
                puppetMaster.writeFile(FilenameBox.Text, FileTextbox.Text, ClientListBox.SelectedIndex);
        }

        private void ReadFileButton_Click(object sender, EventArgs e)
        {
            if (FilenameBox.Text.Equals("") || SemanticsTextBox.Text.Equals(""))
                ClientEventBox.Text += "Please, fill all the fields necessary to read a file\r\n";
            else
                puppetMaster.readFile(FilenameBox.Text, Convert.ToInt32(SemanticsTextBox.Text), ClientListBox.SelectedIndex);
        }

        private void KillProcessesButton_Click(object sender, EventArgs e)
        {
            clientCounter = 0;
            dataServerCounter = 0;
            clientPort = 8000;
            dataServerPort = 9000;
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
    }
}
