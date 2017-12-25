using System.Collections.Generic;

namespace SafeModel
{
    public interface ISafe : IFileSafe
    {
        List<RecordHeader> GetRecordHeaders(string searchText);
        Record GetRecord(string recordId);
        string WorkingDirectory { get; set; }
        string UserName { get; set; }

        void ReoganizeFiles(string recordId);
        void UpsertRecord(Record record);
    }

    public interface IFileSafe
    {
        void StoreFile(string recordId, string fileId, string fileUri);
        void RetreiveFile(string recordId, string fileId, string fileUri);
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

        public List<FileRecord> FileRecords { get; set; }
    }

    public class FileRecord
    {
        public string Name { get; set; }
        public string Extention { get; set; }
        public string FileId { get; set; }
    }
}