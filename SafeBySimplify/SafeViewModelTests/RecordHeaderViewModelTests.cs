using System.Collections.Generic;
using NUnit.Framework;
using SafeModel;
using SafeViewModel;

namespace SafeViewModelTests
{
    [TestFixture]
    public class RecordHeaderViewModelTests
    {
        private RecordHeaderViewModel _recordHeaderViewModel;
        private readonly string _recordName = "RecordName";
        private readonly List<string> _tags = new List<string>() {"one", "two", "three"};

        [SetUp]
        public void SetUp()
        {
            var recordHeader = new RecordHeader()
            {
                Name = _recordName,
                Tags = _tags,
            };
            _recordHeaderViewModel = new RecordHeaderViewModel(recordHeader, () => { });
        }

        [Test]
        public void Header_contains_name()
        {
            Assert.AreEqual(_recordName, _recordHeaderViewModel.Name);
        }

        [Test]
        public void Header_contains_tags()
        {
            CollectionAssert.AreEqual(_tags, _recordHeaderViewModel.Tags);
        }

        [Test]
        public void Selection_command_is_always_enabled()
        {
            Assert.True(_recordHeaderViewModel.SelectCommand.CanExecute());
        }
    }
}