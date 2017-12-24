using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

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
            var effectiveWorkingDirectory = workingDirectory + "\\" + userName;
            Directory.CreateDirectory(effectiveWorkingDirectory);
            var file = effectiveWorkingDirectory + "\\" + userName + ".acnt";

            var serializedAccount = JsonConvert.SerializeObject(account);
            File.WriteAllText(file, serializedAccount);

        }

        public Account ReadUserAccount(string workingDirectory, string userName)
        {
            var effectiveWorkingDirectory = workingDirectory + "\\" + userName;
            Directory.CreateDirectory(effectiveWorkingDirectory);
            var file = effectiveWorkingDirectory + "\\" + userName + ".acnt";


            var content = File.ReadAllText(file);
            return JsonConvert.DeserializeObject<Account>(content);

        }

    }
}