using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using NSubstitute;
using NSubstitute.Core;
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

                var recordName = "RecordName";
                _addOperationViewModel.Record.Name = recordName;
                _addOperationViewModel.SaveCommand.Execute();

                _safe.Received(1).UpsertRecord(Arg.Is<Record>(x => x.Header.Id == _uniqueId));

                _safe.Received(1).ReoganizeFiles(_uniqueId);
                Assert.AreEqual(_uniqueId, _idAtSaveAction);
            }
        }
    }
}