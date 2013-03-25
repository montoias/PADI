using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Diagnostics;
using System.IO;

using CommonTypes;

namespace PuppetMaster
{

    //TODO : read from config file
    public class PuppetMaster
    {
        
        private List<IPuppetClientServer> clientsList = new List<IPuppetClientServer>();
        private List<Dictionary<string, MetadataInfo>> metadataInfoList = new List<Dictionary<string, MetadataInfo>>();
        private string projectFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        private IPuppetMetadataServer[] metadataList = new IPuppetMetadataServer[3];

        //TODO: replace with full remote location
        private string[] metadataLocation = { "8081", "8082", "8083" };

        private List<string> clientsPorts = new List<string>();
        private BitArray metadataLaunched = new BitArray(3);
        private Form1 form;


        public PuppetMaster()
        {
            TcpChannel channel = (TcpChannel)Helper.GetChannel(8080, true);
            ChannelServices.RegisterChannel(channel, true);
        }

        public void setForm(Form1 form)
        {
            this.form = form;
        }

        public void LaunchClient(String port)
        {
            if (!clientsPorts.Contains(port))
            {
                string args = port + " " + metadataLocation[0] + " " + metadataLocation[1] + " " + metadataLocation[2];

                //TODO: Implement remote starting -> use WMI
                Process.Start(Path.Combine(projectFolder, "ClientServer\\bin\\Debug\\ClientServer.exe"), args);

                IPuppetClientServer newClient = (IPuppetClientServer)Activator.GetObject(
                   typeof(IPuppetClientServer),
                   "tcp://localhost:" + port + "/ClientServer");

                clientsList.Add(newClient);
                form.addClient();
                clientsPorts.Add(port);
                metadataInfoList.Add(new Dictionary<string, MetadataInfo>());
            }
            else
            {
                form.updateMessage("Client already exists at port " + port + "!\r\n");
            }

        }

        public void LaunchMetadata(int selectedMetadata)
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
                form.updateMessage("Metadata Server " + selectedMetadata + " already launched!\r\n");
            }
        }

        public void OpenFile(string filename, int selectedClient)
        {
            try
            {
                MetadataInfo newMetadata = clientsList[selectedClient].popen(filename);
                metadataInfoList[selectedClient].Add(filename, newMetadata);
                form.showMetadata(newMetadata);
            }
            catch (Exception exc) //TODO: FileAlreadyOpenException
            {
                form.updateMessage("File " + filename + " already opened!\r\n");
            }
        }

        public void CloseFile(string filename, int selectedClient)
        {
            try
            {
                clientsList[selectedClient].pclose(filename);
                metadataInfoList[selectedClient].Remove(filename);
            }
            catch (Exception exc) //TODO: FileNotOpenException
            {
                form.updateMessage("File " + filename + " was not opened!\r\n");
            }
        }

        public void createFile(string filename, int selectedClient)
        {
            //TODO: Interface details
            try
            {
                MetadataInfo newMetadata = clientsList[selectedClient].pcreate(filename, 0, 0, 0);
                metadataInfoList[selectedClient].Add(filename, newMetadata);
                //showMetadata(newMetadata);
            }
            catch (Exception exc) //TODO: FileAlreadyCreatedException
            {
                form.updateMessage("File " + filename + " already created!\r\n");
            }
        }

        public void deleteFile(string filename, int selectedClient)
        {
            try
            {
                clientsList[selectedClient].pdelete(filename);
            }
            catch (Exception exc) //TODO: FileInUseException and FileNotExistsException
            {
                form.updateMessage("File " + filename + " cannot be deleted!\r\n");
            }
        }

    }
}
