using System.Collections.Generic;
using System.IO;

namespace SafeModel
{
    public class AccountGatway : IAccountGateway
    {
        public bool IsUsernameCreatable(string workingDirectory, string username)
        {
            var effectiveWorkingDirectory = GetEffectiveWorkingDirectory(workingDirectory, username);
            return !Directory.Exists(effectiveWorkingDirectory);
        }

        private string GetEffectiveWorkingDirectory(string workingDirectory, string username)
        {
            return workingDirectory + "\\" + username;
        }

        public List<string> GetUserNames(string workingDirectory)
        {
            string[] subDirectories = Directory.GetDirectories(workingDirectory);
            var userNames = new List<string>();
            foreach (var subDirectory in subDirectories)
            {
                var directory = subDirectory.Replace(workingDirectory+"\\","");
                var accountFile = subDirectory +"\\"+ directory + ".acnt";
                if(File.Exists(accountFile)) userNames.Add(directory);
            }
            return userNames;
        }

        public void WriteUserRecord(string workingDirectory, string userName, Account account)
        {
        }

        public Account ReadUserAccount(string workingDirectory, string userName)
        {
            throw new System.NotImplementedException();
        }

    }
}