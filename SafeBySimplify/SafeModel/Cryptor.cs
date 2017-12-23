namespace SafeModel
{
    public class Cryptor : ICryptor
    {
        public byte[] GetEncryptedBytes<T>(T content, string password)
        {
            throw new System.NotImplementedException();
        }

        public T GetDecryptedContent<T>(byte[] encryptedBytes, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}