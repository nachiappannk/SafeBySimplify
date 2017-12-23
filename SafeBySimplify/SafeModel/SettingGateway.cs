using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SafeModel
{
    public class SettingGateway : ISettingGateway
    {
        public string GetWorkingDirectory()
        {
            var workingDirectory = string.Empty;
            var isWorkingDirectoryAvailable = TryGetWorkingDirectoryFromSettings(out workingDirectory);
            if (isWorkingDirectoryAvailable) return workingDirectory;
            var defaultWorkingDirectory = GetDefaultWorkingDirectory();
            if (!Directory.Exists(defaultWorkingDirectory))
            {
                Directory.CreateDirectory(defaultWorkingDirectory);
            }
            return defaultWorkingDirectory;
        }

        private static string GetDefaultWorkingDirectory()
        {
            var assemblyNameWithExtentionAndPath = Assembly.GetEntryAssembly().Location;
            var assemblyName = Path.GetFileNameWithoutExtension(assemblyNameWithExtentionAndPath);
            var path = Path.GetDirectoryName(assemblyNameWithExtentionAndPath);
            var result = path + "\\" + assemblyName;

            return result;
        }


        private static string GetSettingFile()
        {
            var assemblyNameWithExtentionAndPath = Assembly.GetEntryAssembly().Location;
            var assemblyName = Path.GetFileNameWithoutExtension(assemblyNameWithExtentionAndPath);
            var path = Path.GetDirectoryName(assemblyNameWithExtentionAndPath);
            //var result = path + "\\" + assemblyName + ".stng";

            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var myAppData = localAppData +"\\" + assemblyName;
            Directory.CreateDirectory(myAppData);
            var result = myAppData + "\\" + assemblyName + ".stng";


            return result;
        }

        private static bool TryGetWorkingDirectoryFromSettings(out string workingDirectory)
        {
            workingDirectory = String.Empty;
            var settingsFile = GetSettingFile();
            if (!File.Exists(settingsFile)) return false;
            var lines = File.ReadLines(settingsFile).ToList();
            if (!lines.Any()) return false;
            var firstLine = lines.ElementAt(0);
            if (!Directory.Exists(firstLine)) return false;
            workingDirectory = firstLine;
            return true;
        }

        public void SetWorkingDirectory(string workingDirectory)
        {
            var settingsFile = GetSettingFile();
            File.WriteAllLines(settingsFile, new List<string>() {workingDirectory});
        }
    }
}