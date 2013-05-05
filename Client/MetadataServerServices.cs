using System;
using System.Collections.Generic;
using CommonTypes;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Client
{
    public partial class Client
    {
        MetadataInfo[] fileRegisters = new MetadataInfo[10];
        FileData[] byteRegisters = new FileData[10];
        private Dictionary<string, int> fileVersions = new Dictionary<string, int>();
        private Dictionary<string, int> fileIndexer = new Dictionary<string, int>();
        private int currentFileRegister = 0;

        public MetadataInfo create(string filename, int numDataServers, int readQuorum, int writeQuorum)
        {
            System.Console.WriteLine("Creating the file:" + filename);
            MetadataInfo info = null;
            try
            {
                info = primaryMetadata.create(filename, numDataServers, readQuorum, writeQuorum);
            }
            catch (FileAlreadyExistsException)
            {
                System.Console.WriteLine("File " + filename + " already exists!");
                return null;
            }
            catch (SocketException)
            {
                return retryCreate(filename, numDataServers, readQuorum, writeQuorum);
            }
            catch (IOException)
            {
                return retryCreate(filename, numDataServers, readQuorum, writeQuorum);
            }

            return info;
        }

        private MetadataInfo retryCreate(string filename, int numDataServers, int readQuorum, int writeQuorum)
        {
            System.Console.WriteLine("Primary metadata was down. Looking for a new one.");
            findPrimaryMetadata();
            return create(filename, numDataServers, readQuorum, writeQuorum);
        }

        public void delete(string filename)
        {
            System.Console.WriteLine("Deleting the file: " + filename);
            try
            {
                primaryMetadata.delete(filename);
            }
            catch (FileDoesNotExistException)
            {
                System.Console.WriteLine("File " + filename + " does not exist!");
            }
            catch (SocketException)
            {
                retryDelete(filename);
            }
            catch (IOException)
            {
                retryDelete(filename);
            }
        }

        private void retryDelete(string filename)
        {
            System.Console.WriteLine("Primary metadata was down. Looking for a new one.");
            findPrimaryMetadata();
            delete(filename);
        }

        public MetadataInfo open(string filename)
        {
            System.Console.WriteLine("Opening the file: " + filename);
            try
            {
                MetadataInfo info = primaryMetadata.open(filename, clientID);
                removeByValue(currentFileRegister);
                fileIndexer[filename] = currentFileRegister;
                fileRegisters[(currentFileRegister++) % 10] = info;
                return info;
            }
            catch (FileAlreadyOpenedException)
            {
                System.Console.WriteLine("The file " + filename + " is already open!");
                return null;
            }
            catch (FileDoesNotExistException)
            {
                System.Console.WriteLine("The file " + filename + " does not exist!");
                return null;
            }
            catch (SocketException)
            {
                return retryOpen(filename);
            }
            catch (IOException)
            {
                return retryOpen(filename);
            }
        }

        private MetadataInfo retryOpen(string filename)
        {
            System.Console.WriteLine("Primary metadata was down. Looking for a new one.");
            findPrimaryMetadata();
            return open(filename);
        }

        public void close(string filename)
        {
            System.Console.WriteLine("Closing the file: " + filename);
            try
            {
                primaryMetadata.close(filename, clientID);

                //It may happen that the currentRegister has turn around, so
                //we need to perform this verification.
                if (fileIndexer.ContainsKey(filename))
                {
                    fileRegisters[fileIndexer[filename]] = null;
                    fileIndexer.Remove(filename);
                }
            }
            catch (FileNotOpenedException)
            {
                System.Console.WriteLine("The file " + filename + " wasn't open!");
            }
            catch (FileDoesNotExistException)
            {
                System.Console.WriteLine("The file " + filename + " does not exist!");
            }
            catch (SocketException)
            {
                retryClose(filename);
            }
            catch (IOException)
            {
                retryClose(filename);
            }
        }

        private void retryClose(string filename)
        {
            System.Console.WriteLine("Primary metadata was down. Looking for a new one.");
            findPrimaryMetadata();
            close(filename);
        }

        /* 
         * Removes the unused mapping of filename to index, when the index
         * goes round and begins at zero again.
         */
        private void removeByValue(int currentFileRegister)
        {
            string toRemove = null;

            foreach (KeyValuePair<string, int> entry in fileIndexer)
            {
                if (entry.Value.Equals(currentFileRegister))
                {
                    toRemove = entry.Key;
                    break;
                }
            }

            if (toRemove != null)
                fileIndexer.Remove(toRemove);
        }

        /*
         * This function is invoked by the metadata server, when a new data server becomes available 
         * and the client requested for a file that didn't have enough servers.
         */
        public void updateMetadata(string filename, MetadataInfo m)
        {
            if (fileIndexer.ContainsKey(filename))
            {
                Object key = fileRegisters[fileIndexer[filename]];
                System.Console.WriteLine("Received an updated message for: " + filename);
                lock (key)
                {
                    fileRegisters[fileIndexer[filename]] = m;
                    Monitor.PulseAll(key);
                }
            }
        }

    }
}
