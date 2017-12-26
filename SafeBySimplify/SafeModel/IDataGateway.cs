namespace SafeModel
{
    public interface IDataGateway
    {
        byte[] GetBytes(string fileUri);
        void PutBytes(string fileUri, byte[] bytes);
        void DeleteRecordIfAvailable(string fileUri);
    }
}