using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonTypes;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Collections;

namespace PuppetMaster
{

    //TODO : read from config file
    public partial class Form1 : Form
    {
        private List<IPuppetClientServer> clientsList = new List<IPuppetClientServer>();

        private List<Dictionary<string, MetadataInfo>> metadataInfoList = new List<Dictionary<string, MetadataInfo>>(); 

        private string projectFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        
        private IPuppetMetadataServer[] metadataList = new IPuppetMetadataServer[3];
        private int selectedMetadata = 0;

        //TODO: replace with full remote location
        private string[] metadataLocation = {"8081", "8082", "8083" };
        private int clientCounter = 0;

        private List<string> clientsPorts = new List<string>();
        private BitArray metadataLaunched = new BitArray(3);

        public Form1()
        {
            TcpChannel channel = (TcpChannel)Helper.GetChannel(8080, true);
            ChannelServices.RegisterChannel(channel, true);

            InitializeComponent();
        }

        private void LaunchClient_Click(object sender, EventArgs e)
        {
            string port = PortBox.Text;

            if (!clientsPorts.Contains(port))
            {
                string args = port + " " + metadataLocation[0] + " " + metadataLocation[1] + " " + metadataLocation[2];

                //TODO: Implement remote starting -> use WMI
                Process.Start(Path.Combine(projectFolder, "ClientServer\\bin\\Debug\\ClientServer.exe"), args);

                IPuppetClientServer newClient = (IPuppetClientServer)Activator.GetObject(
                   typeof(IPuppetClientServer),
                   "tcp://localhost:" + port + "/ClientServer");

                clientsList.Add(newClient);
                ClientListBox.Items.Add("Client " + clientCounter++);
                clientsPorts.Add(port);
                metadataInfoList.Add(new Dictionary<string, MetadataInfo>());
            }
            else
            {
                EventBox.Text += "Client already exists at port " + port + "!\r\n";
            }
            
        }

        private void LaunchMetadataButton_Click(object sender, EventArgs e)
        {
            string location = metadataLocation[selectedMetadata];

            if (!metadataLaunched[selectedMetadata])
            {
                //TODO: Implement remote starting -> use WMI
                Process.Start(Path.Combine(projectFolder, "MetadataServer\\bin\\Debug\\MetadataServer.exe"), location);
                IPuppetMetadataServer newMetadata = (IPuppetMetadataServer)Activator.GetObject(
                   typeof(IPuppetMetadataServer),
                   "tcp://localhost:" + location + "/ClientServer");
                metadataList[selectedMetadata] = newMetadata;
                metadataLaunched[selectedMetadata] = true;
            }
            else
            {
                EventBox.Text += "Metadata Server " + selectedMetadata + " already launched!\r\n";
            }
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            int selectedClient = ClientListBox.SelectedIndex;
            string filename = FilenameBox.Text;

            try {
                MetadataInfo newMetadata = clientsList[selectedClient].popen(filename);
                metadataInfoList[selectedClient].Add(filename, newMetadata);
                showMetadata(newMetadata);
            }
            catch (Exception exc) //TODO: FileAlreadyOpenException
            {
                EventBox.Text += "File " + filename + " already opened!\r\n";
            }
        }


        private void CloseFileButton_Click(object sender, EventArgs e)
        {
            int selectedClient = ClientListBox.SelectedIndex;
            string filename = FilenameBox.Text;

            try
            {
                clientsList[selectedClient].pclose(filename);
                metadataInfoList[selectedClient].Remove(filename);
            }
            catch (Exception exc) //TODO: FileNotOpenException
            {
                EventBox.Text += "File " + filename + " was not opened!\r\n";
            }
        }

        private void CreateFileButton_Click(object sender, EventArgs e)
        {
            int selectedClient = ClientListBox.SelectedIndex;
            string filename = FilenameBox.Text;

            //TODO: Interface details
            try
            {
                MetadataInfo newMetadata = clientsList[selectedClient].pcreate(filename, 0, 0, 0);
                metadataInfoList[selectedClient].Add(filename, newMetadata);
                showMetadata(newMetadata);
            }
            catch (Exception exc) //TODO: FileAlreadyCreatedException
            {
                EventBox.Text += "File " + filename + " already created!\r\n";
            }
        }

        private void DeleteFileButton_Click(object sender, EventArgs e)
        {
            int selectedClient = ClientListBox.SelectedIndex;
            string filename = FilenameBox.Text;

            try
            {
                clientsList[selectedClient].pdelete(filename);
            }
            catch (Exception exc) //TODO: FileInUseException and FileNotExistsException
            {
                EventBox.Text += "File " + filename + " cannot be deleted!\r\n";
            }

        }

        private void showMetadata(MetadataInfo newMetadata)
        {
            EventBox.Text += "-------------------\r\nMETADATA\r\n";
            EventBox.Text += "Filename:" + newMetadata.filename + "\r\n";
            EventBox.Text += "NumDataServers:" + newMetadata.numDataServers + "\r\n";
            EventBox.Text += "ReadQuorum:" + newMetadata.readQuorum + "\r\n";
            EventBox.Text += "WriteQuorum:" + newMetadata.writeQuorum + "\r\n";
            EventBox.Text += "DataServers:" + newMetadata.dataServers + "\r\n"; 
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

    }
}
