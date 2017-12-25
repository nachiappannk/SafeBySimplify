using System.Collections.Generic;
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
                Tags = "one;two;three",
            }).ToList();
            return recordHeaders;
        }

        public Record GetRecord(string recordId)
        {
            var record = new Record();
            record.Header = new RecordHeader();
            record.FileRecords = new List<FileRecord>();
            record.PasswordRecords = new List<PasswordRecord>();
            record.Header.Id = recordId;
            return record;
        }

        public string WorkingDirectory { get; set; }
        public string UserName { get; set; }
        public void StoreFile(string recordId, string fileId, string fileUri)
        {
        }

        public void RetreiveFile(string recordId, string fileId, string fileUri)
        {
        }

        public void ReoganizeFiles(string recordId)
        {
        }

        public void UpsertRecord(Record record)
        {
        }

        public void DeleteRecord(string recordId)
        {

        }
    }
}