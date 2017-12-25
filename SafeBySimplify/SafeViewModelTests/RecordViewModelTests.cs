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
        private string _id = "someId";
        private string _name = "someName";
        private string _tagString = "tag1;tag2";
        private List<string> _tags;
        private RecordViewModel _recordViewModel;
        private List<PasswordRecord> _passwordRecords;
        private List<FileRecord> _fileRecords;
        private IFileSafe _fileSafe;
        private IFileIdGenerator _fileIdGenerator;


        [SetUp]
        public void SetUp()
        {
            _tags = _tagString.Split(';').ToList();

            _passwordRecords = new List<PasswordRecord>()
            {
                new PasswordRecord() {Name = "Name1", Value = "Value1"},
                new PasswordRecord() {Name = "Name2", Value = "Value2"},
                new PasswordRecord() {Name = "Name3", Value = "Value3"},
            };

            _fileRecords = new List<FileRecord>()
            {
                new FileRecord() {Name = "Name1", Extention = "pdf", Description = "Description1", AssociatedRecordId = "A1", FileId = "F1"},
                new FileRecord() {Name = "Name2", Extention = "txt", Description = "Description2" , AssociatedRecordId = "A1" , FileId = "F2"},
                new FileRecord() {Name = "Name3", Extention = "xlsx", Description = "Description3" , AssociatedRecordId = "A1" , FileId = "F3"},
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

        [Test]
        public void View_model_name_depends_on_record()
        {
            Assert.AreEqual(_recordViewModel.Name, _name);
        }

        [Test]
        public void View_model_id_depends_on_record()
        {
            Assert.AreEqual(_recordViewModel.Id, _id);
        }

        [Test]
        public void View_model_tags_depends_on_record()
        {
            Assert.AreEqual(_recordViewModel.Tags, _tagString);
        }

        [Test]
        public void View_model_password_records_is_same_as_password_records_of_the_seed_record_plus_one_empty_at_last()
        {
            var passwordRecords = GetPasswordRecordsFromViewModel();
            var expectedPasswordRecords = CopyListAndAddRecordAtEnd(_passwordRecords, string.Empty, string.Empty);
            CollectionAssert.AreEqual(expectedPasswordRecords, passwordRecords);
        }

        private List<PasswordRecord> GetPasswordRecordsFromViewModel()
        {
            var passwordRecords = _recordViewModel.PasswordRecords
                .Select(x => new PasswordRecord() {Name = x.Name, Value = x.Value}).ToList();
            return passwordRecords;
        }

        [Test]
        public void When_the_last_password_record_is_filled_then_an_empty_password_record_is_added()
        {
            var name = "someName";
            var value = "someValue";

            var lastRecord = _recordViewModel.PasswordRecords.Last();
            
            lastRecord.Name = name;
            lastRecord.Value = value;
            var list = CopyListAndAddRecordAtEnd(_passwordRecords, name, value);
            var expectedList = CopyListAndAddRecordAtEnd(list, string.Empty, string.Empty);

            CollectionAssert.AreEqual(expectedList, _recordViewModel.PasswordRecords.Select(x => new PasswordRecord()
            {
                Name = x.Name,
                Value =  x.Value,
            }));
        }

        [Test]
        public void View_model_file_record_is_same_as_that_of_records()
        {
            var fileRecords = GetFileRecordsFromViewModel();
            CollectionAssert.AreEqual(_fileRecords, fileRecords);
        }

        [Test]
        public void When_a_file_is_added_then_the_file_is_added_in_the_view_model()
        {
            var fileId = "FileID";
            _fileIdGenerator.GetFileId().Returns(fileId);

            var file = @"c:\temp\test.pdf";
            _recordViewModel.AddFileRecord(file);

            var fileRecordViewModel = _recordViewModel.FileRecords.Last();
            Assert.AreEqual(fileId, fileRecordViewModel.FileRecordId);
            Assert.AreEqual(_id, fileRecordViewModel.RecordId);
            Assert.AreEqual("test",fileRecordViewModel.Name);
            Assert.AreEqual(string.Empty, fileRecordViewModel.Description);
            Assert.AreEqual("pdf",fileRecordViewModel.Extention);
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

        private List<PasswordRecord> CopyListAndAddRecordAtEnd(List<PasswordRecord> passwordRecords, string name, string value)
        {
            var expectedPasswordRecords = passwordRecords.ToList();
            expectedPasswordRecords.Add(new PasswordRecord() {Name = name, Value = value});
            return expectedPasswordRecords;
        }
    }
}