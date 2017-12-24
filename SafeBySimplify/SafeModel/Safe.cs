﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SafeModel
{
    public class Safe : ISafe
    {
        public List<RecordHeader> GetRecordHeaders(string searchText)
        {
            Thread.Sleep(1000);
            var charArray = searchText.ToCharArray();
            var stringArray = charArray.Select((x, y) => x.ToString() + y.ToString());
            var recordHeaders = stringArray.Select(x => new RecordHeader()
            {
                Name = x,
                Tags = new List<string>() { "one", "two", "three" }
            }).ToList();
            return recordHeaders;
        }

        public Record GetRecord(string recordId)
        {
            throw new System.NotImplementedException();
        }

        public string WorkingDirectory { get; set; }
        public string UserName { get; set; }
    }
}