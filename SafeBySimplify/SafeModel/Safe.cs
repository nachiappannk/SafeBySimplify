using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SafeModel
{
    public class Safe : ISafe
    {
        private readonly string _password;

        public Safe(string password)
        {
            _password = password;
            Cryptor = new Cryptor();
            DataGateway = new DataGateway();
        }

        public ICryptor Cryptor { get; set; }

        public IDataGateway DataGateway { get; set; }

        public List<RecordHeader> GetRecordHeaders(string searchText)
        {

            var files = DataGateway.GetRecordNames(GetEffectiveWorkingDirectory(), "*.rcd");

            return files.Select(f => GetRecordFromUri(f).Header).ToList();


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

        public void ReoganizeFiles(string recordId)
        {

        }

        public Record GetRecord(string recordId)
        {
            var recordFileUri = GetRecordFileUri(recordId);
            return GetRecordFromUri(recordFileUri);
        }

        private Record GetRecordFromUri(string recordFileUri)
        {
            var encryptedBytes = DataGateway.GetBytes(recordFileUri);
            var record = Cryptor.GetDecryptedContent<Record>(encryptedBytes, _password);
            return record;
        }

        public string WorkingDirectory { get; set; }
        public string UserName { get; set; }




        public void UpsertRecord(Record record)
        {
            var recordFileUri = GetRecordFileUri(record.Header.Id);
            var encryptedRecordBytes = Cryptor.GetEncryptedBytes(record, _password);

            DataGateway.DeleteRecordIfAvailable(recordFileUri);
            DataGateway.PutBytes(recordFileUri, encryptedRecordBytes);
        }

        public void DeleteRecord(string recordId)
        {
            var recordFile = GetRecordFileUri(recordId);
            DataGateway.DeleteRecordIfAvailable(recordFile);
        }


        public void StoreFile(string recordId, string fileId, string fileUri)
        {
            var effectiveFile = GetEncryptedFileUri(recordId, fileId);

            var fileBytes = DataGateway.GetBytes(fileUri);
            var encryptedBytes = Cryptor.GetEncryptedBytes(fileBytes, _password);
            DataGateway.PutBytes(effectiveFile, encryptedBytes);

        }

        private string GetEncryptedFileUri(string recordId, string fileId)
        {
            return GetFileUri(recordId + "_" + fileId, "encfile");
        }

        private string GetRecordFileUri(string recordId)
        {
            return GetFileUri(recordId,"rcd");
        }

        private string GetFileUri(string fileName, string extention)
        {
            var effectiveWorkingDirectory = GetEffectiveWorkingDirectory();
            var effictiveFile = effectiveWorkingDirectory + "\\" + fileName + "." + extention;
            return effictiveFile;
        }

        private string GetEffectiveWorkingDirectory()
        {
            var effectiveWorkingDirectory = WorkingDirectory + "\\" + UserName;
            return effectiveWorkingDirectory;
        }

        public void RetreiveFile(string recordId, string fileId, string fileUri)
        {
            var encryptedFileUri = GetEncryptedFileUri(recordId, fileId);
            var encryptedBytes = DataGateway.GetBytes(encryptedFileUri);
            var fileByes = Cryptor.GetDecryptedContent<byte[]>(encryptedBytes, _password);
            DataGateway.PutBytes(fileUri, fileByes);
        }
    }
}