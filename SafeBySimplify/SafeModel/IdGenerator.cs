using System;
using System.Linq;
using System.Net.NetworkInformation;

namespace SafeModel
{
    public class IdGenerator : IUniqueIdGenerator, IFileIdGenerator
    {
        public string GetUniqueId()
        {
            return GetMacId() + "-" + GetTimeBasedId();
        }

        public string GetTimeBasedId()
        {
            var dateTime = DateTime.Now;
            return dateTime.ToString("yyyyMMddhhmmssfff");
        }

        private string GetMacId()
        {
            var networkInterface = NetworkInterface.GetAllNetworkInterfaces().First();
            return networkInterface.GetPhysicalAddress().ToString();
        }

        public string GetFileId()
        {
            return GetTimeBasedId();
        }
    }
}