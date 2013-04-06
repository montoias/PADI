using System;
using System.Collections.Generic;
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

        private Form1 form;
        private string projectFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        private string[] scriptInstructions;

        private List<IClientServerPuppet> clientsList = new List<IClientServerPuppet>();
        private List<IDataServerPuppet> dataServersList = new List<IDataServerPuppet>();

        private List<Dictionary<string, MetadataInfo>> metadataInfoList = new List<Dictionary<string, MetadataInfo>>();
        private List<Dictionary<string, FileData>> fileInfoList = new List<Dictionary<string, FileData>>();

        private IMetadataServerPuppet[] metadataList = new IMetadataServerPuppet[3];
        private string[] metadataLocation = { "8081", "8082", "8083" };
        private BitArray metadataLaunched = new BitArray(3);

        
        private int clientPort = 8000;
        private int dataServerPort = 9000;
        private List<int> usedPorts = new List<int>();



        public PuppetMaster()
        {
            TcpChannel channel = (TcpChannel)Helper.GetChannel(8080, true);
            ChannelServices.RegisterChannel(channel, true);
            addKnownPorts();
        }

        private void addKnownPorts()
        {
            usedPorts.Add(8080);
            foreach (string port in metadataLocation)
            {
                usedPorts.Add(Convert.ToInt32(port));
            }
        }

        public void setForm(Form1 form)
        {
            this.form = form;
        }

        public void launchClient()
        {
            string args = clientPort + " " + metadataLocation[0] + " " + metadataLocation[1] + " " + metadataLocation[2];

            Process.Start(Path.Combine(projectFolder, "ClientServer\\bin\\Debug\\ClientServer.exe"), args);

            IClientServerPuppet newClient = (IClientServerPuppet)Activator.GetObject(
                typeof(IClientServerPuppet),
                "tcp://localhost:" + clientPort + "/ClientServer");

            clientsList.Add(newClient);
            form.addClient();
            usedPorts.Add(clientPort++);
            metadataInfoList.Add(new Dictionary<string, MetadataInfo>());
            

        }

        public void launchMetadata(int selectedMetadata)
        {
            string location = metadataLocation[selectedMetadata];

            if (!metadataLaunched[selectedMetadata])
            {
                Process.Start(Path.Combine(projectFolder, "MetadataServer\\bin\\Debug\\MetadataServer.exe"), location);
                IMetadataServerPuppet newMetadata = (IMetadataServerPuppet)Activator.GetObject(
                   typeof(IMetadataServerPuppet),
                   "tcp://localhost:" + location + "/MetadataServer");
                metadataList[selectedMetadata] = newMetadata;
                metadataLaunched[selectedMetadata] = true;
            }
            else
            {
                form.updateClientBox("Metadata Server " + selectedMetadata + " already launched!\r\n");
            }
        }

        public void launchDataServer()
        {
            string args = dataServerPort + " " + metadataLocation[0] + " " + metadataLocation[1] + " " + metadataLocation[2];

            Process.Start(Path.Combine(projectFolder, "DataServer\\bin\\Debug\\DataServer.exe"), args);
            IDataServerPuppet newDataServer = (IDataServerPuppet)Activator.GetObject(
                typeof(IDataServerPuppet),
                "tcp://localhost:" + dataServerPort + "/DataServer");
            dataServersList.Add(newDataServer);
            form.addDataServer();
            usedPorts.Add(dataServerPort++);
            fileInfoList.Add(new Dictionary<string, FileData>());
        }

        public void openFile(string filename, int selectedClient)
        {
            try
            {
                MetadataInfo newMetadata = clientsList[selectedClient].open(filename);
                metadataInfoList[selectedClient].Add(filename, newMetadata);
                form.showMetadata(newMetadata);
            }
            catch (FileAlreadyOpenedException) 
            {
                form.updateClientBox("File " + filename + " already opened!\r\n");
            }
            catch (TableSizeExcedeedException) 
            {
                form.updateClientBox("There are 10 slots occupied, you should close one before trying to open a file!\r\n");
            }
            catch (FileDoesNotExistException)
            {
                form.updateClientBox("The file: " + filename + " you've tried to open doesn't exist!\r\n");
            }
        }

        public void closeFile(string filename, int selectedClient)
        {
            try
            {
                clientsList[selectedClient].close(filename);
                metadataInfoList[selectedClient].Remove(filename);
            }
            catch (FileNotOpenedException)
            {
                form.updateClientBox("File " + filename + " was not opened!\r\n");
            }
        }

        public void createFile(string filename, int nServers, int readQuorum, int writeQuorum, int selectedClient)
        {
            try
            {
                MetadataInfo newMetadata = clientsList[selectedClient].create(filename, nServers, readQuorum, writeQuorum);
                metadataInfoList[selectedClient].Add(filename, newMetadata);
                form.showMetadata(newMetadata);
            }
            catch (TableSizeExcedeedException)
            {
                form.updateClientBox("There are 10 slots occupied, you should close one before trying to create a file!\r\n");
            }
            catch (FileAlreadyExistsException)
            {
                form.updateClientBox("File " + filename + " already created!\r\n");
            }
            catch (InsufficientDataServersException)
            {
                form.updateClientBox("There aren't enough servers for the request!~\r\n");
            }
        }

        public void deleteFile(string filename, int selectedClient)
        {
            try
            {
                clientsList[selectedClient].delete(filename);
            }
            catch (FileDoesNotExistException) 
            {
                form.updateClientBox("File " + filename + " does not exist!\r\n");
            }
            catch (CannotDeleteFileException)
            {
                form.updateClientBox("The file cannot be deleted because is being used!\r\n");
            }
        }

        public void writeFile(string filename, string file, int selectedClient)
        {
            try
            {
                clientsList[selectedClient].write(filename, file);
            }
            catch (MetadataFileDoesNotExistException) //TODO: FileInUseException 
            {
                form.updateClientBox("You have to have the metadata file in order to perform a write!\r\n");
            }
            catch (Exception e) //TODO: FileInUseException 
            {
                form.updateClientBox(e.Message);
            }
            
        }

        public void readFile(string filename, int semantics, int selectedClient)
        {
            FileData fileData = null;

            try
            {
                fileData = clientsList[selectedClient].read(filename, 0);
                fileInfoList[selectedClient].Add(filename, fileData);
            }
            catch (MetadataFileDoesNotExistException)
            {
                form.updateClientBox("You have to have the metadata file in order to perform a read!\r\n");
            }
            catch (FileDoesNotExistException)
            {
                form.updateClientBox("File " + filename + " does not exist!\r\n");
            }
            catch (ArgumentException)
            {
                fileInfoList[selectedClient][filename] = fileData;
            }

            form.updateFileText(byteArrayToString(fileData.file), fileData.version);

        }

        public void failMetadata()
        {
            throw new NotImplementedException();
        }

        public void recoverMetadata()
        {
            throw new NotImplementedException();
        }

        public void dumpMetadataServer(int selectedMetadata)
        {
            //iterate through metadatas and dump each one
            form.updateMetadataBox(metadataList[selectedMetadata].dump());
        }

        public void freezeDataServer()
        {
            throw new NotImplementedException();
        }

        public void unfreezeDataServer()
        {
            throw new NotImplementedException();
        }

        public void failDataServer(int selectedDataServer)
        {
            dataServersList[selectedDataServer].fail();
        }

        public void recoverDataServer(int selectedDataServer)
        {
            dataServersList[selectedDataServer].recover();
        }

        public void killProcesses()
        {
            foreach(Process proc in Process.GetProcessesByName("DataServer")){
                proc.Kill();
            }
            foreach (Process proc in Process.GetProcessesByName("ClientServer"))
            {
                proc.Kill();
            }
            foreach (Process proc in Process.GetProcessesByName("MetadataServer"))
            {
                proc.Kill();
            }

            clientPort = 8000;
            dataServerPort = 9000;
            usedPorts.Clear();
            metadataLaunched.SetAll(false);
            addKnownPorts();
        }

        public void loadScript(string scriptFile)
        {
            string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, scriptFile);
            scriptInstructions = File.ReadAllLines(path);
        }

        public void runScript()
        {
            foreach (string instruction in scriptInstructions)
            {
                if (instruction[0] != '#')
                {
                    interpretInstruction(instruction);
                }
            }
        }


        public void nextStep()
        {
        }
        
        private void interpretInstruction(string command)
        {
            string [] parameters = command.Split(',');
            string[] processInst = parameters[0].Split(' ');
            string instruction = processInst[0];
            string[] processInfo = processInst[1].Split('-');
            int processNumber = Convert.ToInt32(processInfo[1]);

            switch (processInfo[0])
            {
                case "c":
                    if(clientsList.Count <= processNumber)
                    {
                        launchClient();
                    }
                    break;
                case "m":
                    if(!metadataLaunched[processNumber])
                    {
                        launchMetadata(processNumber);
                    }
                    break;
                case "d":
                    if(dataServersList.Count <= processNumber)
                    {
                        launchDataServer();
                    }
                    break;
            }

            switch (instruction)
            {
                case "FREEZE":
                    if (processInfo[0].Equals("d"))
                    {
                        freezeDataServer();
                    }
                    break;
                case "UNFREEZE":
                    if (processInfo[0].Equals("d"))
                    {
                        unfreezeDataServer();
                    }
                    break;
                case "CREATE":
                    if (processInfo[0].Equals("c"))
                    {
                        createFile(parameters[1], Convert.ToInt32(parameters[2]), Convert.ToInt32(parameters[3]), Convert.ToInt32(parameters[4]), processNumber);
                    }
                    break;
                case "DELETE":
                    if (processInfo[0].Equals("c"))
                    {
                        deleteFile(parameters[1], processNumber);
                    }
                    break;
                case "OPEN":
                    if (processInfo[0].Equals("c"))
                    {
                        openFile(parameters[1], processNumber);
                    }
                    break;
                case "CLOSE":
                    if (processInfo[0].Equals("c"))
                    {
                        closeFile(parameters[1], processNumber);
                    }
                    break;
                case "WRITE":
                    if (processInfo[0].Equals("c"))
                    {
                        /*UNFINISHED*/
                        writeFile(parameters[1], processNumber);
                    }
                    break;
                case "READ":
                case "FAIL":
                    if (processInfo[0].Equals("d"))
                    {
                    }
                    else if(processInfo[0].Equals("m"))
                    {
                    }
                    break;
                case "RECOVER":
                    if (processInfo[0].Equals("d"))
                    {
                    }
                    else if(processInfo[0].Equals("m"))
                    {
                    }
                    break;
                case "DUMP":
                    switch (processInfo[0])
                    {
                        case "c":
                            break;
                        case "m":
                            break;
                        case "d":
                            break;
                    }
                    break;
                case "EXESCRIPT":
                    break;
                default:
                    break;
            }
        }

        private string byteArrayToString(byte[] b)
        {
            char[] chars = new char[b.Length / sizeof(char)];
            System.Buffer.BlockCopy(b, 0, chars, 0, b.Length);
            return new string(chars);
        }
    }
}
