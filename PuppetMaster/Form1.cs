using System;
using System.Windows.Forms;

namespace PuppetMaster
{
    public partial class Form1 : Form
    {
        private PuppetMaster puppetMaster;
        private int clientCounter = 0;
        private int dataServerCounter = 0;
        private int metadataServerCounter = 0;

        private const int NUM_REGISTERS = 10;
        private const int MAX_ITEMS = 10;

        /*
         * Class constructor. Passes a puppetMaster as an argument, which is the one used
         * to execute the user input/commands received in the form.
         */
        public Form1(PuppetMaster puppetMaster)
        {
            this.puppetMaster = puppetMaster;
            InitializeComponent();

            for (int i = 0; i < NUM_REGISTERS; i++)
            {
                FileRegisterBox.Items.Add(i);
                ByteRegisterBox.Items.Add(i);
            }

            for (int i = 0; i < MAX_ITEMS; i++)
            {
                WriteQuorumBox.Items.Add(i);
                ReadQuorumBox.Items.Add(i);
                NServersBox.Items.Add(i);
            }

            WriteQuorumBox.SelectedIndex = 0;
            ReadQuorumBox.SelectedIndex = 0;
            NServersBox.SelectedIndex = 0;
            FileRegisterBox.SelectedIndex = 0;
            ByteRegisterBox.SelectedIndex = 0;
            WriteModeBox.SelectedIndex = 0;
            SemanticsBox.SelectedIndex = 0;
        }


        /*********************************
         ********* PUPPET MASTER *********
         *********************************/

        private void LaunchClientButton_Click(object sender, EventArgs e)
        {
            puppetMaster.launchClient();
        }

        public void addClient()
        {
            ClientBox.Items.Add("c-" + clientCounter);
            ClientBox.SelectedIndex = clientCounter++;
        }

        private void LaunchMetadataButton_Click(object sender, EventArgs e)
        {
            if (metadataServerCounter < 3)
            {
                puppetMaster.launchMetadata(metadataServerCounter);
            }
        }

        public void addMetadataServer()
        {
            if (metadataServerCounter < 3)
            {
                MetadataBox.Items.Add("m-" + metadataServerCounter);
                MetadataBox.SelectedIndex = metadataServerCounter++;
            }
        }

        private void LaunchDataServerButton_Click(object sender, EventArgs e)
        {
            puppetMaster.launchDataServer();
        }

        public void addDataServer()
        {
            DataServerBox.Items.Add("d-" + dataServerCounter);
            DataServerBox.SelectedIndex = dataServerCounter++;
        }

        private void CreateFileButton_Click(object sender, EventArgs e)
        {
            puppetMaster.createFile(FilenameBox.Text, NServersBox.SelectedIndex,
                ReadQuorumBox.SelectedIndex, WriteQuorumBox.SelectedIndex, ClientBox.SelectedIndex);
        }

        private void DeleteFileButton_Click(object sender, EventArgs e)
        {
            puppetMaster.deleteFile(FilenameBox.Text, ClientBox.SelectedIndex);
        }

        private void OpenFileButton_Click(object sender, EventArgs e)
        {
            puppetMaster.openFile(FilenameBox.Text, ClientBox.SelectedIndex);
        }

        private void CloseFileButton_Click(object sender, EventArgs e)
        {
            puppetMaster.closeFile(FilenameBox.Text, ClientBox.SelectedIndex);
        }

        private void ReadFileButton_Click(object sender, EventArgs e)
        {
            puppetMaster.readFile(FileRegisterBox.SelectedIndex, SemanticsBox.SelectedText, FileRegisterBox.SelectedIndex, ClientBox.SelectedIndex);
        }

        private void WriteFileButton_Click(object sender, EventArgs e)
        {
            if (WriteModeBox.SelectedIndex == 0)
                puppetMaster.writeFile(FileRegisterBox.SelectedIndex, FileTextbox.Text, ClientBox.SelectedIndex);
            else
                puppetMaster.writeFile(FileRegisterBox.SelectedIndex, ByteRegisterBox.SelectedIndex, ClientBox.SelectedIndex);
        }

        private void DumpClientButton_Click(object sender, EventArgs e)
        {
            puppetMaster.dumpClient(ClientBox.SelectedIndex);
        }

        private void FailMetadataButton_Click(object sender, EventArgs e)
        {
            puppetMaster.failMetadata(MetadataBox.SelectedIndex);
        }

        private void RecoverMetadataServerButton_Click(object sender, EventArgs e)
        {
            puppetMaster.recoverMetadata(MetadataBox.SelectedIndex);
        }

        private void DumpMetadataButton_Click(object sender, EventArgs e)
        {
            puppetMaster.dumpMetadataServer(MetadataBox.SelectedIndex);
        }

        private void KillProcessesButton_Click(object sender, EventArgs e)
        {
            clientCounter = 0;
            dataServerCounter = 0;
            metadataServerCounter = 0;
            ClientBox.Items.Clear();
            DataServerBox.Items.Clear();
            MetadataBox.Items.Clear();
            puppetMaster.killProcesses();
        }

        private void FailDataServerButton_Click(object sender, EventArgs e)
        {
            puppetMaster.failDataServer(DataServerBox.SelectedIndex);
        }

        private void RecoverDataServerButton_Click(object sender, EventArgs e)
        {
            puppetMaster.recoverDataServer(DataServerBox.SelectedIndex);
        }

        private void FreezeDataServerButton_Click(object sender, EventArgs e)
        {
            puppetMaster.freezeDataServer(DataServerBox.SelectedIndex);
        }

        private void UnfreezeDataServerButton_Click(object sender, EventArgs e)
        {
            puppetMaster.unfreezeDataServer(DataServerBox.SelectedIndex);
        }

        private void DumpDataServerButton_Click(object sender, EventArgs e)
        {
            puppetMaster.dumpDataServer(DataServerBox.SelectedIndex);
        }

        private void LoadScriptButton_Click(object sender, EventArgs e)
        {
            puppetMaster.loadScript(ScriptFileBox.Text);
        }

        private void RunScriptButton_Click(object sender, EventArgs e)
        {
            CurrentInstructionBox.Text = puppetMaster.runScript(RunInstructionLineBox.SelectedIndex).ToString();
        }

        private void NextStepButton_Click(object sender, EventArgs e)
        {
            CurrentInstructionBox.Text = puppetMaster.nextStep().ToString();
        }

        /***********************
         ********* GUI *********
         ***********************/

        public void updateFileText(string msg, int version)
        {
            FileTextbox.Clear();
            FileTextbox.Text += version + "\r\n" + msg;
        }

        public void updateScriptText(string msg, int numInst)
        {
            int i;
            EventBox.Clear();
            EventBox.Text += msg;

            for (i = 0; i < numInst; i++)
                RunInstructionLineBox.Items.Add(i);

            RunInstructionLineBox.SelectedIndex = i-1;
        }
    }
}
