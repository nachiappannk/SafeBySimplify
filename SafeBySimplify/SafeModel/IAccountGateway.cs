using System.Collections.Generic;

namespace SafeModel
{
    public interface IAccountGateway
    {
        bool IsUsernameCreatable(string workingDirectory, string username);
        List<string> GetUserNames(string workingDirectory);
        void WriteUserRecord(string userName, byte[] masterPassBytes, string verifyingWord, byte[] verifyingWordEncryptedBytes);
    }

    public class AccountGatway : IAccountGateway
    {
        public bool IsUsernameCreatable(string workingDirectory, string username)
        {
            return true;
        }

        public List<string> GetUserNames(string workingDirectory)
        {
            return new List<string>() {  "one", "two", "three"};
        }

        public void WriteUserRecord(string userName, byte[] masterPassBytes, string verifyingWord, byte[] verifyingWordEncryptedBytes)
        {
        }
    }
}