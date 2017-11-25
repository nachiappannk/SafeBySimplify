namespace SafeModel
{
    public interface ICryptor
    {
        byte[] GetEncryptedBytes<T>(T content, string password);
    }
}