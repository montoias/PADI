using CommonTypes;
using System;
using System.Collections.Generic;
using System.IO;

namespace MetadataServer
{
    public partial class MetadataServer
    {
        public void register(int location)
        {
            isAlive();

            System.Console.WriteLine("Registering new data server at tcp://localhost:" + location + "/DataServer");
            RegisterDTO instruction = new RegisterDTO(location);
            sendInstruction(instruction);

            metadataState.dataServersList.Add(location);
            dataServerLoad.Add(new KeyValuePair<int, DataServerStats>(location, new DataServerStats()));
            Utils.serializeObject<MetadataServerState>(metadataState, stateFile);

            if (port.Equals(primaryServerLocation) && metadataState.queueFiles.Count > 0)
            {
                processQueue(location);
            }
        }



        private void processQueue(int location)
        {

            //Used to modify the queue without throwing an exception
            List<string> fileList = new List<string>(metadataState.queueFiles.Keys);

            foreach (string filename in fileList)
            {
                System.Console.WriteLine("processing files in the queue: " + filename);

                LocalFilenameInfo dataServerInfo = new LocalFilenameInfo(location, generateLocalFileName());

                processMetadataQueue(filename, dataServerInfo);
            }
        }

        private void processMetadataQueue(string filename, LocalFilenameInfo dataServerInfo)
        {
            QueueFileDTO instruction = new QueueFileDTO(filename, dataServerInfo);
            sendInstruction(instruction);

            string path = Path.Combine(fileFolder, filename);
            MetadataInfo metadata = Utils.deserializeObject<MetadataInfo>(path);

            if (!metadata.dataServers.Contains(dataServerInfo))
            {
                metadata.dataServers.Add(dataServerInfo);
                updateMetadata(metadata);

                if (port.Equals(primaryServerLocation))
                    getDataServer(dataServerInfo.location).create(dataServerInfo.localFilename, Utils.stringToByteArray(""), 0, -1, filename);

                if (metadataState.queueFiles[filename].numDataServers == metadataState.dataServersList.Count)
                {
                    metadataState.queueFiles.Remove(filename);

                    if (port.Equals(primaryServerLocation) && metadataState.openedFiles.ContainsKey(filename))
                            notifyClients(metadata);
                }
                Utils.serializeObject<MetadataServerState>(metadataState, stateFile);
            }
        }

        private void notifyClients(MetadataInfo metadata)
        {
            string filename = metadata.filename;

            foreach (int clientID in metadataState.openedFiles[filename])
            {
                int clientLocation = clientID + 8000;
                System.Console.WriteLine("updating client at port :" + clientLocation);
                IClientMetadataServer client = (IClientMetadataServer)Activator.GetObject(
                       typeof(IClientMetadataServer), "tcp://localhost:" + clientLocation + "/Client");

                client.updateMetadata(filename, metadata);
            }
        }
    }
}
