using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SafeModel.Standard
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
            Cryptor = new Cryptor();
        }

        public ISettingGateway SettingGateway { get; set; }
        public IAccountGateway AccountGateway { get; set; }
        public ICryptor Cryptor { get; set; }

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



        public ISafe CreateSafeForNonExistingUser(string userName, string masterpassword, string password)
        {

            var masterPassBytes = GetEncryptedBytes(masterpassword, password);
            var verifyingWord = "SafeBySimplify";
            var veriyfingWordEncryptedBytes = GetEncryptedBytes(verifyingWord, password);

            var account = new Account()
            {
                MasterEncryptedPassBytes = masterPassBytes,
                VerifyingWord = verifyingWord,
                VeryifyingWordEncryptedBytes = veriyfingWordEncryptedBytes,
            };
            AccountGateway.WriteUserRecord(WorkingDirectory, userName, account);

            var safeForNonExistingUser = new Safe(masterpassword);
            safeForNonExistingUser.UserName = userName;
            safeForNonExistingUser.WorkingDirectory = WorkingDirectory;
            return safeForNonExistingUser;
        }

        private byte[] GetEncryptedBytes<T>(T content, string password)
        {
            return Cryptor.GetEncryptedBytes(content, password);
        }

        public bool TryCreateSafeForExistingUser(string userName, string password, out ISafe safe)
        {
            var account = AccountGateway.ReadUserAccount(WorkingDirectory, userName);
            var verifyingWordBytesForCurrentPassword = Cryptor.GetEncryptedBytes(account.VerifyingWord, password);
            try
            {
                var masterPassword = Cryptor.GetDecryptedContent<string>(account.MasterEncryptedPassBytes, password);
                safe = null;

                if (account.VeryifyingWordEncryptedBytes.Length != verifyingWordBytesForCurrentPassword.Length)
                    return false;
                for (var i = 0; i < account.VeryifyingWordEncryptedBytes.Length; i++)
                {
                    if (account.VeryifyingWordEncryptedBytes[i] != verifyingWordBytesForCurrentPassword[i])
                        return false;
                }
                safe = new Safe(masterPassword);
                safe.UserName = userName;
                safe.WorkingDirectory = WorkingDirectory;
                return true;
            }
            catch (Exception e)
            {
                safe = null;
                return false;
            }



        }

        public List<string> GetUserNames()
        {
            return AccountGateway.GetUserNames(WorkingDirectory);
        }
    }
}
