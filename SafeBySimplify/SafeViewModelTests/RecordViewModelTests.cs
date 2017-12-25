using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using SafeModel;
using SafeViewModel;

namespace SafeViewModelTests
{
    [TestFixture]
    public class RecordViewModelTests
    {
        private string _id;
        private string _name;
        private string _tagString;
        private RecordViewModel _recordViewModel;
        private List<PasswordRecord> _passwordRecords;
        private List<FileRecord> _fileRecords;
        private IFileSafe _fileSafe;
        private IFileIdGenerator _fileIdGenerator;


        [SetUp]
        public void SetUp()
        {
            _id = "someId";
            _name = "someName";
            _tagString = "tag1;tag2";

            _passwordRecords = new List<PasswordRecord>()
            {
                new PasswordRecord() {Name = "Name1", Value = "Value1"},
                new PasswordRecord() {Name = "Name2", Value = "Value2"},
                new PasswordRecord() {Name = "Name3", Value = "Value3"},
            };

            _fileRecords = new List<FileRecord>()
            {
                new FileRecord() {Name = "Name1", Extention = "pdf", Description = "Description1", AssociatedRecordId = _id, FileId = "F1"},
                new FileRecord() {Name = "Name2", Extention = "txt", Description = "Description2" , AssociatedRecordId = _id , FileId = "F2"},
                new FileRecord() {Name = "Name3", Extention = "xlsx", Description = "Description3" , AssociatedRecordId = _id , FileId = "F3"},
            };

            var record = new Record
            {
                Header = new RecordHeader(),
                FileRecords = new List<FileRecord>(),
                PasswordRecords = new List<PasswordRecord>()
            };

            record.Header.Id = _id;
            record.Header.Name = _name;
            record.Header.Tags = _tagString;
            record.PasswordRecords.AddRange(_passwordRecords);
            record.FileRecords.AddRange(_fileRecords);

            _fileSafe = Substitute.For<IFileSafe>();

            _fileIdGenerator = Substitute.For<IFileIdGenerator>();

            _recordViewModel = new RecordViewModel(record, _fileSafe, _fileIdGenerator);
        }
        
        private List<FileRecord> GetFileRecordsFromViewModel()
        {
            var fileRecords = _recordViewModel
                .FileRecords.Select(x => new FileRecord()
                {
                    Name = x.Name,
                    Extention = x.Extention,
                    FileId = x.FileRecordId,
                    Description = x.Description,
                    AssociatedRecordId = x.RecordId
                }).ToList();
            return fileRecords;
        }

        private List<PasswordRecord> GetPasswordRecordsFromViewModel()
        {
            var passwordRecords = _recordViewModel.PasswordRecords
                .Select(x => new PasswordRecord() { Name = x.Name, Value = x.Value }).ToList();
            return passwordRecords;
        }


        private List<PasswordRecord> CopyListAndAddRecordAtEnd(List<PasswordRecord> passwordRecords, string name, string value)
        {
            var expectedPasswordRecords = passwordRecords.ToList();
            expectedPasswordRecords.Add(new PasswordRecord() {Name = name, Value = value});
            return expectedPasswordRecords;
        }

        public abstract class RecordViewModelModification : RecordViewModelTests
        {
            [SetUp]
            public void RecordViewModelModificationSetUp()
            {
                ModifyViewModelAndExpectedValue();
            }


            protected abstract void ModifyViewModelAndExpectedValue();

            [Test]
            public void Name_is_correct()
            {
                Assert.AreEqual(_recordViewModel.Name, _name);
            }

            [Test]
            public void Id_is_correct()
            {
                Assert.AreEqual(_recordViewModel.Id, _id);
            }

            [Test]
            public void Tag_is_correct()
            {
                Assert.AreEqual(_recordViewModel.Tags, _tagString);
            }

            [Test]
            public void Password_records_are_correct()
            {
                var passwordRecords = GetPasswordRecordsFromViewModel();
                _passwordRecords = CopyListAndAddRecordAtEnd(_passwordRecords, string.Empty, string.Empty);
                CollectionAssert.AreEqual(_passwordRecords, passwordRecords);
            }

            [Test]
            public void View_model_file_record_is_same_as_that_of_records()
            {
                var fileRecords = GetFileRecordsFromViewModel();
                CollectionAssert.AreEqual(_fileRecords, fileRecords);
            }

            [Test]
            public void Record_got_is_correct()
            {
                var record = _recordViewModel.GetRecord();
                Assert.AreEqual(_name, record.Header.Name);
                Assert.AreEqual(_id, record.Header.Id);
                Assert.AreEqual(_tagString, record.Header.Tags);
                Assert.AreEqual(_passwordRecords, record.PasswordRecords);
                Assert.AreEqual(_fileRecords, record.FileRecords);
            }

            [Test]
            public void When_file_is_downloaded_then_file_is_retrieved()
            {
                var fileRecordViewModel = _recordViewModel.FileRecords.ElementAt(0);
                var fileUri = @"C:\Temp\Tes.pdf";
                fileRecordViewModel.DownloadFileAs(fileUri);
                _fileSafe.Received(1).RetreiveFile(_id, fileRecordViewModel.FileRecordId, fileUri);
            }
        }

        public class NameModification : RecordViewModelModification
        {
            protected override void ModifyViewModelAndExpectedValue()
            {
                var modifiedName = "newName";
                _recordViewModel.Name = modifiedName;
                _name = modifiedName;
            }
        }

        public class TagModification : RecordViewModelModification
        {
            protected override void ModifyViewModelAndExpectedValue()
            {
                var modifiedTagString = "Tag11;Tag12";
                _recordViewModel.Tags = modifiedTagString;
                _tagString = modifiedTagString;
            }
        }

        public class NoModification : RecordViewModelModification
        {
            protected override void ModifyViewModelAndExpectedValue()
            {
            }
        }


        public class AddingPasswordRecordAtEnd : RecordViewModelModification
        {
            protected override void ModifyViewModelAndExpectedValue()
            {
                var name = "ssss";
                var lastPasswordRecordViewModel = _recordViewModel.PasswordRecords.Last();
                lastPasswordRecordViewModel.Name = name;


                _passwordRecords.Add(new PasswordRecord() {Name = name, Value = string.Empty});
            }
        }

        public class PasswordRecordModified : RecordViewModelModification
        {
            protected override void ModifyViewModelAndExpectedValue()
            {
                var name = "ssss";
                var value = "5555";
                var passwordRecordViewModel = _recordViewModel.PasswordRecords.ElementAt(1);
                passwordRecordViewModel.Name = name;
                passwordRecordViewModel.Value = value;
                _passwordRecords.ElementAt(1).Name = name;
                _passwordRecords.ElementAt(1).Value = value;
            }
        }

        public class PasswordRecordIsDeleted : RecordViewModelModification
        {
            protected override void ModifyViewModelAndExpectedValue()
            {
                var passwordRecordViewModel = _recordViewModel.PasswordRecords.ElementAt(1);
                passwordRecordViewModel.RemoveCommand.Execute();
                _passwordRecords.Remove(_passwordRecords.ElementAt(1));
            }
        }

        public class FileRecordIsAdded : RecordViewModelModification
        {
            private string _fileId = "FileID";
            private string _fileUri = @"c:\temp\test.pdf";

            protected override void ModifyViewModelAndExpectedValue()
            {
                _fileIdGenerator.GetFileId().Returns(_fileId);
                _recordViewModel.AddFileRecord(_fileUri);

                var fileRecord = new FileRecord()
                {
                    Name =  "test",
                    Extention = "pdf",
                    FileId = _fileId,
                    AssociatedRecordId = _id,
                    Description = string.Empty,

                };
                _fileRecords.Add(fileRecord);
            }

            [Test]
            public void File_is_writtern_to_safe()
            {
                _fileSafe.Received(1).StoreFile(_id, _fileId, _fileUri);
            }

        }

        public class FileRecordIsModified : RecordViewModelModification
        {
            protected override void ModifyViewModelAndExpectedValue()
            {
                var fileRecordViewModel = _recordViewModel.FileRecords.ElementAt(1);
                var fileRecord = _fileRecords.ElementAt(1);

                var name = "name";
                fileRecordViewModel.Name = name;
                fileRecord.Name = name;

                var description = "desc";
                fileRecordViewModel.Description = description;
                fileRecord.Description = description;
            }
        }

        public class FileRecordIsDeleted : RecordViewModelModification
        {
            protected override void ModifyViewModelAndExpectedValue()
            {
                var fileRecordViewModel = _recordViewModel.FileRecords.ElementAt(1);
                var fileRecord = _fileRecords.ElementAt(1);

                fileRecordViewModel.DeleteCommand.Execute();
                _fileRecords.Remove(fileRecord);
            }
        }

    }
}