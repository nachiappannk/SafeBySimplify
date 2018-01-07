using System.Collections.Generic;

namespace SafeModel.Standard
{
    public interface IDataGateway
    {
        byte[] GetBytes(string fileUri);
        void PutBytes(string fileUri, byte[] bytes);
        void DeleteFileIfAvailable(string fileUri);

        List<string> GetFileNames(string directory, string pattern);
    }
}
