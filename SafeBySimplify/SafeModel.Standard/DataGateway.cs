using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SafeModel.Standard
{
    public class DataGateway : IDataGateway
    {
        public byte[] GetBytes(string fileUri)
        {
            return File.ReadAllBytes(fileUri);
        }

        public void PutBytes(string fileUri, byte[] bytes)
        {
            if (File.Exists(fileUri)) File.Delete(fileUri);
            File.WriteAllBytes(fileUri, bytes);
        }

        public void DeleteFileIfAvailable(string fileUri)
        {
            if (File.Exists(fileUri)) File.Delete(fileUri);
        }

        public List<string> GetFileNames(string directory, string pattern)
        {
            return Directory.GetFiles(directory, pattern).ToList();
        }
    }
}
