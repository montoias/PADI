
namespace CommonTypes
{
    public interface IMetadataServerDataServer
    {
        int getPrimaryMetadataLocation();
        void register(int location);
    }
}
