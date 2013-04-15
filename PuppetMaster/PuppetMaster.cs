﻿using CommonTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text.RegularExpressions;
using System.Runtime.Remoting.Messaging;

namespace PuppetMaster
{
    public class PuppetMaster
    {

        private Form1 form;

        private string projectFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        private string metadataExec = "MetadataServer\\bin\\Debug\\MetadataServer.exe";
        private string dataServerExec = "DataServer\\bin\\Debug\\DataServer.exe";
        private string clientExec = "ClientServer\\bin\\Debug\\ClientServer.exe";

        private List<string> scriptInstructions = new List<string>();
        private int currentInstruction;

        private List<IClientServerPuppet> clientsList = new List<IClientServerPuppet>();
        private List<IDataServerPuppet> dataServersList = new List<IDataServerPuppet>();

        private IMetadataServerPuppet[] metadataList = new IMetadataServerPuppet[3];
        private string[] metadataLocation = { "8081", "8082", "8083" };
        private BitArray metadataLaunched = new BitArray(3);

        private int clientPort = 8000;
        private int dataServerPort = 9000;

        public delegate void ExescriptDelegate(int selectedClient, string filename);

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
            clientsList[selectedClient].open(filename);
        }

        public void closeFile(string filename, int selectedClient)
        {
            clientsList[selectedClient].close(filename);
        }

        public void createFile(string filename, int nServers, int readQuorum, int writeQuorum, int selectedClient)
        {
            clientsList[selectedClient].create(filename, nServers, readQuorum, writeQuorum);
        }

        public void deleteFile(string filename, int selectedClient)
        {
            clientsList[selectedClient].delete(filename);
        }

        public void writeFile(int fileRegister, string file, int selectedClient)
        {
            clientsList[selectedClient].write(fileRegister, file);
        }

        public void writeFile(int fileRegister, int byteRegister, int selectedClient)
        {
            clientsList[selectedClient].write(fileRegister, byteRegister);
        }

        public void readFile(int fileRegister, string semantics, int byteRegister, int selectedClient)
        {
            FileData fileData = clientsList[selectedClient].read(fileRegister, semantics, byteRegister);

            if (fileData != null)
                form.updateFileText(Utils.byteArrayToString(fileData.file), fileData.version);
        }

        public void copy(int selectedClient, int fileRegister1, string semantics, int fileRegister2, string salt)
        {
            clientsList[selectedClient].copy(fileRegister1, semantics, fileRegister2, salt);
        }

        public void dumpClient(int selectedClient)
        {
            clientsList[selectedClient].dump();
        }

        public void exescript(int selectedClient, string filename)
        {
            string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, filename);
            string[] fileText = File.ReadAllLines(path);
            clientsList[selectedClient].exescript(fileText);
        }

        private void exescriptAsync(int selectedClient, string filename)
        {
            exescript(selectedClient, filename);
        }

        private void exescriptAsyncCallBack(IAsyncResult ar)
        {
            ExescriptDelegate exescriptDelegate = (ExescriptDelegate)((AsyncResult)ar).AsyncDelegate;
            exescriptDelegate.EndInvoke(ar);
        }
        
        /****************************
         ********* METADATA *********
         ****************************/
        public void failMetadata(int selectedMetadata)
        {
            //metadataList[selectedMetadata].fail();
        }

        public void recoverMetadata(int selectedMetadata)
        {
            //metadataList[selectedMetadata].recover();
        }

        public void dumpMetadataServer(int selectedMetadata)
        {
            metadataList[selectedMetadata].dump();
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
            dataServersList[selectedDataServer].unfreeze();
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
            dataServersList[selectedDataServer].dump();
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

            Process.Start(Path.Combine(projectFolder, clientExec), args);

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

            Process.Start(Path.Combine(projectFolder, metadataExec), location);
            IMetadataServerPuppet newMetadata = (IMetadataServerPuppet)Activator.GetObject(
               typeof(IMetadataServerPuppet),
               "tcp://localhost:" + location + "/MetadataServer");
            metadataList[selectedMetadata] = newMetadata;
            metadataLaunched[selectedMetadata] = true;
            form.addMetadataServer();
        }

        /*
         * This method launches a data server in a determined port, which begins at 9000
         * and increments for each new data server. The locations of the metadataServers are passed
         * as arguments to the client, in order to establish posterior connections.
         */
        public void launchDataServer()
        {
            string args = dataServerPort + " " + metadataLocation[0] + " " + metadataLocation[1] + " " + metadataLocation[2];

            Process.Start(Path.Combine(projectFolder, dataServerExec), args);
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
            string scriptText = "";
            int i = 0;
            string[] scriptLines = File.ReadAllLines(path);
            currentInstruction = 0;

            foreach (string instruction in scriptLines)
                if (!instruction[0].Equals('#'))
                {
                    scriptInstructions.Add(instruction);
                    scriptText += i++ + ": " + instruction + "\r\n";
                }

            form.updateScriptText(scriptText, i);
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

        private void interpretInstruction(string command)
        {
            string[] parameters = Regex.Split(command, ", ");
            string[] processInst = parameters[0].Split(' ');
            string instruction = processInst[0];
            string[] processInfo = processInst[1].Split('-');
            int processNumber = Convert.ToInt32(processInfo[1]) - 1;

            launchProcessIfNeeded(processInfo[0], processNumber);

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
                        string textFile = parameters[2].Replace("\"", "");
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
                    ExescriptDelegate exescriptDelegate = new ExescriptDelegate(exescriptAsync);
                    AsyncCallback exescriptCallback = new AsyncCallback(exescriptAsyncCallBack);
                    exescriptDelegate.BeginInvoke(processNumber, processInst[2], exescriptCallback, null);
                    break;
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

        public void killProcesses()
        {
            foreach (Process proc in Process.GetProcessesByName("DataServer"))
                proc.Kill();

            foreach (Process proc in Process.GetProcessesByName("ClientServer"))
                proc.Kill();

            foreach (Process proc in Process.GetProcessesByName("MetadataServer"))
                proc.Kill();

            clientPort = 8000;
            dataServerPort = 9000;
            metadataLaunched.SetAll(false);
        }
    }
}
