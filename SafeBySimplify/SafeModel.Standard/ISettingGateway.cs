namespace SafeModel.Standard
{
    public interface ISettingGateway
    {
        string GetWorkingDirectory();
        void SetWorkingDirectory(string workingDirectory);
    }
}
