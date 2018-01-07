namespace SafeModel.Standard
{
    public interface IFileSafe
    {
        void StoreFile(string recordId, string fileId, string fileUri);
        void RetreiveFile(string recordId, string fileId, string fileUri);
    }
}
