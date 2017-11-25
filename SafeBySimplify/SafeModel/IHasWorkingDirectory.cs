namespace SafeModel
{
    public interface IHasWorkingDirectory
    {
        string WorkingDirectory { get; set; }
    }


    public interface ISettingGateway
    {
        string GetWorkingDirectory();
        string PutWorkingDirectory(string workingDirectory);
        bool IsWorkingDirectoryAvailable();

    }
}