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
using CommonInterfaces;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Diagnostics;
using System.IO;

namespace PuppetMaster
{
    public partial class Form1 : Form
    {
        TcpChannel channel;
        private List<IPuppetClient> clientsList = new List<IPuppetClient>();
        private List<MetadataInfo> metadataInfoList = new List<MetadataInfo>(); // TODO: replace by bounded list
        private string projectFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        //TODO: Replace by dictionary filename->metadata with boundaries
        private List<IPuppetMetadata> metadataList = new List<IPuppetMetadata>(); 

        public Form1()
        {
            channel = (TcpChannel)Helper.GetChannel(8080, true);
            ChannelServices.RegisterChannel(channel, true);

            InitializeComponent();
        }

        private void LaunchClient_Click(object sender, EventArgs e)
        {
            //TODO: Implement remote starting -> use WMI
            Process.Start(Path.Combine(projectFolder, "Client\\bin\\Debug\\Client.exe"));
            IPuppetClient newClient = (IPuppetClient)Activator.GetObject(
               typeof(IPuppetClient),
               "tcp://localhost:8086/Client");
            clientsList.Add(newClient);
        }

        private void LaunchMetadataButton_Click(object sender, EventArgs e)
        {
            //TODO: Implement remote starting -> use WMI
            Process.Start(Path.Combine(projectFolder, "Metadata\\bin\\Debug\\Metadata.exe"));
            IPuppetMetadata newMetadata = (IPuppetMetadata)Activator.GetObject(
               typeof(IPuppetMetadata),
               "tcp://localhost:8081/Metadata");
            metadataList.Add(newMetadata);
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            //TODO: Obtain previously the current client
            MetadataInfo newMetadata = clientsList[0].popen(FilenameBox.Text);
            metadataInfoList.Add(newMetadata);
            showMetadata(newMetadata);
        }

        private void CreateFileButton_Click(object sender, EventArgs e)
        {
            //TODO: Catch Exception
            //TODO: Obtain previously the current client
            MetadataInfo newMetadata = clientsList[0].pcreate(FilenameBox.Text, 0, 0, 0);
            metadataInfoList.Add(newMetadata);
            showMetadata(newMetadata);
        }

        private void DeleteFileButton_Click(object sender, EventArgs e)
        {
            clientsList[0].pdelete(FilenameBox.Text);

        }

        private void CloseFileButton_Click(object sender, EventArgs e)
        {
           clientsList[0].pclose(FilenameBox.Text);
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

    }
}
