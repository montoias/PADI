using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTypes;
using System.Collections;
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
            Utils.serializeObject<MetadataServerState>(metadataState, stateFile);

            if (port.Equals(primaryServerLocation) && metadataState.queueFiles.Count > 0)
            {
                processMetadataQueue(new LocalFilenameInfo(location, generateLocalFileName()));
            }
        }

        private void processMetadataQueue(LocalFilenameInfo dataServerInfo)
        {
            QueueFileDTO instruction = new QueueFileDTO(dataServerInfo);
            sendInstruction(instruction);

            //Used to modify the queue without throwing an exception
            List<string> fileList = new List<string>(metadataState.queueFiles.Keys);

            foreach (string filename in fileList)
            {
                System.Console.WriteLine("processing files in the queue: " + filename);

                string path = Path.Combine(fileFolder, filename);
                MetadataInfo metadata = Utils.deserializeObject<MetadataInfo>(path);
                metadata.dataServers.Add(dataServerInfo);
                Utils.serializeObject<MetadataInfo>(metadata, path);
                metadataTable[filename] = metadata;
                metadataState.queueFiles[filename] = metadata;

                UpdateMetadataDTO update = new UpdateMetadataDTO(metadata);
                metadataState.log.Add(update);
                metadataState.currentInstruction++;

                if (metadataState.queueFiles[filename].numDataServers == metadataState.dataServersList.Count)
                {
                    metadataState.queueFiles.Remove(filename);

                    if (metadataState.openedFiles.ContainsKey(filename))
                    {
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
                Utils.serializeObject<MetadataServerState>(metadataState, stateFile);
            }
        }
    }
}
