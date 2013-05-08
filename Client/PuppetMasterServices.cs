using System;
using System.Text.RegularExpressions;
using CommonTypes;

namespace Client
{
    public partial class Client
    {

        public void copy(int fileRegister1, string semantics, int fileRegister2, string salt)
        {
            System.Console.WriteLine(fileRegister1 + " to " + fileRegister2 + " (" + semantics + ") : " + salt);
            FileData fileData = readOnly(fileRegister1, semantics);
            write(fileRegister2, Utils.byteArrayToString(fileData.file) + salt);
        }

        public void exescript(string[] scriptInstructions)
        {
            foreach (string instruction in scriptInstructions)
                if (!instruction[0].Equals('#'))
                    interpretInstruction(instruction);
        }

        private void interpretInstruction(string command)
        {
            string[] parameters = command.Split(',');
            string[] processInst = parameters[0].Split(' ');
            string instruction = processInst[0];
            string[] processInfo = processInst[1].Split('-');
            int processNumber = Convert.ToInt32(processInfo[1]) - 1;
            string textFile = "";

            //Removing whitespaces
            for (int i = 0; i < parameters.Length; i++)
            {
                if (i == 2)
                {
                    textFile = parameters[2].Replace("\"", "");
                }

                parameters[i] = parameters[i].Replace(" ", "");
            }

            switch (instruction)
            {
                case "WRITE":
                    try
                    {
                        int registerIndex = Convert.ToInt32(parameters[2]);
                        write(Convert.ToInt32(parameters[1]), registerIndex);
                    }
                    catch (FormatException)
                    {
                        write(Convert.ToInt32(parameters[1]), textFile);
                    }

                    break;
                case "READ":
                    read(Convert.ToInt32(parameters[1]), parameters[2], Convert.ToInt32(parameters[3]));
                    break;
                case "OPEN":
                    open(parameters[1]);
                    break;
                case "CLOSE":
                    close(parameters[1]);
                    break;
                case "CREATE":
                    create(parameters[1], Convert.ToInt32(parameters[2]), Convert.ToInt32(parameters[3]), Convert.ToInt32(parameters[4]));
                    break;
                case "DELETE":
                    delete(parameters[1]);
                    break;
                case "COPY":
                    string salt = parameters[4].Replace("\"", "");
                    copy(Convert.ToInt32(parameters[1]), parameters[2], Convert.ToInt32(parameters[3]), salt);
                    break;
                case "DUMP":
                    dump();
                    break;
            }
        }
    }
}
