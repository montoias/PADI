
namespace CommonTypes
{
    public interface IDataServerPuppet
    {
        void init(int[] metadataList);
        void freeze();
        void unfreeze();
        void fail();
        void recover();
        string dump();
    }
}
