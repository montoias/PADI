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

        public Form1(PuppetMaster puppetMaster)
        {
            this.puppetMaster = puppetMaster;
            InitializeComponent();
        }

        private void LaunchClient_Click(object sender, EventArgs e)
        {
            puppetMaster.LaunchClient(ClientPortBox.Text);
        }

        private void LaunchMetadataButton_Click(object sender, EventArgs e)
        {
            puppetMaster.LaunchMetadata(selectedMetadata);
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            puppetMaster.OpenFile(FilenameBox.Text, ClientListBox.SelectedIndex);
        }


        private void CloseFileButton_Click(object sender, EventArgs e)
        {
            puppetMaster.CloseFile(FilenameBox.Text, ClientListBox.SelectedIndex);
        }

        private void CreateFileButton_Click(object sender, EventArgs e)
        {
            puppetMaster.createFile(FilenameBox.Text, ClientListBox.SelectedIndex);
        }

        private void DeleteFileButton_Click(object sender, EventArgs e)
        {
            puppetMaster.deleteFile(FilenameBox.Text, ClientListBox.SelectedIndex);
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
            ClientListBox.Items.Add("Client " + clientCounter++);
        }

        public void updateMessage(string msg)
        {
            EventBox.Text += msg;
        }

        public void showMetadata(MetadataInfo newMetadata)
        {
            EventBox.Text += "-------------------\r\nMETADATA\r\n";
            EventBox.Text += "Filename:" + newMetadata.filename + "\r\n";
            EventBox.Text += "NumDataServers:" + newMetadata.numDataServers + "\r\n";
            EventBox.Text += "ReadQuorum:" + newMetadata.readQuorum + "\r\n";
            EventBox.Text += "WriteQuorum:" + newMetadata.writeQuorum + "\r\n";
            EventBox.Text += "DataServers:" + newMetadata.dataServers + "\r\n";
        }
    }
}
