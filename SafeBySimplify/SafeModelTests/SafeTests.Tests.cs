using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using SafeModel;

namespace SafeModelTests
{
    public partial class SafeTests
    {
        public class Tests : SafeTests
        {


            [Test]
            public void When_record_is_deleted_then_the_record_file_is_deleted()
            {
                _safe.DeleteRecord(_recordId);

                var fileUri = GetRecordFileUri(_recordId);
                _dataGateway.Received(1).DeleteRecordIfAvailable(fileUri);
            }

            [Test]
            public void When_record_is_upserted_then_the_old_record_is_deleted_if_needed()
            {
                _cryptor.GetEncryptedBytes(_record, _password).Returns(_recordEncryptedBytes);


                _safe.UpsertRecord(_record);

                var fileUri = GetRecordFileUri(_recordId);
                _dataGateway.Received(1).DeleteRecordIfAvailable(fileUri);
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