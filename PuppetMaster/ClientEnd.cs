using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTypes;
using System.Runtime.Remoting.Messaging;
using System.IO;

namespace PuppetMaster
{
    public partial class PuppetMaster
    {
        public delegate void ExescriptDelegate(int selectedClient, string filename);
        List<IClientPuppet> clientsList = new List<IClientPuppet>();

        public void openFile(string filename, int selectedClient)
        {
            clientsList[selectedClient].open(filename);
        }

        public void closeFile(string filename, int selectedClient)
        {
            clientsList[selectedClient].close(filename);
        }

        public void createFile(string filename, int nServers, int readQuorum, int writeQuorum, int selectedClient)
        {
            clientsList[selectedClient].create(filename, nServers, readQuorum, writeQuorum);
        }

        public void deleteFile(string filename, int selectedClient)
        {
            clientsList[selectedClient].delete(filename);
        }

        public void writeFile(int fileRegister, string file, int selectedClient)
        {
            clientsList[selectedClient].write(fileRegister, file);
        }

        public void writeFile(int fileRegister, int byteRegister, int selectedClient)
        {
            clientsList[selectedClient].write(fileRegister, byteRegister);
        }

        public FileData readFile(int fileRegister, string semantics, int byteRegister, int selectedClient)
        {
            FileData fileData = clientsList[selectedClient].read(fileRegister, semantics, byteRegister);

            return fileData;
        }

        public void copy(int selectedClient, int fileRegister1, string semantics, int fileRegister2, string salt)
        {
            clientsList[selectedClient].copy(fileRegister1, semantics, fileRegister2, salt);
        }

        public void dumpClient(int selectedClient)
        {
            clientsList[selectedClient].dump();
        }

        public void executeExescript(int processNumber, string filename)
        {
            ExescriptDelegate exescriptDelegate = new ExescriptDelegate(exescriptAsync);
            AsyncCallback exescriptCallback = new AsyncCallback(exescriptAsyncCallBack);
            exescriptDelegate.BeginInvoke(processNumber, filename, exescriptCallback, null);
        }

        private void exescriptAsync(int selectedClient, string filename)
        {
            string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, filename);
            string[] fileText = File.ReadAllLines(path);
            clientsList[selectedClient].exescript(fileText);
        }

        private void exescriptAsyncCallBack(IAsyncResult ar)
        {
            ExescriptDelegate exescriptDelegate = (ExescriptDelegate)((AsyncResult)ar).AsyncDelegate;
            exescriptDelegate.EndInvoke(ar);
        }
    }
}
