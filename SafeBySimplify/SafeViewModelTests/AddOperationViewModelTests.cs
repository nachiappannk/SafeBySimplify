using System;
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

        [Test]
        public void When_user_name_are_filled_in_password_record_then_an_empty_password_record_is_added()
        {
            When_some_details_are_filled_in_password_record_then_an_empty_password_record_is_added(r =>
                r.Name = "SomeUserName");
        }


        [Test]
        public void When_password_are_filled_in_password_record_then_an_empty_password_record_is_added()
        {
            When_some_details_are_filled_in_password_record_then_an_empty_password_record_is_added(r =>
                r.Value = "SomeUserName");
        }


        public void When_some_details_are_filled_in_password_record_then_an_empty_password_record_is_added(Action<PasswordRecord> detailFiller)
        {
            var collectionModified = false;
            var initialNumberOfPasswordRecords = _addOperationViewModel.Record.PasswordRecords.Count;
            _addOperationViewModel.Record.PasswordRecords.CollectionChanged += (sender, args) =>
                {
                    collectionModified = true;
                };


            var passwordRecord = _addOperationViewModel.Record.PasswordRecords.ElementAt(initialNumberOfPasswordRecords - 1);
            detailFiller.Invoke(passwordRecord);
            Assert.True(collectionModified);
            Assert.AreEqual(initialNumberOfPasswordRecords + 1, _addOperationViewModel.Record.PasswordRecords.Count);
            var newlyAddedPaswordRecord = _addOperationViewModel.Record.PasswordRecords.ElementAt(initialNumberOfPasswordRecords);
            Assert.AreEqual(string.Empty, newlyAddedPaswordRecord.Name);
            Assert.AreEqual(string.Empty, newlyAddedPaswordRecord.Value);
        }
    }
}