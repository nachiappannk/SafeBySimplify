using System.Collections.Generic;

namespace SafeModel
{
    public interface ISafe
    {
        List<RecordHeader> GetRecordHeaders(string searchText);
        Record GetRecord(string recordId);

        string WorkingDirectory { get; set; }

        string UserName { get; set; }


    }


    public class PasswordRecord
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class Record
    {
        public RecordHeader Header { get; set; }

        public List<PasswordRecord> PasswordRecords { get; set; }
    }
}