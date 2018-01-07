namespace SafeModel.Standard
{
    public class SettingGatewayForTests : ISettingGateway
    {
        private string _workingDirectory;


        public string GetWorkingDirectory()
        {
            return _workingDirectory;
        }

        public void SetWorkingDirectory(string workingDirectory)
        {
            _workingDirectory = workingDirectory;
        }
    }
}
