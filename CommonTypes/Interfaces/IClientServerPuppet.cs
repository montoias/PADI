
namespace CommonTypes
{
    public interface IClientPuppet
    {
        void init(int[] metadataList);
        MetadataInfo create(string filename, int numDataServers, int readQuorum, int writeQuorum);
        MetadataInfo open(string filename);
        void close(string filename);
        void delete(string filename);
        FileData read(int fileRegister, string semantics, int byteRegister);
        void write(int fileRegister, string file);
        void write(int fileRegister, int byteRegister);
        void copy(int fileRegister1, string semantics, int fileRegister2, string salt);
        string dump();
        void exescript(string[] filename);
    }
}
