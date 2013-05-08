using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTypes;
using System.IO;
using System.Collections;

namespace MetadataServer
{
    public partial class MetadataServer
    {
        public MetadataInfo create(string filename, int numDataServers, int readQuorum, int writeQuorum)
        {
            isAlive();

            string path = Path.Combine(fileFolder, filename);
            MetadataInfo metadata;

            if (!metadataTable.ContainsKey(filename) && !File.Exists(path))
            {
                System.Console.WriteLine("Creating file: " + filename);

                int tempNumServers = (numDataServers > metadataState.dataServersList.Count) ? metadataState.dataServersList.Count : numDataServers;
                List<LocalFilenameInfo> locations = new List<LocalFilenameInfo>();
                List<KeyValuePair<int, DataServerStats>> sortedServers = sortedServers = (from entry in dataServerLoad
                                                                               orderby entry.Value.serverLoad ascending
                                                                               select entry).ToList();

                for (int i = 0; i < tempNumServers; i++)
                {
                    string localFilename = generateLocalFileName();
                    locations.Add(new LocalFilenameInfo (sortedServers[i].Key, localFilename));
                    getDataServer(sortedServers[i].Key).create(localFilename, Utils.stringToByteArray(""), 0, -1, filename);
                }

                metadata = new MetadataInfo(filename, numDataServers, readQuorum, writeQuorum, locations);
                CreateDTO instruction = new CreateDTO(metadata);

                sendInstruction((InstructionDTO) instruction);
                metadataState.log.Add(instruction);
                metadataState.currentInstruction++;

                if (numDataServers > metadataState.dataServersList.Count)
                {
                    System.Console.WriteLine("Adding to data server queue: " + filename);
                    metadataState.queueFiles.Add(filename, metadata);
                }

                metadataTable.Add(filename, metadata);
                Utils.serializeObject<MetadataInfo>(metadata, path);
                object key = stateFile;
                lock (key)
                {
                    Utils.serializeObject<MetadataServerState>(metadataState, stateFile);
                }

                return metadata;
            }
            else
            {
                System.Console.WriteLine("File already exists:" + filename);
                throw new FileAlreadyExistsException();
            }
        }

        public MetadataInfo create(string filename, int numDataServers, int readQuorum, int writeQuorum, List<LocalFilenameInfo> locations)
        {
            isAlive();

            string path = Path.Combine(fileFolder, filename);
            MetadataInfo metadata;

            if (!metadataTable.ContainsKey(filename) && !File.Exists(path))
            {
                System.Console.WriteLine("Creating file: " + filename);

                metadata = new MetadataInfo(filename, numDataServers, readQuorum, writeQuorum, locations);
                CreateDTO instruction = new CreateDTO(metadata);

                sendInstruction(instruction);
                metadataState.log.Add(instruction);
                metadataState.currentInstruction++;

                if (numDataServers > metadataState.dataServersList.Count)
                {
                    System.Console.WriteLine("Adding to data server queue: " + filename);
                    metadataState.queueFiles.Add(filename, metadata);
                }

                metadataTable.Add(filename, metadata);
                Utils.serializeObject<MetadataInfo>(metadata, path);
                object key = stateFile;
                lock (key)
                {
                    Utils.serializeObject<MetadataServerState>(metadataState, stateFile);
                }

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
            DeleteDTO instruction = new DeleteDTO(filename);
            sendInstruction(instruction);
            metadataState.log.Add(instruction);
            metadataState.currentInstruction++;

            if (File.Exists(path))
            {
                System.Console.WriteLine("Deleting file:" + filename);
                metadataTable.Remove(filename);

                if (metadataState.openedFiles.ContainsKey(filename))
                    metadataState.openedFiles.Remove(filename);

                if (metadataState.queueFiles.ContainsKey(filename))
                    metadataState.queueFiles.Remove(filename);

                object key = stateFile;
                lock (key)
                {
                    Utils.serializeObject<MetadataServerState>(metadataState, stateFile);
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

            if (metadataState.openedFiles.ContainsKey(filename) && metadataState.openedFiles[filename].Contains(location))
                throw new FileAlreadyOpenedException();

            OpenDTO instruction = new OpenDTO(filename, location);
            sendInstruction(instruction);

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

            if (!metadataState.openedFiles.ContainsKey(filename))
            {
                List<int> clientList = new List<int>();
                clientList.Add(location);
                metadataState.openedFiles.Add(filename, clientList);
            }
            else
                metadataState.openedFiles[filename].Add(location);

            object key = stateFile;
            lock (key)
            {
                Utils.serializeObject<MetadataServerState>(metadataState, stateFile);
            }
            return metadata;
        }

        public void close(string filename, int location)
        {
            isAlive();

            CloseDTO instruction = new CloseDTO(filename, location);
            sendInstruction(instruction);

            if (!metadataState.openedFiles.ContainsKey(filename) || !metadataState.openedFiles[filename].Contains(location))
                throw new FileNotOpenedException();

            if (metadataState.openedFiles[filename].Count == 1)
                metadataState.openedFiles.Remove(filename);
            else
                metadataState.openedFiles[filename].Remove(location);

            object key = stateFile;
            lock (key)
            {
                Utils.serializeObject<MetadataServerState>(metadataState, stateFile);
            }
        }
    }
}
