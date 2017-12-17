﻿using System.Linq;
using NUnit.Framework;

namespace SafeViewModelTests
{
    public partial class AddOperationViewModelTests
    {
        public class Tests : AddOperationViewModelTests
        {
            [Test]
            public void Initially_the_heading_is_empty()
            {
                Assert.AreEqual(string.Empty, _addOperationViewModel.Record.Name);
            }

            [Test]
            public void Initially_the_tags_is_empty()
            {
                CollectionAssert.IsEmpty(_addOperationViewModel.Record.Tags);
            }

            [Test]
            public void Initially_there_is_one_empty_password_record()
            {
                Assert.AreEqual(1, _addOperationViewModel.Record.PasswordRecords.Count);
                var firstPasswordRecord = _addOperationViewModel.Record.PasswordRecords.ElementAt(0);
                Assert.AreEqual(string.Empty, firstPasswordRecord.Name);
                Assert.AreEqual(string.Empty, firstPasswordRecord.Value);
            }

            [Test]
            public void Initially_there_is_no_file_record()
            {
                Assert.AreEqual(0, _addOperationViewModel.Record.FileRecords.Count);
            }

            [Test]
            public void Initially_discard_command_is_enabled()
            {
                Assert.True(_addOperationViewModel.DiscardCommand.CanExecute());
            }

            [Test]
            public void Initially_save_command_is_disabled()
            {
                Assert.False(_addOperationViewModel.SaveCommand.CanExecute());
            }

            [Test]
            public void When_discard_command_is_made_then_discard_action_is_performed()
            {
                _addOperationViewModel.DiscardCommand.Execute();
                Assert.True(_isDiscardActionPerformed);
            }

            [Test]
            public void When_name_is_non_empty_then_save_command_is_enabled()
            {
                _addOperationViewModel.Record.Name = "SomeName";
                Assert.True(_saveCommandObserver.ValueOfCanExecuteOnLatestEvent);
            }
        }
    }
}