using System;
using System.Linq;
using System.Net.NetworkInformation;

namespace SafeModel.Standard
{
    public class IdGenerator : IRecordIdGenerator, IFileIdGenerator
    {
        public string GetRecordId()
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
