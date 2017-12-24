using System;
using NSubstitute;
using NUnit.Framework;
using SafeModel;

namespace SafeViewModelTests
{
    public partial class AddOperationViewModelTests
    {
        public class ValidNameIsEntered : AddOperationViewModelTests
        {
            [SetUp]
            public void ValidNameIsEnteredSetUp()
            {
                _addOperationViewModel.Record.Name = "SomeName";
                Assume.That(_saveCommandObserver.ValueOfCanExecuteOnLatestEvent);
            }

            [Test]
            public void When_name_is_made_non_empty_and_then_empty_then_save_command_is_disabled()
            {
                _addOperationViewModel.Record.Name = String.Empty;
                Assert.False(_saveCommandObserver.ValueOfCanExecuteOnLatestEvent);
            }


            [Test]
            public void When_saved_the_record_is_written_in_safe_the_files_are_reoganized()
            {



                _addOperationViewModel.SaveCommand.Execute();
                _safe.Received(1).UpsertRecord(Arg.Any<Record>());
                Assert.AreEqual(_uniqueId, _idAtSaveAction);
            }
        }
    }
}