using Newtonsoft.Json;
using System.Text;

namespace SafeModel.Standard
{
    public class Cryptor : ICryptor
    {

        public byte[] GetEncryptedBytes<T>(T content, string password)
        {
            var flatContent = JsonConvert.SerializeObject(content);
            AesCryptorWrapper cryptorWrapper = new AesCryptorWrapper(password);
            return cryptorWrapper.Encrypt(Encoding.UTF8.GetBytes(flatContent));
        }

        public T GetDecryptedContent<T>(byte[] encryptedBytes, string password)
        {
            AesCryptorWrapper cryptorWrapper = new AesCryptorWrapper(password);
            var decryptedBytes = cryptorWrapper.Decrypt(encryptedBytes);
            var searializedString = Encoding.UTF8.GetString(decryptedBytes);
            return JsonConvert.DeserializeObject<T>(searializedString);
        }
    }
}
