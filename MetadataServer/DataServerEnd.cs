using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTypes;
using System.Collections;
using System.IO;

namespace MetadataServer
{
    public partial class MetadataServer : MarshalByRefObject, IMetadataServerClient, IMetadataServerPuppet, IMetadataServerDataServer, IMetadataServer
    {
        public void register(int location)
        {
            isAlive();

            System.Console.WriteLine("Registering new data server at tcp://localhost:" + location + "/DataServer");
            string instruction = "REGISTER" + "," + location;
            sendInstruction(instruction);
            log.Add(instruction);

            dataServersList.Add(location, (IDataServerMetadataServer)Activator.GetObject(
               typeof(IDataServerMetadataServer),
               "tcp://localhost:" + location + "/DataServer"));

            processMetadataQueue(location);
        }

        private void processMetadataQueue(int location)
        {
            string pair = location + "," + generateLocalFileName();
            List<string> fileList = new List<string>(queueMetadata.Keys);
            foreach (string filename in fileList)
            {
                System.Console.WriteLine("processing files in the queue: " + filename);

                string path = Path.Combine(fileFolder, filename);
                MetadataInfo metadata = Utils.deserializeObject<MetadataInfo>(path);
                metadata.dataServers.Add(pair);
                Utils.serializeObject<MetadataInfo>(metadata, path);
                metadataTable[filename] = metadata;


                if (queueMetadata[filename].numDataServers == dataServersList.Count)
                {
                    queueMetadata.Remove(filename);
                    if (openedFiles.ContainsKey(filename))
                    {
                        List<int> clientIDs = openedFiles[filename];

                        foreach (int clientID in clientIDs)
                        {
                            int loc = clientID + 8000;
                            System.Console.WriteLine("updating client at port :" + loc);
                            IClientMetadataServer client = (IClientMetadataServer)Activator.GetObject(
                                   typeof(IClientMetadataServer), "tcp://localhost:" + loc + "/Client");

                            client.updateMetadata(filename, metadata);
                        }
                    }
                }

            }
        }
    }
}
