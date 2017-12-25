using Newtonsoft.Json;

namespace SafeModel
{
    public interface IRecordIdGenerator
    {
        string GetRecordId();
    }


    public interface IFileIdGenerator
    {
        string GetFileId();
    }
}