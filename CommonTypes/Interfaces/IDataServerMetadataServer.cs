
namespace CommonTypes
{
    public interface IDataServerMetadataServer
    {
        DataServerStats getStats();
        void restartStats();
        void create(string localFilename, byte[] file, int version, int clientID, string filename);
        FileData read(string localFilename);
        void write(string localFilename, FileData file);

    }
}
