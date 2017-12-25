using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SafeModel;
using SafeViewModel;
using SafeViewModelTests.TestTools;

namespace SafeViewModelTests
{
    [TestFixture]
    public partial class AddOperationViewModelTests
    {
        private AddOperationViewModel _addOperationViewModel;
        private bool _isDiscardActionPerformed = false;
        private string _idAtSaveAction;
        private CommandObserver _saveCommandObserver;
        private IUniqueIdGenerator _uniqueIdGenerator;
        private string _uniqueId = "SomeUniqueID";
        private ISafe _safe;
        private IFileIdGenerator _fileIdGenerator;

        [SetUp]
        public void SetUp()
        {
            _uniqueIdGenerator = Substitute.For<IUniqueIdGenerator>();
            _fileIdGenerator = Substitute.For<IFileIdGenerator>();
            _uniqueIdGenerator.GetUniqueId().Returns(_uniqueId);

            _safe = Substitute.For<ISafe>();

            _addOperationViewModel = new AddOperationViewModel(() => { _isDiscardActionPerformed = true; }, (x) =>
                {
                    _idAtSaveAction = x;
                }, 
                _uniqueIdGenerator,
                _fileIdGenerator,
                _safe);
            _saveCommandObserver = _addOperationViewModel.SaveCommand.GetDelegateCommandObserver();
        }

        [Test]
        public void When_discarded_then_the_record_is_reoganized_in_safe_and_discard_action_is_executed()
        {
            _addOperationViewModel.DiscardCommand.Execute();
            _safe.Received(1).ReoganizeFiles(_uniqueId);
            Assert.True(_isDiscardActionPerformed);
        }

        [Test]
        public void When_file_is_added_then_correct_file_record_is_added()
        {
            bool _isCollectionModified = false;
            _addOperationViewModel.Record.FileRecords.CollectionChanged += (a, b) => { _isCollectionModified = true; };

            var fileId = "fileId";

            _fileIdGenerator.GetFileId().Returns(fileId);

            
            var file = @"D:\Test\One.pdf";
            _addOperationViewModel.Record.AddFileRecord(file);
            var correspondingFileRecord = _addOperationViewModel.Record.FileRecords.Last();

            Assert.True(_isCollectionModified);
            Assert.AreEqual("One", correspondingFileRecord.Name);
            Assert.AreEqual("pdf", correspondingFileRecord.Extention);
            Assert.True(correspondingFileRecord.DeleteCommand.CanExecute());

            _safe.Received(1).StoreFile(_uniqueId, fileId, file);

        }

        [Test]
        public void When_file_is_downloaded_then_correct_file_record_is_downloaded()
        {
            var file = @"D:\Test\One.pdf";
            _addOperationViewModel.Record.AddFileRecord(file);

            var correspondingFileRecord = _addOperationViewModel.Record.FileRecords.Last();

            correspondingFileRecord.DownloadFileAs(file);

            _safe.Received(1).RetreiveFile(_uniqueId, correspondingFileRecord.FileRecordId, file);
        }

        [TestCase("One")]
        [TestCase("Two")]
        public void When_file_is_deleted_the_correct_file_is_removed_from_ui(string fileToBeDeleted)
        {
            var file1 = @"D:\Test\One.pdf";
            var file2 = @"D:\Test\Two.pdf";
            bool _isCollectionModified = false;
            _addOperationViewModel.Record.AddFileRecord(file1);
            _addOperationViewModel.Record.AddFileRecord(file2);

            _addOperationViewModel.Record.FileRecords.CollectionChanged += (a, b) => { _isCollectionModified = true; };

            var viewModelToBeDeleted = _addOperationViewModel.Record.FileRecords.Last(x => x.Name == fileToBeDeleted);
            viewModelToBeDeleted.DeleteCommand.Execute();

            Assert.True(_isCollectionModified);
            Assert.False(_addOperationViewModel.Record.FileRecords.Select(x => x.Name).Contains(fileToBeDeleted));
        }

        [Test]
        public void When_details_are_filled_and_removed_then_correct_record_is_removed()
        {
            var filedToBeRemoved = "Field2";
            var valueToBeRemoved = "Value2";
            List<Tuple<string, string>> fieldNameAndValues = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("Field1", "Value1"),
                new Tuple<string, string>(filedToBeRemoved, valueToBeRemoved),
                new Tuple<string, string>("Field3", "Value3")
            };

            fieldNameAndValues.ForEach(x => AddPasswordRecordAtEnd(x.Item1, x.Item2));

            var removeCommand = _addOperationViewModel.Record.PasswordRecords.First(x => x.Name == filedToBeRemoved).RemoveCommand;
            Assume.That(removeCommand.CanExecute());
            removeCommand.Execute();

            CollectionAssert.IsEmpty(_addOperationViewModel.Record.PasswordRecords.Where(x => x.Name == filedToBeRemoved));
            CollectionAssert.IsEmpty(_addOperationViewModel.Record.PasswordRecords.Where(x => x.Value == valueToBeRemoved));

        }

        private void AddPasswordRecordAtEnd(string fieldName, string fieldValue)
        {
            var initialNumberOfPasswordRecords = _addOperationViewModel.Record.PasswordRecords.Count;
            var passwordRecord = _addOperationViewModel.Record.PasswordRecords.ElementAt(initialNumberOfPasswordRecords - 1);
            passwordRecord.Name = fieldName;
            passwordRecord.Value = fieldValue;
        }
    }
}