using Newtonsoft.Json;

namespace SafeModel
{
    public interface IUniqueIdGenerator
    {
        string GetUniqueId();
    }


    public interface IFileIdGenerator
    {
        string GetFileId();
    }
}