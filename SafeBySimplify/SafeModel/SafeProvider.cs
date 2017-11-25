using System.Collections.Generic;

namespace SafeModel
{
    public class SafeProvider : ISafeProvider
    {
        private readonly ISettingGateway _settingGateway;

        public SafeProvider(ISettingGateway settingGateway)
        {
            _settingGateway = settingGateway;
        }

        public string WorkingDirectory
        {
            get { return _settingGateway.GetWorkingDirectory(); }
            set { _settingGateway.SetWorkingDirectory(value); }
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

        public bool TryCreateSafeForExistingUser(string userName, string password, out ISafe safe)
        {
            safe = new SafeXXX();
            return true;
        }

        public List<string> GetUserNames()
        {
            return new List<string>() {"one", "two", "three"};
        }
    }
}