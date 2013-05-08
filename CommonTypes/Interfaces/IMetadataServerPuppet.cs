
namespace CommonTypes
{
    public interface IMetadataServerPuppet
    {
        void init(int[] metadataList);
        void fail();
        void recover();
        string dump();
    }
}
