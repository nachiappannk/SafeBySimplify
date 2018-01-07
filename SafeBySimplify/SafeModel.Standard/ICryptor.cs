namespace SafeModel.Standard
{
    public interface ICryptor
    {
        byte[] GetEncryptedBytes<T>(T content, string password);
        T GetDecryptedContent<T>(byte[] encryptedBytes, string password);
    }
}
