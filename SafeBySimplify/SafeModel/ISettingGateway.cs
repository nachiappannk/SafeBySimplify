using System.IO;

namespace SafeModel
{
    public interface ISettingGateway
    {
        string GetWorkingDirectory();
        void SetWorkingDirectory(string workingDirectory);
    }
}