﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTypes;
using System.Threading;
using System.IO;

namespace DataServer
{
    public partial class DataServer : MarshalByRefObject, IDataServerClient, IDataServerPuppet, IDataServerMetadataServer
    {
        public FileData read(string filename)
        {
            lock (this)
            {
                if (isFrozen)
                    Monitor.Wait(this);
                else
                    checkFailure();

                string path = Path.Combine(fileFolder, filename);

                if (File.Exists(path))
                {
                    System.Console.WriteLine("Opening file:" + filename);
                    return Utils.deserializeObject<FileData>(path);
                }
                else
                {
                    FileData f = new FileData(Utils.stringToByteArray(""), 0, 0);
                    Utils.serializeObject<FileData>(f, path);
                    return f;
                }
            }
        }

        public void write(string filename, FileData f)
        {
            lock (this)
            {
                if (isFrozen)
                    Monitor.Wait(this);
                else
                    checkFailure();

                System.Console.WriteLine("Writing the file:" + filename);
                string path = Path.Combine(fileFolder, filename);
                FileData prev = read(filename);

                if( f.version > prev.version)
                    Utils.serializeObject<FileData>(f, path);
                else if (f.version == prev.version)
                {
                    if(f.clientID < prev.clientID)
                        Utils.serializeObject<FileData>(f, path);
                }
                
            }
        }
    }
}
