using System;
using System.IO;
using System.Xml.Serialization;

namespace CommonTypes
{
    public static class Utils
    {
        public static void serializeObject<T>(T myObject, string filename)
        {
            TextWriter tw = new StreamWriter(filename);
            XmlSerializer x = new XmlSerializer(myObject.GetType());
            x.Serialize(tw, myObject);
            tw.Close();
        }

        public static T deserializeObject<T>(string filename)
        {
            TextReader tr = new StreamReader(filename);
            Type type = typeof(T);
            XmlSerializer x = new XmlSerializer(type);

            T myObject = (T)x.Deserialize(tr);
            tr.Close();

            return myObject;
        }

        public static string byteArrayToString(byte[] b)
        {
            char[] chars = new char[b.Length / sizeof(char)];
            System.Buffer.BlockCopy(b, 0, chars, 0, b.Length);
            return new string(chars);
        }

        public static byte[] stringToByteArray(string s)
        {
            byte[] bytes = new byte[s.Length * sizeof(char)];
            System.Buffer.BlockCopy(s.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static void createFolderFile(string fileFolder)
        {
            if (Directory.Exists(fileFolder))
                Directory.Delete(fileFolder, true);

            Directory.CreateDirectory(fileFolder);
        }
    }
}
