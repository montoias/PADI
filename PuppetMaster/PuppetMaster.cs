using System;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Diagnostics;
using System.IO;

using CommonTypes;
using System.Text.RegularExpressions;
using System.Text;

namespace PuppetMaster
{
    public class PuppetMaster 
    {

        private Form1 form;
        private string projectFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        private string[] scriptInstructions;
        private int currentInstruction;

        private List<IClientServerPuppet> clientsList = new List<IClientServerPuppet>();
        private List<IDataServerPuppet> dataServersList = new List<IDataServerPuppet>();

        private IMetadataServerPuppet[] metadataList = new IMetadataServerPuppet[3];
        private string[] metadataLocation = { "8081", "8082", "8083" };
        private BitArray metadataLaunched = new BitArray(3);
        
        private int clientPort = 8000;
        private int dataServerPort = 9000;

        /*
         * Class constructor. Used to initialize its tcp channel, which is used to 
         * communicate with other entities.
         */
        public PuppetMaster()
        {
            TcpChannel channel = (TcpChannel)Helper.GetChannel(8080, true);
            ChannelServices.RegisterChannel(channel, true);
        }

        /**************************
         ********* CLIENT *********
         **************************/

        public void openFile(string filename, int selectedClient)
        {
            try
            {
                form.showMetadata(clientsList[selectedClient].open(filename));
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
                form.showMetadata(clientsList[selectedClient].create(filename, nServers, readQuorum, writeQuorum));
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
                form.updateClientBox("There aren't enough servers for the request!\r\n");
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

        public void writeFile(int fileRegister, string file, int selectedClient)
        {
            try
            {
                clientsList[selectedClient].write(fileRegister, file);
            }
            catch (Exception e) //TODO: FileInUseException 
            {
                form.updateClientBox(e.StackTrace);
            }
            
        }

        public void writeFile(int fileRegister, int byteRegister, int selectedClient)
        {
            try
            {
                clientsList[selectedClient].write(fileRegister, byteRegister);
            }
            catch (Exception e) //TODO: FileInUseException 
            {
                form.updateClientBox(e.Message);
            }

        }

        public void readFile(int fileRegister, string semantics, int byteRegister, int selectedClient)
        {
            FileData fileData = null;
            try
            {
                fileData = clientsList[selectedClient].read(fileRegister, semantics, byteRegister);
            }
            catch (Exception e) 
            {
                form.updateClientBox(e.Message);
            }

            form.updateFileText(byteArrayToString(fileData.file), fileData.version);

        }

        public void dumpClient(int selectedClient)
        {
            form.updateClientBox(clientsList[selectedClient].dump());
        }

        /****************************
         ********* METADATA *********
         ****************************/
        public void failMetadata(int selectedMetadata)
        {
        }

        public void recoverMetadata(int selectedMetadata)
        {
        }

        public void dumpMetadataServer(int selectedMetadata)
        {
            //iterate through metadatas and dump each one
            form.updateMetadataBox(metadataList[selectedMetadata].dump());
        }


        /*******************************
         ********* DATA SERVER *********
         *******************************/
        public void freezeDataServer(int selectedDataServer)
        {
            dataServersList[selectedDataServer].freeze();
        }

        public void unfreezeDataServer(int selectedDataServer)
        {
            //dataServersList[selectedDataServer].unfreeze();
        }

        public void failDataServer(int selectedDataServer)
        {
            dataServersList[selectedDataServer].fail();
        }

        public void recoverDataServer(int selectedDataServer)
        {
            dataServersList[selectedDataServer].recover();
        }

        public void dumpDataServer(int selectedDataServer)
        {
            form.updateDataServerBox(dataServersList[selectedDataServer].dump());
        }

        /*********************************
         ********* PUPPET MASTER *********
         *********************************/

        /*
         * This method launches a client in a determined port, which begins at 8000
         * and increments for each new client. The locations of the metadataServers are passed
         * as arguments to the client, in order to establish posterior connections.
         */
        public void launchClient()
        {
            string args = clientPort + " " + metadataLocation[0] + " " + metadataLocation[1] + " " + metadataLocation[2];

            Process.Start(Path.Combine(projectFolder, "ClientServer\\bin\\Debug\\ClientServer.exe"), args);

            IClientServerPuppet newClient = (IClientServerPuppet)Activator.GetObject(
                typeof(IClientServerPuppet),
                "tcp://localhost:" + clientPort + "/ClientServer");

            clientsList.Add(newClient);
            form.addClient();
            clientPort++;
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

        /*
         * This method launches a data server in a determined port, which begins at 9000
         * and increments for each new data server. The locations of the metadataServers are passed
         * as arguments to the client, in order to establish posterior connections.
         */
        public void launchDataServer()
        {
            string args = dataServerPort + " " + metadataLocation[0] + " " + metadataLocation[1] + " " + metadataLocation[2];

            Process.Start(Path.Combine(projectFolder, "DataServer\\bin\\Debug\\DataServer.exe"), args);
            IDataServerPuppet newDataServer = (IDataServerPuppet)Activator.GetObject(
                typeof(IDataServerPuppet),
                "tcp://localhost:" + dataServerPort + "/DataServer");
            dataServersList.Add(newDataServer);
            form.addDataServer();
            dataServerPort++;
        }

        /*
         * The script is loaded from the "PADI/PuppetMaster" directory.
         */
        public void loadScript(string scriptFile)
        {
            string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, scriptFile);
            scriptInstructions = File.ReadAllLines(path);
            currentInstruction = 0;
        }

        /*
         * Runs a previously loaded script while ignoring the lines with an hash (comment)
         */
        public void runScript()
        {
            foreach (string instruction in scriptInstructions)
            {
                if (!instruction[0].Equals('#'))
                {
                    interpretInstruction(instruction);
                }
            }
        }

        public void nextStep()
        {
            while ((scriptInstructions[currentInstruction][0]).Equals('#'))
                currentInstruction++;

            interpretInstruction(scriptInstructions[currentInstruction++]);
        }
        
        private void interpretInstruction(string command)
        {
            string[] parameters = Regex.Split(command, ", ");
            string[] processInst = parameters[0].Split(' ');
            string instruction = processInst[0];
            string[] processInfo = processInst[1].Split('-');
            int processNumber = Convert.ToInt32(processInfo[1])-1;
            
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
                case "WRITE":
                    if (processInfo[0].Equals("c"))
                    {
                        try
                        {
                            int registerIndex = Convert.ToInt32(parameters[2]);
                            writeFile(Convert.ToInt32(parameters[1]), registerIndex, processNumber);
                        }
                        catch (FormatException)
                        {
                            string textFile = parameters[2].Replace("\"", "");
                            writeFile(Convert.ToInt32(parameters[1]), textFile, processNumber);
                        }
                        
                    }
                    break;
                case "READ":
                    if (processInfo[0].Equals("c"))
                    {
                        readFile(Convert.ToInt32(parameters[1]), parameters[2], Convert.ToInt32(parameters[3]), processNumber);
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
                case "FREEZE":
                    if (processInfo[0].Equals("d"))
                    {
                        freezeDataServer(processNumber);
                    }
                    break;
                case "UNFREEZE":
                    if (processInfo[0].Equals("d"))
                    {
                        unfreezeDataServer(processNumber);
                    }
                    break;
                case "FAIL":
                    if (processInfo[0].Equals("d"))
                    {
                        failDataServer(processNumber);
                    }
                    else if (processInfo[0].Equals("m"))
                    {
                        failMetadata(processNumber);
                    }
                    break;
                case "RECOVER":
                    if (processInfo[0].Equals("d"))
                    {
                        recoverDataServer(processNumber);
                    }
                    else if (processInfo[0].Equals("m"))
                    {
                        recoverMetadata(processNumber);
                    }
                    break;
                case "DUMP":
                    switch (processInfo[0])
                    {
                        case "c":
                            dumpClient(processNumber);
                            break;
                        case "m":
                            dumpMetadataServer(processNumber);
                            break;
                        case "d":
                            dumpDataServer(processNumber);
                            break;
                    }
                    break;
                case "COPY":
                    break;
                case "EXESCRIPT":
                    break;
            }
        }

        public void setForm(Form1 form)
        {
            this.form = form;
        }

        public void killProcesses()
        {
            foreach (Process proc in Process.GetProcessesByName("DataServer"))
            {
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
            metadataLaunched.SetAll(false);
        }

        private string byteArrayToString(byte[] b)
        {
            char[] chars = new char[b.Length / sizeof(char)];
            System.Buffer.BlockCopy(b, 0, chars, 0, b.Length);
            return new string(chars);
        }
    }
}
