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

        [SetUp]
        public void SetUp()
        {
            _tags = _tagString.Split(';').ToList();

            var record = new Record
            {
                Header = new RecordHeader(),
                FileRecords = new List<FileRecord>(),
                PasswordRecords = new List<PasswordRecord>()
            };
            record.Header.Id = _id;
            record.Header.Name = _name;
            record.Header.Tags = _tagString;

            _recordViewModel = new RecordViewModel(record, null, null);
        }

        [Test]
        public void view_model_name_depenends_on_record()
        {
            Assert.AreEqual(_recordViewModel.Name, _name);
        }

        [Test]
        public void view_model_id_depenends_on_record()
        {
            Assert.AreEqual(_recordViewModel.Id, _id);
        }

        [Test]
        public void view_model_tags_depenends_on_record()
        {
            Assert.AreEqual(_recordViewModel.Tags, _tagString);
        }


    }
}