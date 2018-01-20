using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using SafeModel.Standard;

namespace SafeModelTests
{
    [TestFixture]
    public partial class SafeTests
    {
        private string _safeWorkingDirectory = @"C:\Temp";
        private string _password = "password";
        private string _userName = "abcdefghi";
        private IDataGateway _dataGateway;
        private ICryptor _cryptor;
        private Safe _safe;

        private Record _record;

        private string _recordId = "recordId";
        private string _recordName = "SomeName";
        private string _recordTags = "SomeTags;tags2";

        private readonly byte[] _recordEncryptedBytes = new byte[] { 2, 5, 2, 5, 2, 4, 2, 42, 3 };

        [SetUp]
        public void SetUp()
        {
            _dataGateway = Substitute.For<IDataGateway>();
            _cryptor = Substitute.For<ICryptor>();

            _safe = new Safe(_password);
            _safe.WorkingDirectory = _safeWorkingDirectory;
            _safe.UserName = _userName;
            _safe.DataGateway = _dataGateway;
            _safe.Cryptor = _cryptor;


            _record = new Record
            {
                FileRecords = new List<FileRecord>(),
                PasswordRecords = new List<PasswordRecord>(),
                Header = new RecordHeader
                {
                    Id = _recordId,
                    Name = _recordName,
                    Tags = _recordTags
                }
            };
        }

        private string GetFileUri(string workingDirectory, string userName, string fileName, string extention)
        {
            var fileUri = GetEffectiveWorkingDirectory(workingDirectory, userName) + "\\" + fileName + "." + extention;
            return fileUri;
        }

        private string GetEffectiveWorkingDirectory(string workingDirectory, string userName)
        {
            return workingDirectory + "\\" + userName;
        }

        private string GetRecordFileUri(string recordId)
        {
            return GetFileUri(_safeWorkingDirectory, _userName, recordId, "rcd");
        }
    }

}