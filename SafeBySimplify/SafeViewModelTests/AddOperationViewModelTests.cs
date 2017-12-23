using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SafeViewModel;
using SafeViewModelTests.TestTools;

namespace SafeViewModelTests
{
    [TestFixture]
    public partial class AddOperationViewModelTests
    {
        private AddOperationViewModel _addOperationViewModel;
        private bool _isDiscardActionPerformed = false;
        private CommandObserver _saveCommandObserver;

        [SetUp]
        public void SetUp()
        {
            _addOperationViewModel = new AddOperationViewModel(() => { _isDiscardActionPerformed = true; }, (x) => { });
            _saveCommandObserver = _addOperationViewModel.SaveCommand.GetDelegateCommandObserver();
        }


        public class PasswordRecordIsAdded : AddOperationViewModelTests
        {
            private bool _isCollectionModified = false;
            private int _initialNumberOfPasswordRecords;
            private CommandObserver _lastRecordRemoveCommandObserver;

            [SetUp]
            public void PasswordRecordIsAddedSetup()
            {
                _addOperationViewModel.Record.PasswordRecords.CollectionChanged += (sender, args) =>
                {
                    _isCollectionModified = true;
                };
                _initialNumberOfPasswordRecords = _addOperationViewModel.Record.PasswordRecords.Count;
                var passwordRecord = _addOperationViewModel.Record.PasswordRecords.ElementAt(_initialNumberOfPasswordRecords - 1);
                _lastRecordRemoveCommandObserver = passwordRecord.RemoveCommand.GetDelegateCommandObserver();
                passwordRecord.Name = "NewValue";
            }

            [Test]
            public void New_empty_password_record_is_added_that_can_not_be_deleted()
            {
                Assert.AreEqual(_initialNumberOfPasswordRecords + 1, _addOperationViewModel.Record.PasswordRecords.Count);
                var newlyAddedPaswordRecord = _addOperationViewModel.Record.PasswordRecords.ElementAt(_initialNumberOfPasswordRecords);
                Assert.AreEqual(string.Empty, newlyAddedPaswordRecord.Name);
                Assert.AreEqual(string.Empty, newlyAddedPaswordRecord.Value);
                Assert.False(newlyAddedPaswordRecord.RemoveCommand.CanExecute());
            }

            [Test]
            public void The_old_last_record_remove_command_becomes_enabled()
            {
                Assert.True(_lastRecordRemoveCommandObserver.ValueOfCanExecuteOnLatestEvent);
            }


        }

        //[Test]
        //public void When_file_is_added_then_correct_file_record_is_added()
        //{
        //    var file = @"D:\Test\One.pdf";
        //    _addOperationViewModel.Record.AddFileRecord(file);
        //    var correspondingFileRecord = _addOperationViewModel.Record.FileRecords.Last();
        //    Assert.Fail("Yet to be implemented");
        //    //Assert.AreEqual(correspondingFileRecord.Name);
        //}

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