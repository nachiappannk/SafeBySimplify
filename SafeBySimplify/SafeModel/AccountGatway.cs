using System.Collections.Generic;

namespace SafeModel
{
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

        public void WriteUserRecord(string userName, Account account)
        {
        }

        public Account ReadUserAccount(string userName)
        {
            throw new System.NotImplementedException();
        }

    }
}