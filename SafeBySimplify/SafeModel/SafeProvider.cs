namespace SafeModel
{
    public class SafeProvider : IHasWorkingDirectory, ISafeProviderForNonExistingUser
    {
        private readonly ISettingGateway _settingGateway;

        public SafeProvider(ISettingGateway settingGateway)
        {
            _settingGateway = settingGateway;
        }

        public string WorkingDirectory
        {
            get
            {
                if (_settingGateway.IsWorkingDirectoryAvailable())
                    return _settingGateway.GetWorkingDirectory();
                return "";
            }
            set { _settingGateway.PutWorkingDirectory(value); }
        }

        public bool IsUserNameValidForNonExistingUser(string userName, out string errorMessage)
        {
            //TBD
            errorMessage = string.Empty;
            if(userName.Length > 4) return true;
            return false;
        }

        public bool IsPasswordValidForNonExistingUser(string password, out string errorMessage)
        {
            //TBD
            errorMessage = string.Empty;
            if (password.Length > 4) return true;
            return false;
        }

        public class SafeXXX : ISafe
        {

        }

        public ISafe CreateSafeForNonExistingUser(string userName, string password)
        {
            return new SafeXXX();
        }
    }
}