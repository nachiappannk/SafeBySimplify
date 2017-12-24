using System;
using System.Linq;
using System.Net.NetworkInformation;
using Newtonsoft.Json;

namespace SafeModel
{
    public interface IUniqueIdGenerator
    {
        string GetUniqueId();
        string GetSemiUniqueId();
    }


    public class UniqueIdGenerator : IUniqueIdGenerator
    {
        public string GetUniqueId()
        {
            return GetMacId() + "-" + GetSemiUniqueId();
        }

        public string GetSemiUniqueId()
        {
            var dateTime = DateTime.Now;
            return dateTime.ToString("yyyyMMddhhmmssfff");
        }

        

        private string GetMacId()
        {
            var networkInterface = NetworkInterface.GetAllNetworkInterfaces().First();
            return networkInterface.GetPhysicalAddress().ToString();
        }
    }
}