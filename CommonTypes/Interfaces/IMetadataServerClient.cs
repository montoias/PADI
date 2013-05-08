
namespace CommonTypes
{
    public interface IMetadataServerClient
    {
        MetadataInfo open(string filename, int location);
        void close(string filename, int location);
        MetadataInfo create(string filename, int numDataServers, int readQuorum, int writeQuorum);
        void delete(string filename);
        int getPrimaryMetadataLocation();
    }
}
