using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTypes;
using System.IO;
using System.Collections;

namespace MetadataServer
{
    public partial class MetadataServer : MarshalByRefObject, IMetadataServerClient, IMetadataServerPuppet, IMetadataServerDataServer, IMetadataServer
    {
        public MetadataInfo create(string filename, int numDataServers, int readQuorum, int writeQuorum)
        {
            isAlive();

            string path = Path.Combine(fileFolder, filename);
            MetadataInfo metadata;


            if (!metadataTable.ContainsKey(filename) && !File.Exists(path))
            {
                System.Console.WriteLine("Creating file: " + filename);

                int tempNumServers = (numDataServers > dataServersList.Count) ? dataServersList.Count : numDataServers;
                List<int> dataServersArray = new List<int>(dataServersList.Keys);
                List<string> locations = new List<string>();

                //TODO: Distribution algorithm
                for (int i = 0; i < tempNumServers; i++)
                {
                    locations.Add(dataServersArray[i] + "," + generateLocalFileName());
                }

                metadata = new MetadataInfo(filename, numDataServers, readQuorum, writeQuorum, locations);
                string instruction = "CREATE" + "," + filename + "," + numDataServers + "," + readQuorum + "," + writeQuorum + ",";
                foreach (string location in locations)
                    instruction += location + "\n";
                sendInstruction(instruction);
                log.Add(instruction);

                if (numDataServers > dataServersList.Count)
                {
                    System.Console.WriteLine("Adding to data server queue: " + filename);
                    queueMetadata.Add(filename, metadata);
                }

                metadataTable.Add(filename, metadata);
                Utils.serializeObject<MetadataInfo>(metadata, path);

                return metadata;
            }
            else
            {
                System.Console.WriteLine("File already exists:" + filename);
                throw new FileAlreadyExistsException();
            }
        }

        public MetadataInfo create(string filename, int numDataServers, int readQuorum, int writeQuorum, List<string> locations)
        {
            isAlive();

            string path = Path.Combine(fileFolder, filename);
            MetadataInfo metadata;
            string instruction = "CREATE" + "," + filename + "," + numDataServers + "," + readQuorum + "," + writeQuorum + ",";
            foreach (string location in locations)
                instruction += location + "\n";

            sendInstruction(instruction);
            log.Add(instruction);

            if (!metadataTable.ContainsKey(filename) && !File.Exists(path))
            {
                System.Console.WriteLine("Creating file: " + filename);

                int tempNumServers = (numDataServers > dataServersList.Count) ? dataServersList.Count : numDataServers;
                ArrayList dataServersArray = new ArrayList(dataServersList.Keys);

                metadata = new MetadataInfo(filename, numDataServers, readQuorum, writeQuorum, locations);

                if (numDataServers > dataServersList.Count)
                {
                    System.Console.WriteLine("Adding to data server queue: " + filename);
                    queueMetadata.Add(filename, metadata);
                }

                metadataTable.Add(filename, metadata);
                Utils.serializeObject<MetadataInfo>(metadata, path);

                return metadata;
            }
            else
            {
                System.Console.WriteLine("File already exists:" + filename);
                throw new FileAlreadyExistsException();
            }
        }

        public void delete(string filename)
        {
            isAlive();

            string path = Path.Combine(fileFolder, filename);
            string instruction = "DELETE" + "," + filename;
            sendInstruction(instruction);
            log.Add(instruction);

            if (File.Exists(path))
            {
                System.Console.WriteLine("Deleting file:" + filename);
                metadataTable.Remove(filename);
                if (openedFiles.ContainsKey(filename))
                    foreach (int clientID in openedFiles[filename])
                    {
                        int loc = clientID + 8000;
                        System.Console.WriteLine("updating client at port :" + loc);
                        IClientMetadataServer client = (IClientMetadataServer)Activator.GetObject(
                               typeof(IClientMetadataServer), "tcp://localhost:" + loc + "/Client");
                    }

                File.Delete(path);

            }
            else
            {
                throw new FileDoesNotExistException();
            }
        }

        public MetadataInfo open(string filename, int location)
        {
            isAlive();

            string path = Path.Combine(fileFolder, filename);
            MetadataInfo metadata;
            string instruction = "OPEN" + "," + filename + "," + location;

            if (openedFiles.ContainsKey(filename) && openedFiles[filename].Contains(location))
                throw new FileAlreadyOpenedException();

            sendInstruction(instruction);
            log.Add(instruction);

            if (metadataTable.ContainsKey(filename))
            {
                System.Console.WriteLine("File in cache:" + filename);
                metadata = metadataTable[filename];
            }
            else
            {
                if (File.Exists(path))
                {
                    System.Console.WriteLine("Opening file from disk:" + filename);
                    metadata = Utils.deserializeObject<MetadataInfo>(path);
                    metadataTable.Add(filename, metadata);
                }

                else
                {
                    System.Console.WriteLine("File doesn't exist:" + filename);
                    throw new FileDoesNotExistException();
                }
            }

            if (!openedFiles.ContainsKey(filename))
            {
                List<int> clientList = new List<int>();
                clientList.Add(location);
                openedFiles.Add(filename, clientList);
            }
            else
                openedFiles[filename].Add(location);

            return metadata;
        }

        public void close(string filename, int location)
        {
            isAlive();

            string instruction = "CLOSE" + "," + filename + "," + location;
            sendInstruction(instruction);
            log.Add(instruction);

            if (!openedFiles.ContainsKey(filename) || !openedFiles[filename].Contains(location))
                throw new FileNotOpenedException();

            if (openedFiles[filename].Count == 1)
                openedFiles.Remove(filename);
        }
    }
}
