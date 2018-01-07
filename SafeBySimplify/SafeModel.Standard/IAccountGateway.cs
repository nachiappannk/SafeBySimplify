using System.Collections.Generic;

namespace SafeModel.Standard
{
    public interface IAccountGateway
    {
        bool IsUsernameCreatable(string workingDirectory, string username);
        List<string> GetUserNames(string workingDirectory);
        void WriteUserRecord(string workingDirectory, string userName, Account account);

        Account ReadUserAccount(string workingDirectory, string userName);
    }
}
