using CommonTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text.RegularExpressions;
using System.Threading;

namespace PuppetMaster
{
    public partial class PuppetMaster
    {
        private Form1 form;

        private string projectFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        private string metadataExec = "MetadataServer\\bin\\Debug\\MetadataServer.exe";
        private string dataServerExec = "DataServer\\bin\\Debug\\DataServer.exe";
        private string clientExec = "Client\\bin\\Debug\\Client.exe";

        private List<string> scriptInstructions = new List<string>();
        private int currentInstruction;
        private string currentPath = "";

        private int[] metadataLocations = { 8081, 8082, 8083 };
        private BitArray metadataLaunched = new BitArray(3);

        private int clientPort = 8000;
        private int dataServerPort = 9000;

        /*
         * Class constructor. Used to initialize its tcp channel, which is used to 
         * communicate with other entities.
         */
        public PuppetMaster()
        {
            TcpChannel channel = (TcpChannel)Helper.GetChannel(8080);
            ChannelServices.RegisterChannel(channel, false);
        }

        /*
         * This method launches a client in a determined port, which begins at 8000
         * and increments for each new client. The locations of the metadataServers are passed
         * as arguments to the client, in order to establish posterior connections.
         */
        public void launchClient()
        {

            Process.Start(Path.Combine(projectFolder, clientExec), clientPort.ToString());

            IClientPuppet newClient = (IClientPuppet)Activator.GetObject(
                typeof(IClientPuppet),
                "tcp://localhost:" + clientPort + "/Client");
            Thread.Sleep(1000);
            newClient.init(metadataLocations);
            clientsList.Add(newClient);
            clientPort++;
        }

        public void launchMetadata(int selectedMetadata)
        {
            int location = metadataLocations[selectedMetadata];

            Process.Start(Path.Combine(projectFolder, metadataExec), location.ToString());
            Thread.Sleep(1000);
            IMetadataServerPuppet newMetadata = (IMetadataServerPuppet)Activator.GetObject(
               typeof(IMetadataServerPuppet),
               "tcp://localhost:" + location + "/MetadataServer");

            newMetadata.init(metadataLocations);
            metadataList[selectedMetadata] = newMetadata;
            metadataLaunched[selectedMetadata] = true;
        }

        /*
         * This method launches a data server in a determined port, which begins at 9000
         * and increments for each new data server. The locations of the metadataServers are passed
         * as arguments to the client, in order to establish posterior connections.
         */
        public void launchDataServer()
        {
            Process.Start(Path.Combine(projectFolder, dataServerExec), dataServerPort.ToString());
            Thread.Sleep(1000);
            IDataServerPuppet newDataServer = (IDataServerPuppet)Activator.GetObject(
                typeof(IDataServerPuppet),
                "tcp://localhost:" + dataServerPort + "/DataServer");

            newDataServer.init(metadataLocations);
            dataServersList.Add(newDataServer);
            dataServerPort++;
        }

        /*
         * The script is loaded from the "PADI/PuppetMaster" directory.
         */
        public void loadScript(string path)
        {
            string scriptText = "";
            int j = 0;
            string[] scriptLines = File.ReadAllLines(path);
            currentInstruction = 0; 
            currentPath = Directory.GetParent(path).FullName;

            foreach (string instruction in scriptLines)
                if (!instruction[0].Equals('#'))
                {
                    scriptInstructions.Add(instruction);
                    scriptText += j++ + ": " + instruction + "\r\n";
                }

            form.updateScriptText(scriptText, j);
            form.updateMessageBox("Loaded Script.");
        }

        /*
         * Runs a previously loaded script while ignoring the lines with an hash (comment)
         */
        public int runScript(int line)
        {
            if (line <= currentInstruction)
                return currentInstruction;

            for (int i = currentInstruction; i <= line; i++)
                interpretInstruction(scriptInstructions[i]);

            currentInstruction = line;
            return currentInstruction++;
        }

        public int nextStep()
        {
            interpretInstruction(scriptInstructions[currentInstruction]);
            return currentInstruction++;
        }

        public void interpretInstruction(string command)
        {
            string[] parameters = command.Split(',');
            string[] processInst = parameters[0].Split(' ');
            string instruction = processInst[0];
            string[] processInfo = processInst[1].Split('-');
            int processNumber = Convert.ToInt32(processInfo[1]) - 1;
            launchProcessIfNeeded(processInfo[0], processNumber);
            string textFile = "";

            //Removing whitespaces
            for (int i = 0; i < parameters.Length; i++)
            {
                if (i == 2)
                {
                    textFile = parameters[2].Replace("\"", "");
                }

                parameters[i] = parameters[i].Replace(" ", "");
            }

            switch (instruction)
            {
                case "WRITE":
                    try
                    {
                        int registerIndex = Convert.ToInt32(parameters[2]);
                        writeFile(Convert.ToInt32(parameters[1]), registerIndex, processNumber);
                    }
                    catch (FormatException)
                    {
                        writeFile(Convert.ToInt32(parameters[1]), textFile, processNumber);
                    }

                    break;
                case "READ":
                    readFile(Convert.ToInt32(parameters[1]), parameters[2], Convert.ToInt32(parameters[3]), processNumber);
                    break;
                case "OPEN":
                    openFile(parameters[1], processNumber);
                    break;
                case "CLOSE":
                    closeFile(parameters[1], processNumber);
                    break;
                case "CREATE":
                    createFile(parameters[1], Convert.ToInt32(parameters[2]), Convert.ToInt32(parameters[3]), Convert.ToInt32(parameters[4]), processNumber);
                    break;
                case "DELETE":
                    deleteFile(parameters[1], processNumber);
                    break;
                case "FREEZE":
                    freezeDataServer(processNumber);
                    break;
                case "UNFREEZE":
                    unfreezeDataServer(processNumber);
                    break;
                case "FAIL":
                    if (processInfo[0].Equals("d"))
                        failDataServer(processNumber);
                    else if (processInfo[0].Equals("m"))
                        failMetadata(processNumber + 1);
                    break;
                case "RECOVER":
                    if (processInfo[0].Equals("d"))
                        recoverDataServer(processNumber);
                    else if (processInfo[0].Equals("m"))
                        recoverMetadata(processNumber + 1);
                    break;
                case "DUMP":
                    switch (processInfo[0])
                    {
                        case "c":
                            dumpClient(processNumber);
                            break;
                        case "m":
                            dumpMetadataServer(processNumber + 1);
                            break;
                        case "d":
                            dumpDataServer(processNumber);
                            break;
                    }
                    break;
                case "COPY":
                    string salt = parameters[4].Replace("\"", "");
                    copy(processNumber, Convert.ToInt32(parameters[1]), parameters[2], Convert.ToInt32(parameters[3]), salt);
                    break;
                case "EXESCRIPT":
                    executeExescript(processNumber, processInst[2]);
                    break;
                default:
                    throw new IOException();
            }
        }

        private void launchProcessIfNeeded(string process, int processNumber)
        {
            switch (process)
            {
                case "c":
                    if (clientsList.Count <= processNumber)
                        launchClient();
                    break;
                case "m":
                    if (!metadataLaunched[processNumber + 1])
                        launchMetadata(processNumber + 1);
                    break;
                case "d":
                    if (dataServersList.Count <= processNumber)
                        launchDataServer();
                    break;
            }
        }

        public void setForm(Form1 form)
        {
            this.form = form;
        }
    }
}
