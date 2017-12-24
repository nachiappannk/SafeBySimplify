using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;
using SafeModel;

namespace SafeViewModelTests
{
    public partial class AddOperationViewModelTests
    {
        public class ValidNameIsEntered : AddOperationViewModelTests
        {
            [SetUp]
            public void ValidNameIsEnteredSetUp()
            {
                _addOperationViewModel.Record.Name = "SomeName";
                Assume.That(_saveCommandObserver.ValueOfCanExecuteOnLatestEvent);
            }

            [Test]
            public void When_name_is_made_non_empty_and_then_empty_then_save_command_is_disabled()
            {
                _addOperationViewModel.Record.Name = String.Empty;
                Assert.False(_saveCommandObserver.ValueOfCanExecuteOnLatestEvent);
            }


            [Test]
            public void When_saved_the_record_is_written_in_safe_the_files_are_reoganized()
            {
                _uniqueIdGenerator.GetSemiUniqueId().Returns("ssss");

                var recordName = "RecordName";
                _addOperationViewModel.Record.Name = recordName;
                _addOperationViewModel.Record.Tags = new List<string>() {"One","Two"};
                var passwordRecord = _addOperationViewModel.Record.PasswordRecords.Last();
                passwordRecord.Name = "One";
                passwordRecord.Value = "Two";

                _addOperationViewModel.Record.AddFileRecord("D:\\test\\One.pdf");

                var fileRecord = _addOperationViewModel.Record.FileRecords.First();

                var passwordRecords = new List<PasswordRecord>();
                passwordRecords.Add(new PasswordRecord() {Name =  "One", Value = "Two"});

                _addOperationViewModel.SaveCommand.Execute();
                _safe.Received(1).UpsertRecord(Arg.Is<Record>(
                    (x) => 
                    x.Header.Name == recordName &&
                    x.Header.Id == _uniqueId &&
                    x.Header.Tags.Join("-") == _addOperationViewModel.Record.Tags.Join("-") &&
                    IsPasswordRecordsMatching(x.PasswordRecords, passwordRecords) &&
                    x.FileRecords.First().Name == "One" &&
                    x.FileRecords.First().Extention == "pdf" &&
                    x.FileRecords.First().FileId == fileRecord.FileRecordId
                    ));
                Assert.AreEqual(_uniqueId, _idAtSaveAction);
            }

            private bool IsPasswordRecordsMatching(List<PasswordRecord> r1, List<PasswordRecord> r2)
            {
                if (r1.Count != r2.Count) return false;
                for (int i = 0; i < r1.Count; i++)
                {
                    if (r1[i].Name != r2[i].Name) return false;
                    if (r1[i].Value != r2[i].Value) return false;
                }
                return true;
            }
        }
    }
}