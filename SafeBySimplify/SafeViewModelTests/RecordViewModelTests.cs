using System.Collections.Generic;
using System.Linq;
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

            _recordViewModel = new RecordViewModel(record, null, null);
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
            var passwordRecords = _recordViewModel.PasswordRecords.Select(x => new PasswordRecord() {Name = x.Name, Value = x.Value}).ToList();
            var expectedPasswordRecords = CopyListAndAddRecordAtEnd(_passwordRecords, string.Empty, string.Empty);
            CollectionAssert.AreEqual(expectedPasswordRecords, passwordRecords);
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

        private List<PasswordRecord> CopyListAndAddRecordAtEnd(List<PasswordRecord> passwordRecords, string name, string value)
        {
            var expectedPasswordRecords = passwordRecords.ToList();
            expectedPasswordRecords.Add(new PasswordRecord() {Name = name, Value = value});
            return expectedPasswordRecords;
        }
    }
}