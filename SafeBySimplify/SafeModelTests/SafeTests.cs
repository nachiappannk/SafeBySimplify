using System;
using NSubstitute;
using NUnit.Framework;
using SafeModel;

namespace SafeModelTests
{
    [TestFixture]
    public partial class SafeTests
    {
        private string _safeWorkingDirectory = @"C:\Temp";
        private string _password = "password";
        private string _userName = "abcdefghi";
        private string _recordId = "recordId";
        private IDataGateway _dataGateway;
        private ICryptor _cryptor;
        private Safe _safe;

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
        }

        [Test]
        public void When_record_is_deleted_then_the_record_file_is_deleted()
        {
            _safe.DeleteRecord(_recordId);

            var fileUri = GetFileUri(_safeWorkingDirectory, _userName, _recordId, "rcd");
            _dataGateway.Received(1).DeleteRecordIfAvailable(fileUri);
        }

        private static string GetFileUri(string workingDirectory, string userName, string fileName, string extention)
        {
            var fileUri = workingDirectory + "\\" + userName + "\\" + fileName + "." + extention;
            return fileUri;
        }
    }

}