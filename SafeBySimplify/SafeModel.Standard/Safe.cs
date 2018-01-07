using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SafeModel.Standard
{
    public class Safe : ISafe
    {
        private readonly string _password;

        public Safe(string password)
        {
            _password = password;
            Cryptor = new Cryptor();
            DataGateway = new DataGateway();
            Searcher = new Searcher();
        }

        public ICryptor Cryptor { get; set; }

        public IDataGateway DataGateway { get; set; }

        public ISearcher Searcher { get; set; }

        public List<RecordHeader> GetRecordHeaders(string searchText)
        {

            var files = DataGateway.GetFileNames(GetEffectiveWorkingDirectory(), "*.rcd");
            var recordHeaders = files.Select(f => GetRecordFromUri(f).Header).ToList();
            return Searcher.Search(recordHeaders, searchText);
        }

        public void ReorganizeFiles(string recordId)
        {

            var validFiles = new List<string>();
            var recordFiles = DataGateway.GetFileNames(GetEffectiveWorkingDirectory(), recordId + ".rcd");
            if (recordFiles.Count != 0)
            {
                var record = GetRecord(recordId);
                validFiles.AddRange(record.FileRecords.Select(x => x.AssociatedRecordId + "_" + x.FileId).ToList());
            }

            var pattern = recordId + "_*.encfile";
            var files = DataGateway.GetFileNames(GetEffectiveWorkingDirectory(), pattern);
            foreach (var file in files)
            {
                var fileWithOutPath = Path.GetFileName(file);
                var fileNameWithoutExtention = Path.GetFileNameWithoutExtension(fileWithOutPath);
                if (!validFiles.Contains(fileNameWithoutExtention)) DataGateway.DeleteFileIfAvailable(file);
            }
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

            DataGateway.DeleteFileIfAvailable(recordFileUri);
            DataGateway.PutBytes(recordFileUri, encryptedRecordBytes);
        }

        public void DeleteRecord(string recordId)
        {
            var recordFile = GetRecordFileUri(recordId);
            DataGateway.DeleteFileIfAvailable(recordFile);
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
            return GetFileUri(recordId, "rcd");
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
