using System.Collections.Generic;

namespace SafeModel
{
    public interface IAccountGateway
    {
        bool IsUsernameCreatable(string workingDirectory, string username);
        List<string> GetUserNames(string workingDirectory);
        void WriteUserRecord(string workingDirectory, string userName, Account account);

        Account ReadUserAccount(string workingDirectory, string userName);
    }

    public class Account
    {
        public byte[] MasterEncryptedPassBytes { get; set; }
        public string VerifyingWord { get; set; }
        public byte[] VeryifyingWordEncryptedBytes { get; set; }

    }
}