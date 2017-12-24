using System.Linq;
using NUnit.Framework;
using SafeViewModelTests.TestTools;

namespace SafeViewModelTests
{
    public partial class AddOperationViewModelTests
    {
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
    }
}