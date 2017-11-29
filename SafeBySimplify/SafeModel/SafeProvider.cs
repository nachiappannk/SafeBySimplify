using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

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

        public const string PasswordTooShortErrorMessage = "Password has to be 8 or more characters";

        public SafeProvider()
        {
            SettingGateway = new SettingGateway();
            AccountGateway = new AccountGatway();
        }

        public ISettingGateway SettingGateway { get; set; }
        public IAccountGateway AccountGateway { get; set; }

        public ICryptor  Cryptor { get; set; }

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
            if (!AccountGateway.IsUsernameCreatable(WorkingDirectory, userName))
            {
                errorMessage = UserNameProbablyExistsErrorMessage;
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }

        public bool IsPasswordValidForNonExistingUser(string password, out string errorMessage)
        {
            errorMessage = PasswordTooShortErrorMessage;
            if (password.Length < 8) return false;
            errorMessage = string.Empty;
            return true;
        }

        public class SafeXXX : ISafe
        {
            
            public Task<List<RecordHeader>> GetRecordsAsync(string searchText, CancellationToken token)
            {
                return Task.Run(() =>
                {
                    var charArray = searchText.ToCharArray();
                    var stringArray = charArray.Select((x, y) => x.ToString() + y.ToString());
                    var recordHeaders = stringArray.Select(x => new RecordHeader()
                    {
                        Name = x,
                        Tags = new List<string>() { "one", "two", "three"}
                    }).ToList();
                    return recordHeaders;

                });
            }
        }

        public ISafe CreateSafeForNonExistingUser(string userName, string masterpassword, string password)
        {

            var masterPassBytes = GetEncryptedBytes(masterpassword, password);
            var verifyingWord = "SafeBySimplify";
            var veriyfingWordEncryptedBytes = GetEncryptedBytes(verifyingWord, password);
            AccountGateway.WriteUserRecord(userName, masterPassBytes, verifyingWord, veriyfingWordEncryptedBytes);

            return new SafeXXX();
        }

        private byte[] GetEncryptedBytes<T>(T content, string password)
        {
            return Cryptor.GetEncryptedBytes(content, password);
        }

        public bool TryCreateSafeForExistingUser(string userName, string password, out ISafe safe)
        {
            safe = new SafeXXX();
            return true;
        }

        public List<string> GetUserNames()
        {
            return AccountGateway.GetUserNames(WorkingDirectory);
        }
    }
}