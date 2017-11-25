using System.Collections.Generic;

namespace SafeModel
{
    public interface IAccountGateway
    {
        bool IsUsernameCreatable(string workingDirectory, string username);

        List<string> GetUserNames(string workingDirectory);
        void WriteUserRecord(string userName, byte[] masterPassBytes, string verifyingWord, byte[] verifyingWordEncryptedBytes);
    }
}