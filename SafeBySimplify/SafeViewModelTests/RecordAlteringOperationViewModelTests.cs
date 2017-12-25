using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using SafeModel;
using SafeViewModel;

namespace SafeViewModelTests
{
    [TestFixture]
    public class RecordAlteringOperationViewModelTests
    {
        private Record _record;
        private ISafe _safe;
        private RecordAlteringOperationViewModel _recordAlteringOperationViewModel;

        [SetUp]
        public void SetUp()
        {
            _record = new Record()
            {
                Header = new RecordHeader()
                {
                    Id = "RecordId",
                    Name = "RecordName",
                    Tags = "Tag1;Tag2",
                },
                PasswordRecords = new List<PasswordRecord>()
                {
                    new PasswordRecord() {Name = "SomeName1", Value = "SomeValue1"},
                }
            };

            _safe = Substitute.For<ISafe>();
            var recordId = _record.Header.Id;
            _safe.GetRecord(recordId).Returns(_record);
            _recordAlteringOperationViewModel = new RecordAlteringOperationViewModel(_safe, recordId);

        }


        [Test]
        public void Initially_the_record_name_is_as_per_safe()
        {
            Assert.AreEqual(_record.Header.Name, _recordAlteringOperationViewModel.Record.Name);
        }

        [Test]
        public void Initially_the_record_tags_is_as_per_safe()
        {
            CollectionAssert.AreEquivalent(_record.Header.Tags, _recordAlteringOperationViewModel.Record.Tags);
        }
        

        [Test]
        public void Initially_the_password_records_is_as_per_safe()
        {
            var expectedRecordNameValues = _record.PasswordRecords.Select(x => x.Name + "--" + x.Value);
            var actualRecordNameValues = _recordAlteringOperationViewModel.Record.PasswordRecords.Select(x => x.Name + "--" + x.Value);
            CollectionAssert.AreEquivalent(expectedRecordNameValues, actualRecordNameValues);
        }
    }
}