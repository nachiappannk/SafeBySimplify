using NSubstitute;
using NUnit.Framework;

namespace SafeModelTests
{
    public partial class SafeTests
    {
        public class FileTests : SafeTests
        {
            private string _fileUri = @"C:\Temp\My.pdf";
            private readonly byte[] _contentBytes = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 };
            private readonly byte[] _encryptedContentBytes = new byte[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 };
            private string _fileId = "fieldId";
            private string _encryptedFileUri;

            [SetUp]
            public void FileTestsSetUp()
            {
                _encryptedFileUri = GetEncryptedFileUri(_safeWorkingDirectory, _userName, _recordId, _fileId);
            }


            [Test]
            public void When_a_file_is_stored_it_is_encrypted_and_stored()
            {
                _safe.DataGateway.GetBytes(_fileUri).Returns(_contentBytes);
                _safe.Cryptor.GetEncryptedBytes(_contentBytes, _password).Returns(_encryptedContentBytes);
                _safe.StoreFile(_recordId, _fileId, _fileUri);
                _safe.DataGateway.Received(1).PutBytes(_encryptedFileUri, _encryptedContentBytes);
            }

            [Test]
            public void When_a_file_is_retrieved_it_is_encrypted_and_stored()
            {
                _safe.DataGateway.GetBytes(_encryptedFileUri).Returns(_encryptedContentBytes);
                _safe.Cryptor.GetDecryptedContent<byte[]>(_encryptedContentBytes, _password).Returns(_contentBytes);
                _safe.RetreiveFile(_recordId, _fileId, _fileUri);
                _safe.DataGateway.Received(1).PutBytes(_fileUri, _contentBytes);

            }

            private static string GetEncryptedFileUri(string workingDirectory, string userName, string recordId, string fileId)
            {
                return GetFileUri(workingDirectory, userName, recordId + "_" + fileId, "encfile");
            }

        }
    }
}