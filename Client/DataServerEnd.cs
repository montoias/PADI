using System;
using System.Collections.Generic;
using CommonTypes;
using System.Threading;
using System.Runtime.Remoting.Messaging;

namespace Client
{
    public partial class Client : MarshalByRefObject, IClientPuppet, IClientMetadataServer
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
            ReadDelegate readDelegate = new ReadDelegate(readAsync);
            List<IAsyncResult> results = new List<IAsyncResult>();
            FileData fileData;

            MetadataInfo metadata = checkMetadata(fileRegister);

            foreach (string dsInfo in metadata.dataServers)
            {
                string serverLocation = metadata.getDataServerLocation(dsInfo);
                string localFilename = metadata.getLocalFilename(dsInfo);

                System.Console.WriteLine("Reading from dataServer " + (Convert.ToInt32(serverLocation) - 9000));

                IDataServerClient dataServer = (IDataServerClient)Activator.GetObject(
                typeof(IDataServerClient),
                "tcp://localhost:" + serverLocation + "/DataServer");

                results.Add(readDelegate.BeginInvoke(dataServer, localFilename, null, null));
            }

            fileData = readQuorum(metadata, results, semantics);
            if (fileData == null)
                return null;

            if (semantics.Equals("monotonic") && fileVersions.ContainsKey(metadata.filename) && (fileVersions[metadata.filename] >= fileData.version))
            {
                System.Console.WriteLine("Monotonic read was requested: The file obtained was older than the one read before!");
                return null;
            }

            fileVersions[metadata.filename] = fileData.version;
            System.Console.WriteLine("File Read. \r\nVersion " + fileData.version + ": " + Utils.byteArrayToString(fileData.file));
            return fileData;
        }


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


        private FileData readQuorum(MetadataInfo metadata, List<IAsyncResult> results, string semantics)
        {
            Dictionary<int, FileData> versionResults = new Dictionary<int, FileData>();
            Dictionary<int, int> versionCounter = new Dictionary<int, int>();
            int numResults = 0;
            int maxResults = 0;
            int maxVersion = 0;

            while (numResults < metadata.writeQuorum && results.Count > 0)
            {
                Thread.Sleep(1000);
                System.Console.WriteLine("Waiting for quorum");
                for (int i = results.Count - 1; i > -1; i--)
                {
                    if (results[i].IsCompleted)
                    {
                        try
                        {
                            FileData tempFile = ((ReadDelegate)((AsyncResult)results[i]).AsyncDelegate).EndInvoke(results[i]);
                            if (versionCounter.ContainsKey(tempFile.version))
                                versionCounter[tempFile.version]++;
                            else
                            {
                                versionResults.Add(tempFile.version, tempFile);
                                versionCounter.Add(tempFile.version, 1);
                            }
                            numResults++;
                        }
                        catch (Exception)
                        {
                            //Possibly send request to next available server
                        }
                        results.RemoveAt(i);
                    }
                }
            }

            if (numResults < metadata.writeQuorum) //TODO: Print error message -> No servers available (retry?)
            {
                System.Console.WriteLine("Error in writeQuorum: not enough results");
                return null;
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



        public void write(int fileRegister, string textFile)
        {
            System.Console.WriteLine("Writing to DS where metadata is from file register " + fileRegister);

            MetadataInfo metadata = checkMetadata(fileRegister);
            WriteDelegate writeDelegate = new WriteDelegate(writeAsync);
            List<IAsyncResult> results = new List<IAsyncResult>();

            foreach (string dsInfo in metadata.dataServers)
            {
                string serverLocation = metadata.getDataServerLocation(dsInfo);
                string localFilename = metadata.getLocalFilename(dsInfo);

                System.Console.WriteLine("Writing to dataServer " + (Convert.ToInt32(serverLocation) - 9000));

                IDataServerClient dataServer = (IDataServerClient)Activator.GetObject(typeof(IDataServerClient),
                    "tcp://localhost:" + serverLocation + "/DataServer");

                //TODO: Obter a versão mais recente -> com quorum de escrita!!!!
                results.Add(writeDelegate.BeginInvoke(dataServer, localFilename, new FileData(Utils.stringToByteArray(textFile), 0, clientID), null, null));
            }

            writeQuorum(metadata, results);
        }

        public void write(int fileRegister, int byteRegister)
        {
            System.Console.WriteLine("Writing to DS where metadata is from file register " + fileRegister);
            System.Console.WriteLine("Contents to write were from byte register " + byteRegister);

            MetadataInfo metadata = checkMetadata(fileRegister);
            FileData fileData = byteRegisters[byteRegister];
            WriteDelegate writeDelegate = new WriteDelegate(writeAsync);
            List<IAsyncResult> results = new List<IAsyncResult>();

            foreach (string dsInfo in metadata.dataServers)
            {
                string serverLocation = metadata.getDataServerLocation(dsInfo);
                string localFilename = metadata.getLocalFilename(dsInfo);

                System.Console.WriteLine("Writing to dataServer " + (Convert.ToInt32(serverLocation) - 9000));

                IDataServerClient dataServer = (IDataServerClient)Activator.GetObject(
                typeof(IDataServerClient),
                "tcp://localhost:" + serverLocation + "/DataServer");

                //TODO: Leitura para obter a versão
                results.Add(writeDelegate.BeginInvoke(dataServer, localFilename, fileData, null, null));
            }

            writeQuorum(metadata, results);
        }

        private void writeQuorum(MetadataInfo metadata, List<IAsyncResult> results)
        {
            int numResults = 0;

            while (numResults < metadata.writeQuorum)
            {
                Thread.Sleep(1000);
                System.Console.WriteLine("Waiting for quorum");
                for (int i = results.Count - 1; i > -1; i--)
                {
                    if (results[i].IsCompleted)
                    {
                        try
                        {
                            ((WriteDelegate)((AsyncResult)results[i]).AsyncDelegate).EndInvoke(results[i]);
                            results.RemoveAt(i);
                            numResults++;
                        }
                        catch (Exception) //Specify exception
                        {
                            //Possibly send request to next available server
                        }
                    }
                }

                if (numResults < metadata.writeQuorum) //TODO: Print error message -> No servers available (retry?)
                    System.Console.WriteLine("Error in writeQuorum: not enough results");
            }

            System.Console.WriteLine("File " + metadata.filename + " written.");
        }
    }
}
