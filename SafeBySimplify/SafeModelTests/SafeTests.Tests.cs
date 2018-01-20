using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using SafeModel.Standard;

namespace SafeModelTests
{
    public partial class SafeTests
    {
        public class Tests : SafeTests
        {

            [Test]
            public void When_record_headers_are_requested_with_a_search_string()
            {

                var searcher = Substitute.For<ISearcher>();
                _safe.Searcher = searcher;

                var fileUris = new string[]
                {
                    @"C:\Temp\good.rcd",
                    @"C:\Temp\bad.rcd",
                };
                
                Record[] records = new Record[]
                {
                    new Record()
                    {
                        Header = new RecordHeader() {Id = "id1", Name = "some Name", Tags = "sssss"},
                    },

                    new Record()
                    {
                        Header = new RecordHeader() {Id = "ss", Name = "sssss", Tags = "sss;sss;"},
                    }, 
                };

                var encryptedFileBytes = new byte[][]
                {
                    new byte[] {2,3,42,12,3,12,3},
                    new byte[] {2,3,4,4,2,3,43,2, 4,23,22,12,3,12,3},
                };

                _dataGateway.GetFileNames(GetEffectiveWorkingDirectory(_safeWorkingDirectory, _userName), "*.rcd")
                    .Returns(fileUris.ToList());

                for (int i = 0; i < fileUris.Length; i++)
                {
                    _dataGateway.GetBytes(fileUris[i]).Returns(encryptedFileBytes[i]);
                    _cryptor.GetDecryptedContent<Record>(encryptedFileBytes[i], _password).Returns(records[i]);
                }

                var searchText = "ssomething";

                searcher.Search(Arg.Any<List<RecordHeader>>(), searchText).Returns(x =>
                {
                    var recordHeaders = x[0] as List<RecordHeader>;
                    recordHeaders.RemoveAt(0);
                    return recordHeaders;
                });

                var recordHeadersAtSafe = _safe.GetRecordHeaders(searchText);
                var expected = records.Select(x => x.Header).ToList();
                expected.RemoveAt(0);
                CollectionAssert.AreEqual(expected, recordHeadersAtSafe);
            }


            [Test]
            public void When_record_is_deleted_then_the_record_file_is_deleted()
            {
                _safe.DeleteRecord(_recordId);

                var fileUri = GetRecordFileUri(_recordId);
                _dataGateway.Received(1).DeleteFileIfAvailable(fileUri);
            }

            [Test]
            public void When_record_is_upserted_then_the_old_record_is_deleted_if_needed()
            {
                _cryptor.GetEncryptedBytes(_record, _password).Returns(_recordEncryptedBytes);


                _safe.UpsertRecord(_record);

                var fileUri = GetRecordFileUri(_recordId);
                _dataGateway.Received(1).DeleteFileIfAvailable(fileUri);
                _dataGateway.Received(1).PutBytes(fileUri, _recordEncryptedBytes);
            }

            [Test]
            public void When_record_is_read_then_encrypted_file_is_read_and_decrtyped_to_a_record()
            {

                _cryptor.GetDecryptedContent<Record>(_recordEncryptedBytes, _password).Returns(_record);
                _dataGateway.GetBytes(GetRecordFileUri(_recordId)).Returns(_recordEncryptedBytes);
                var record = _safe.GetRecord(_recordId);
                Assert.AreEqual(_record, record);
            }


        }


    }
}