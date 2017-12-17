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
            
            _addOperationViewModel.Record.PasswordRecords.CollectionChanged += (sender, args) =>
                {
                    collectionModified = true;
                };

            var initialNumberOfPasswordRecords = _addOperationViewModel.Record.PasswordRecords.Count;
            var passwordRecord = _addOperationViewModel.Record.PasswordRecords.ElementAt(initialNumberOfPasswordRecords - 1);
            detailFiller.Invoke(passwordRecord);


            Assert.True(collectionModified);
            Assert.AreEqual(initialNumberOfPasswordRecords + 1, _addOperationViewModel.Record.PasswordRecords.Count);
            var newlyAddedPaswordRecord = _addOperationViewModel.Record.PasswordRecords.ElementAt(initialNumberOfPasswordRecords);
            Assert.AreEqual(string.Empty, newlyAddedPaswordRecord.Name);
            Assert.AreEqual(string.Empty, newlyAddedPaswordRecord.Value);

            var lastRecord = _addOperationViewModel.Record.PasswordRecords.Last();
            var otherRecords = _addOperationViewModel.Record.PasswordRecords.ToList();
            otherRecords.Remove(lastRecord);

            Assert.True(otherRecords.All(x => x.RemoveCommand.CanExecute()));
            Assert.False(lastRecord.RemoveCommand.CanExecute());


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