using System;
using CommonTypes;
using System.Threading;
using System.IO;
using System.Net.Sockets;

namespace DataServer
{
    public partial class DataServer
    {
        private DataServerStats dataServerStats = new DataServerStats(port);
        private const int WRITE_LOAD = 10;
        private const int READ_LOAD = 1;

        public FileData read(string localFilename)
        {
            lock (this)
            {
                if (isFrozen)
                    Monitor.Wait(this);
                else
                    checkFailure();

                System.Console.WriteLine("Opening file:" + localFilename);
                string path = Path.Combine(fileFolder, localFilename);
                FileData fileData;

                if (File.Exists(path))
                {
                    fileData = Utils.deserializeObject<FileData>(path);
                    dataServerStats.filesAccessed[localFilename] = fileData.filename;

                    dataServerStats.serverLoad += READ_LOAD;

                    if (dataServerStats.fileLoad.ContainsKey(localFilename))
                        dataServerStats.fileLoad[localFilename] += READ_LOAD;
                    else
                        dataServerStats.fileLoad[localFilename] = READ_LOAD;
                }
                else
                {
                    fileData = new FileData(Utils.stringToByteArray(""), 0, -1, "");
                    Utils.serializeObject<FileData>(fileData, path);
                }

                System.Console.WriteLine("Returning from read.");
                return fileData;
            }
        }

        public void write(string localFilename, FileData fileData)
        {
            lock (this)
            {
                if (isFrozen)
                    Monitor.Wait(this);
                else
                    checkFailure();

                System.Console.WriteLine("Writing the file:" + localFilename);
                string path = Path.Combine(fileFolder, localFilename);
                FileData prev = read(localFilename);

                dataServerStats.filesAccessed[localFilename] = fileData.filename;

                if ((fileData.version == prev.version && fileData.clientID < prev.clientID) || fileData.version > prev.version)
                    Utils.serializeObject<FileData>(fileData, path);

                dataServerStats.serverLoad += WRITE_LOAD;

                if (dataServerStats.fileLoad.ContainsKey(localFilename))
                    dataServerStats.fileLoad[localFilename] += WRITE_LOAD;
                else
                    dataServerStats.fileLoad[localFilename] = WRITE_LOAD;
            }
        }

        public DataServerStats getStats()
        {
            checkFailure();
            return dataServerStats;
        }

        public void restartStats()
        {
            dataServerStats.fileLoad.Clear();
            dataServerStats.filesAccessed.Clear();
            dataServerStats.serverLoad = 0;
        }

        public void create(string localFilename, byte[] file, int version, int clientID, string filename)
        {
            string path = Path.Combine(fileFolder, localFilename);
            System.Console.WriteLine("Creating file:" + localFilename);
            FileData fileData = new FileData(file, version, clientID, filename);
            Utils.serializeObject<FileData>(fileData, path);
        }

        public void delete(string localFilename)
        {
            lock (this)
            {
                string path = Path.Combine(fileFolder, localFilename);
                System.Console.WriteLine("Deleting file:" + localFilename);
                File.Delete(path);
            }
        }
    }
}
