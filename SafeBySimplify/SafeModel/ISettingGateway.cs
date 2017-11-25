﻿using System;
using System.IO;

namespace SafeModel
{
    public interface ISettingGateway
    {
        string GetWorkingDirectory();
        string PutWorkingDirectory(string workingDirectory);
        bool IsWorkingDirectoryAvailable();

    }

    public class SettingGateway : ISettingGateway
    {
        public string GetWorkingDirectory()
        {
            var processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            var currentDirectory = Directory.GetCurrentDirectory();
            //throw new NotImplementedException();
            return "";
        }

        public string PutWorkingDirectory(string workingDirectory)
        {
            //throw new System.NotImplementedException();
            return "";
        }

        public bool IsWorkingDirectoryAvailable()
        {
            //throw new System.NotImplementedException();
            return true;
        }
    }
}