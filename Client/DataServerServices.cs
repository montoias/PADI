using System;
using System.Collections.Generic;
using CommonTypes;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using System.Net.Sockets;
using System.IO;
using System.Collections;

namespace Client
{
    public partial class Client
    {
        public delegate void WriteDelegate(IDataServerClient dataServer, string localFilename, FileData file);
        public delegate FileData ReadDelegate(IDataServerClient dataServer, string localFilename);

        private void writeAsync(IDataServerClient dataServer, string localFilename, FileData file)
        {
            dataServer.write(localFilename, file);
        }

        private FileData readAsync(IDataServerClient dataServer, string localFilename)
        {
            return dataServer.read(localFilename);
        }

        public FileData read(int fileRegister, string semantics, int byteRegister)
        {
            System.Console.WriteLine("Reading metadata from file register " + fileRegister + " to byte register " + byteRegister);
            FileData fileData = readOnly(fileRegister, semantics);
            byteRegisters[byteRegister] = fileData;

            return fileData;
        }

        /*
         * This is a different function than read, in order to be able to use the 'copy' function
         * without changing the registers.
         */
        private FileData readOnly(int fileRegister, string semantics)
        {
            MetadataInfo metadata = checkMetadata(fileRegister); 
            FileData fileData = readQuorum(metadata, metadata.readQuorum);

            if (semantics.Equals("monotonic") && fileVersions.ContainsKey(metadata.filename) && (fileVersions[metadata.filename] >= fileData.version))
            {
                System.Console.WriteLine("Monotonic read was requested: The file obtained was older than the one read before! Retrying...");
                return readOnly(fileRegister, semantics);
            }

            fileVersions[metadata.filename] = fileData.version;
            System.Console.WriteLine("File Read. \r\nVersion " + fileData.version + ": " + Utils.byteArrayToString(fileData.file));
            return fileData;
        }

        private FileData readQuorum(MetadataInfo metadata, int quorumNum)
        {
            ReadDelegate readDelegate = new ReadDelegate(readAsync);
            Dictionary<int, FileData> versionResults = new Dictionary<int, FileData>();
            Dictionary<int, int> versionCounter = new Dictionary<int, int>();
            Queue<LocalFilenameInfo> availableServers = new Queue<LocalFilenameInfo>();
            List<IAsyncResult> results = new List<IAsyncResult>();
            BitArray completed = new BitArray(metadata.numDataServers);
            int numResults = 0, maxResults = 0, maxVersion = 0;
            int j;

            System.Console.WriteLine("METADTA C-DSE \r\n" + metadata);

            for (j = 0; j < quorumNum; j++)
            {
                results.Add(readDelegate.BeginInvoke(getDataServer(metadata, metadata.dataServers[j].location), metadata.dataServers[j].localFilename, null, null));
            }
            for (; j < metadata.dataServers.Count; j++)
            {
                availableServers.Enqueue(metadata.dataServers[j]);
            }

            while (numResults < quorumNum)
            {
                System.Console.WriteLine("Waiting for quorum");
                for (int i = 0; i < results.Count; i++)
                {
                    if (results[i].IsCompleted && !completed[i])
                    {
                        try
                        {
                            FileData tempFile = ((ReadDelegate)((AsyncResult)results[i]).AsyncDelegate).EndInvoke(results[i]);
                            if (versionCounter.ContainsKey(tempFile.version))
                                versionCounter[tempFile.version]++;
                            else
                            {
                                versionResults[tempFile.version] = tempFile;
                                versionCounter[tempFile.version] = 1;
                            }
                            numResults++;
                            completed[i] = true;
                        }
                        catch (SocketException)
                        {
                            LocalFilenameInfo dataServerInfo = availableServers.Dequeue();
                            results.Add(readDelegate.BeginInvoke(getDataServer(metadata, dataServerInfo.location), dataServerInfo.localFilename, null, null));
                            availableServers.Enqueue(metadata.dataServers[i]);
                        }
                        catch (IOException)
                        {
                            LocalFilenameInfo dataServerInfo = availableServers.Dequeue();
                            results.Add(readDelegate.BeginInvoke(getDataServer(metadata, dataServerInfo.location), dataServerInfo.localFilename, null, null));
                            availableServers.Enqueue(metadata.dataServers[i]);
                        }
                    }
                }
                Thread.Sleep(1000);
            }

            foreach (int version in versionCounter.Keys)
            {
                System.Console.WriteLine("Version:" + version);
                if (versionCounter[version] > maxResults)
                {
                    maxResults = versionCounter[version];
                    maxVersion = version;
                }
            }

            System.Console.WriteLine("Max Version is: " + maxVersion);

            return versionResults[maxVersion];
        }

        /*
         * This write method is called when the contents being written
         * are passed as string.
         * Note that in order to obtain the most recent version, a read is performed,
         * until the writeQuorum is reached (not the readQuorum).
         */
        public void write(int fileRegister, string textFile)
        {
            System.Console.WriteLine("Writing to DS where metadata is from file register " + fileRegister);

            MetadataInfo metadata = checkMetadata(fileRegister);
            
            writeQuorum(metadata, new FileData(Utils.stringToByteArray(textFile), readQuorum(metadata, metadata.writeQuorum).version + 1, clientID));
        }

        /*
         * This write method is called when the contents being written
         * are passed by a byteRegister.
         * Note that in order to obtain the most recent version, a read is performed,
         * until the writeQuorum is reached (not the readQuorum).
         */
        public void write(int fileRegister, int byteRegister)
        {
            System.Console.WriteLine("Writing to DS where metadata is from file register " + fileRegister);
            System.Console.WriteLine("Contents to write were from byte register " + byteRegister);

            MetadataInfo metadata = checkMetadata(fileRegister);
            FileData fileData = byteRegisters[byteRegister];
            fileData.version = readQuorum(metadata, metadata.writeQuorum).version + 1;
            fileData.clientID = clientID;
            writeQuorum(metadata, fileData);
        }

        private void writeQuorum(MetadataInfo metadata, FileData fileData)
        {
            WriteDelegate writeDelegate = new WriteDelegate(writeAsync);
            Queue<LocalFilenameInfo> availableServers = new Queue<LocalFilenameInfo>();
            List<IAsyncResult> results = new List<IAsyncResult>();
            BitArray completed = new BitArray(metadata.numDataServers);
            int numResults = 0;
            int j;

            for (j = 0; j < metadata.readQuorum; j++)
            {
                results.Add(writeDelegate.BeginInvoke(getDataServer(metadata, metadata.dataServers[j].location), metadata.dataServers[j].localFilename, fileData, null, null));
            }
            for (; j < metadata.dataServers.Count; j++)
            {
                availableServers.Enqueue(metadata.dataServers[j]);
            }

            while (numResults < metadata.writeQuorum)
            {
                Thread.Sleep(1000);
                System.Console.WriteLine("Waiting for quorum");
                for (int i = 0; i < results.Count; i++)
                {
                    if (results[i].IsCompleted && !completed[i])
                    {
                        try
                        {
                            ((WriteDelegate)((AsyncResult)results[i]).AsyncDelegate).EndInvoke(results[i]);
                            results.RemoveAt(i);
                            numResults++;
                            completed[i] = true;
                        }
                        catch (SocketException) 
                        {
                            LocalFilenameInfo dataServerInfo = availableServers.Dequeue();
                            results.Add(writeDelegate.BeginInvoke(getDataServer(metadata, dataServerInfo.location), dataServerInfo.localFilename, fileData, null, null));
                            availableServers.Enqueue(metadata.dataServers[i]);
                        }
                        catch (IOException) 
                        {
                            LocalFilenameInfo dataServerInfo = availableServers.Dequeue();
                            results.Add(writeDelegate.BeginInvoke(getDataServer(metadata, dataServerInfo.location), dataServerInfo.localFilename, fileData, null, null));
                            availableServers.Enqueue(metadata.dataServers[i]);
                        }
                    }
                }
            }

            System.Console.WriteLine("File " + metadata.filename + " written.");
        }

        private IDataServerClient getDataServer(MetadataInfo metadata, int location)
        {
            return (IDataServerClient)Activator.GetObject(
                        typeof(IDataServerClient),
                        "tcp://localhost:" + location + "/DataServer");
        }

        /*
         * Checks if there are enough servers to perform the requested operation.
         * If there aren't, it locks the thread for that object, waiting for the 
         * requested dataservers to come up.
         */
        private MetadataInfo checkMetadata(int fileRegister)
        {
            Object key = fileRegisters[fileRegister];
            lock (key)
            {
                MetadataInfo metadata = fileRegisters[fileRegister];
                if (metadata.dataServers.Count < metadata.numDataServers)
                {
                    System.Console.WriteLine("Locking object " + key.GetHashCode());
                    Monitor.Wait(key);
                }
            }

            return fileRegisters[fileRegister];
        }
    }
}
