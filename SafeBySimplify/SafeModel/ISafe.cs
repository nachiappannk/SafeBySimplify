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

        void DeleteRecord(string recordId);
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

        public override bool Equals(object obj)
        {
            var passwordRecord = obj as PasswordRecord;
            return Equals(passwordRecord);
        }

        protected bool Equals(PasswordRecord other)
        {
            return string.Equals(Name, other.Name) && string.Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return 243;
        }
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
        public string Description { get; set; }
        public string FileId { get; set; }
        public string AssociatedRecordId { get; set; }
        public override bool Equals(object obj)
        {
            var fileRecord = obj as FileRecord;
            return Equals(fileRecord);
        }

        protected bool Equals(FileRecord fileRecord)
        {
            return string.Equals(Name, fileRecord.Name)
                   && string.Equals(Extention, fileRecord.Extention)
                   && string.Equals(Description, fileRecord.Description)
                   && string.Equals(FileId, fileRecord.FileId)
                   && string.Equals(AssociatedRecordId, fileRecord.AssociatedRecordId);
        }

        public override int GetHashCode()
        {
            return 2445;
        }
    }
}