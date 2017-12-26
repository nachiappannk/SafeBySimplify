using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using SafeModel;
using SafeViewModel;
using SafeViewModelTests.TestTools;

namespace SafeViewModelTests
{
    [TestFixture]
    public class RecordAlteringOperationViewModelTests
    {
        private Record _record;
        private ISafe _safe;
        private RecordAlteringOperationViewModel _recordAlteringOperationViewModel;
        private string _recordid;
        private bool _isReloadActionCalled = false;
        private bool _isCloseActionCalled = false;

        [SetUp]
        public void SetUp()
        {
            _recordid = "RecordId";
            _record = new Record()
            {
                Header = new RecordHeader()
                {
                    Id = _recordid,
                    Name = "RecordName",
                    Tags = "Tag1;Tag2",
                },
                PasswordRecords = new List<PasswordRecord>()
                {
                    new PasswordRecord() {Name = "SomeName1", Value = "SomeValue1"},
                },
                FileRecords = new List<FileRecord>()
                {
                    new FileRecord() {Name = "sss", Description = "ddd", Extention = "xyz", FileId = "ssss", AssociatedRecordId = _recordid},
                },
                
            };

            _safe = Substitute.For<ISafe>();
            var recordId = _record.Header.Id;
            _safe.GetRecord(recordId).Returns(_record);
            _recordAlteringOperationViewModel = new RecordAlteringOperationViewModel(_safe, 
                recordId, 
                new IdGenerator(),
                () => { _isReloadActionCalled = true; }, () => { _isCloseActionCalled = true; });

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
        public void Initially_the_record_id_is_as_per_safe()
        {
            CollectionAssert.AreEquivalent(_record.Header.Id, _recordAlteringOperationViewModel.Record.Id);
        }


        [Test]
        public void Initially_the_password_records_is_as_per_safe()
        {
            var passwordRecordsAtViewModel = _recordAlteringOperationViewModel.Record.PasswordRecords.Select(x =>
                new PasswordRecord() {Name = x.Name, Value = x.Value});
            var passwordRecords = _record.PasswordRecords.ToList();
            passwordRecords.Add(new PasswordRecord() {Name = string.Empty, Value = string.Empty});
            CollectionAssert.AreEqual(passwordRecords, passwordRecordsAtViewModel);
        }


        [Test]
        public void Initially_the_file_records_is_as_per_safe()
        {
            var fileRecordsAtViewModel = _recordAlteringOperationViewModel.Record.FileRecords.Select(x =>
                new FileRecord()
                {
                    Name = x.Name,
                    Extention = x.Extention,
                    FileId = x.FileRecordId,
                    AssociatedRecordId = x.RecordId,
                    Description = x.Description,
                });

            var fileRecords = _record.FileRecords.ToList();
            CollectionAssert.AreEqual(fileRecords, fileRecordsAtViewModel);
        }

        [Test]
        public void When_save_command_is_made_then_record_is_saved_files_are_reorganized_and_some_action_is_made()
        {
            Assume.That(_recordAlteringOperationViewModel.SaveCommand.CanExecute());
            _recordAlteringOperationViewModel.SaveCommand.Execute();

            _safe.Received(1).UpsertRecord(Arg.Is<Record>(r => r.Header.Id == _recordid));
            _safe.Received(1).ReorganizeFiles(_recordid);
            Assert.True(_isReloadActionCalled);
        }

        [Test]
        public void When_delete_command_then_record_is_deleted_and_files_are_reorganized_and_close_action_is_called()
        {
            Assume.That(_recordAlteringOperationViewModel.DeleteCommand.CanExecute());
            _recordAlteringOperationViewModel.DeleteCommand.Execute();
            _safe.Received(1).DeleteRecord(_recordid);
            _safe.Received(1).ReorganizeFiles(_recordid);
            Assert.True(_isCloseActionCalled);
        }

        [Test]
        public void When_discard_command_then_files_are_reorganized_and_reload_action_is_called()
        {
            Assume.That(_recordAlteringOperationViewModel.DiscardCommand.CanExecute());
            _recordAlteringOperationViewModel.DiscardCommand.Execute();
            _safe.Received(1).ReorganizeFiles(_recordid);
            Assert.True(_isReloadActionCalled);
        }

        [Test]
        public void When_go_to_search_command_then_files_are_reorganized_and_close_action_is_called()
        {
            Assume.That(_recordAlteringOperationViewModel.GoToSearchCommand.CanExecute());
            _recordAlteringOperationViewModel.GoToSearchCommand.Execute();
            _safe.Received(1).ReorganizeFiles(_recordid);
            Assert.True(_isCloseActionCalled);
        }

        [Test]
        public void When_record_is_modified_then_go_to_search_is_disabled()
        {
            var observer = _recordAlteringOperationViewModel.GoToSearchCommand.GetDelegateCommandObserver();
            _recordAlteringOperationViewModel.Record.Name = "newName";
            Assert.False(_recordAlteringOperationViewModel.GoToSearchCommand.CanExecute());
            Assert.False(observer.ValueOfCanExecuteOnLatestEvent);
        }

        [Test]
        public void Initially_go_to_search_is_enabled()
        {
            var observer = _recordAlteringOperationViewModel.GoToSearchCommand.GetDelegateCommandObserver();
            Assert.True(_recordAlteringOperationViewModel.GoToSearchCommand.CanExecute());
            Assert.True(observer.ValueOfCanExecuteOnLatestEvent);
        }


    }
}