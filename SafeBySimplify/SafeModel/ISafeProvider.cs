using System.Collections.Generic;

namespace SafeModel
{
    public interface ISafeProvider : IHasWorkingDirectory , ISafeProviderForNonExistingUser , ISafeProviderForExistingUser
    {
    }

    public class SafeProvider : IHasWorkingDirectory
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
    }

    public interface ISafeProviderForNonExistingUser
    {
        bool IsUserNameValidForNonExistingUser(string userName, out string errorMessage);
        bool IsPasswordValidForNonExistingUser(string password, out string errorMessage);
        ISafe CreateSafeForNonExistingUser(string userName, string password);
    }

    public interface ISafeProviderForExistingUser
    {
        bool TryCreateSafeForExistingUser(string userName, string password, out ISafe safe);
        List<string> GetUserNames();
    }

    public interface ISafe
    {

    }
}