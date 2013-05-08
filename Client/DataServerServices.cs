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
        private TimeSpan TIMEOUT = TimeSpan.FromSeconds(10);

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
            MetadataInfo metadata = checkMetadata(fileRegister, "readQuorum");
            FileData fileData = readQuorum(metadata, metadata.readQuorum);

            if (semantics.Equals("monotonic") && fileVersions.ContainsKey(metadata.filename) && (fileVersions[metadata.filename] > fileData.version))
            {
                System.Console.WriteLine("Monotonic read was requested: The file obtained was older than the one read before! Retrying...");
                Thread.Sleep(1000);
                return readOnly(fileRegister, semantics);
            }

            fileVersions[metadata.filename] = fileData.version;
            System.Console.WriteLine("File Read. \r\nVersion " + fileData.version + ": " + Utils.byteArrayToString(fileData.file));
            return fileData;
        }

        private FileData readQuorum(MetadataInfo metadata, int quorumNum)
        {
            ReadDelegate readDelegate = new ReadDelegate(readAsync);
            Dictionary<int, int> versionCounter = new Dictionary<int, int>();
            Queue<KeyValuePair<int, LocalFilenameInfo>> availableServers = new Queue<KeyValuePair<int, LocalFilenameInfo>>();
            List<IAsyncResult> results = new List<IAsyncResult>();
            FileData result = null;
            BitArray completed = new BitArray(metadata.numDataServers);
            int numResults = 0, maxVersion = -1;
            int j;

            for (j = 0; j < quorumNum; j++)
            {
                System.Console.WriteLine("Invoking read on " + metadata.dataServers[j].location);
                results.Add(readDelegate.BeginInvoke(getDataServer(metadata, metadata.dataServers[j].location), metadata.dataServers[j].localFilename, null, null));
            }
            for (; j < metadata.dataServers.Count; j++)
            {
                System.Console.WriteLine("Enqueued server @ " + metadata.dataServers[j].location);
                availableServers.Enqueue(new KeyValuePair<int, LocalFilenameInfo>(j, metadata.dataServers[j]));
            }

            while (numResults < quorumNum)
            {
                System.Console.WriteLine("Waiting for read quorum");
                for (int i = 0; i < results.Count; i++)
                {
                    System.Console.WriteLine("Trying result " + i + " " + completed[i]);
                    try
                    {
                        if (results[i].AsyncWaitHandle.WaitOne(TIMEOUT) && !completed[i])
                        {
                            completed[i] = true;
                            FileData tempFile = ((ReadDelegate)((AsyncResult)results[i]).AsyncDelegate).EndInvoke(results[i]);
                            System.Console.WriteLine("Version:" + tempFile.version);
                            if (maxVersion < tempFile.version)
                            {
                                result = tempFile;
                                maxVersion = tempFile.version;
                            }
                            numResults++;
                        }
                        else
                            throw new TimeoutException();
                    }
                    catch (TimeoutException)
                    {
                        availableServers.Enqueue(new KeyValuePair<int, LocalFilenameInfo>(i, metadata.dataServers[i]));
                        KeyValuePair<int, LocalFilenameInfo> dataServerInfo = availableServers.Dequeue();
                        if (results.Count - 1 < dataServerInfo.Key)
                            results.Add(readDelegate.BeginInvoke(getDataServer(metadata, dataServerInfo.Value.location), dataServerInfo.Value.localFilename, null, null));
                        else
                            results[dataServerInfo.Key] = readDelegate.BeginInvoke(getDataServer(metadata, dataServerInfo.Value.location), dataServerInfo.Value.localFilename, null, null);
                        completed[dataServerInfo.Key] = false;
                    }
                    catch (SocketException)
                    {
                        availableServers.Enqueue(new KeyValuePair<int, LocalFilenameInfo>(i, metadata.dataServers[i]));
                        KeyValuePair<int, LocalFilenameInfo> dataServerInfo = availableServers.Dequeue();
                        if (results.Count - 1 < dataServerInfo.Key)
                            results.Add(readDelegate.BeginInvoke(getDataServer(metadata, dataServerInfo.Value.location), dataServerInfo.Value.localFilename, null, null));
                        else
                            results[dataServerInfo.Key] = readDelegate.BeginInvoke(getDataServer(metadata, dataServerInfo.Value.location), dataServerInfo.Value.localFilename, null, null);

                        completed[i] = true;
                        completed[dataServerInfo.Key] = false;
                    }
                }
            }

            System.Console.WriteLine("Max Version is: " + maxVersion);

            return result;
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

            MetadataInfo metadata = checkMetadata(fileRegister, "writeQuorum");
            System.Console.WriteLine(metadata);

            writeQuorum(metadata, new FileData(Utils.stringToByteArray(textFile), readQuorum(metadata, metadata.writeQuorum).version + 1, clientID, metadata.filename));
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

            MetadataInfo metadata = checkMetadata(fileRegister, "writeQuorum");
            FileData fileData = byteRegisters[byteRegister];
            fileData.version = readQuorum(metadata, metadata.writeQuorum).version + 1;
            fileData.clientID = clientID;
            writeQuorum(metadata, fileData);
        }

        private void writeQuorum(MetadataInfo metadata, FileData fileData)
        {
            WriteDelegate writeDelegate = new WriteDelegate(writeAsync);
            Queue<KeyValuePair<int, LocalFilenameInfo>> availableServers = new Queue<KeyValuePair<int, LocalFilenameInfo>>();
            List<IAsyncResult> results = new List<IAsyncResult>();
            BitArray completed = new BitArray(metadata.numDataServers);
            int numResults = 0;
            int j;

            for (j = 0; j < metadata.writeQuorum; j++)
            {
                System.Console.WriteLine("Invoking write on " + metadata.dataServers[j].location);
                results.Add(writeDelegate.BeginInvoke(getDataServer(metadata, metadata.dataServers[j].location), metadata.dataServers[j].localFilename, fileData, null, null));
            }
            for (; j < metadata.dataServers.Count; j++)
            {
                availableServers.Enqueue(new KeyValuePair<int, LocalFilenameInfo>(j, metadata.dataServers[j]));
            }

            while (numResults < metadata.writeQuorum)
            {
                System.Console.WriteLine("Waiting for write quorum");
                for (int i = 0; i < results.Count; i++)
                {
                    System.Console.WriteLine("Trying result " + i);
                    try
                    {
                        if (results[i].AsyncWaitHandle.WaitOne(TIMEOUT) && !completed[i])
                        {
                            ((WriteDelegate)((AsyncResult)results[i]).AsyncDelegate).EndInvoke(results[i]);
                            results.RemoveAt(i);
                            numResults++;
                            System.Console.WriteLine("Result " + i + " is completed.");
                            completed[i] = true;
                        }
                        else
                            throw new TimeoutException();

                    }
                    catch (TimeoutException)
                    {
                        availableServers.Enqueue(new KeyValuePair<int, LocalFilenameInfo>(i, metadata.dataServers[i]));
                        KeyValuePair<int, LocalFilenameInfo> dataServerInfo = availableServers.Dequeue();
                        if (results.Count - 1 < dataServerInfo.Key)
                            results.Add(writeDelegate.BeginInvoke(getDataServer(metadata, dataServerInfo.Value.location),
                                                        dataServerInfo.Value.localFilename, fileData, null, null));
                        else
                            results[dataServerInfo.Key] = writeDelegate.BeginInvoke(getDataServer(metadata, dataServerInfo.Value.location),
                                                        dataServerInfo.Value.localFilename, fileData, null, null);
                        completed[dataServerInfo.Key] = false;
                    }
                    catch (SocketException)
                    {
                        availableServers.Enqueue(new KeyValuePair<int, LocalFilenameInfo>(i, metadata.dataServers[i]));
                        KeyValuePair<int, LocalFilenameInfo> dataServerInfo = availableServers.Dequeue();
                        if (results.Count - 1 < dataServerInfo.Key)
                            results.Add(writeDelegate.BeginInvoke(getDataServer(metadata, dataServerInfo.Value.location),
                                                            dataServerInfo.Value.localFilename, fileData, null, null));
                        else
                            results[dataServerInfo.Key] = writeDelegate.BeginInvoke(getDataServer(metadata, dataServerInfo.Value.location),
                                                        dataServerInfo.Value.localFilename, fileData, null, null);

                        completed[i] = true;
                        completed[dataServerInfo.Key] = false;
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
        private MetadataInfo checkMetadata(int fileRegister, string type)
        {
            Object key = fileRegisters[fileRegister];
            lock (key)
            {
                MetadataInfo metadata = fileRegisters[fileRegister];
                int numQuorum = type.Equals("readQuorum") ? metadata.readQuorum : metadata.writeQuorum;
                if (metadata.dataServers.Count < numQuorum)
                {
                    Monitor.Wait(key);
                }
            }

            return fileRegisters[fileRegister];
        }
    }
}
