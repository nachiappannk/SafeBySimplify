using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SafeModel
{
    public class SafeProvider : ISafeProvider
    {
        public const string UserNameCanNotBeEmptyErrorMessage = "The username can not be empty";

        public const string UserNameHasToBeMinimum8CharactersErrorMessage =
            "The username has to be minimum 8 characters";

        public const string UserNameHasToAlphaNumericWithNoSpecialCharactersErrorMessage =
            "The username can not have any special characters (only alpha numeric)";

        public const string UserNameProbablyExistsErrorMessage = "The username probably exists";

        public SafeProvider()
        {
            SettingGateway = new SettingGateway();
        }

        public ISettingGateway SettingGateway { get; set; }
        public IAccountGateway AccountGateway { get; set; }

        public string WorkingDirectory
        {
            get { return SettingGateway.GetWorkingDirectory(); }
            set { SettingGateway.SetWorkingDirectory(value); }
        }

        public bool IsUserNameValidForNonExistingUser(string userName, out string errorMessage)
        {
            if (string.IsNullOrEmpty(userName))
            {
                errorMessage = UserNameCanNotBeEmptyErrorMessage;
                return false;
            }
            if (userName.Length < 8)
            {
                errorMessage = UserNameHasToBeMinimum8CharactersErrorMessage;
                return false;
            }
            Regex regex = new Regex("^[a-zA-Z0-9]*$");
            if (!regex.IsMatch(userName))
            {
                errorMessage = UserNameHasToAlphaNumericWithNoSpecialCharactersErrorMessage;
                return false;
            }
            if (!AccountGateway.IsUsernameCreatable(userName))
            {
                errorMessage = UserNameProbablyExistsErrorMessage;
                return false;
            }
            errorMessage = string.Empty;
            return true;
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