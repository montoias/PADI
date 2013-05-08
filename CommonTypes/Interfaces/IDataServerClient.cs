
namespace CommonTypes
{
    public interface IDataServerClient
    {
        FileData read(string filename);
        void write(string filename, FileData file);
    }
}
