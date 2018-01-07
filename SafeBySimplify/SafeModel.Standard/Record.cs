using System.Collections.Generic;

namespace SafeModel.Standard
{
    public class Record
    {
        public RecordHeader Header { get; set; }
        public List<PasswordRecord> PasswordRecords { get; set; }
        public List<FileRecord> FileRecords { get; set; }
    }
}
