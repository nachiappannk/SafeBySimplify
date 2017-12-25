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
        private IRecordIdGenerator _recordIdGenerator;
        private string _uniqueId = "SomeUniqueID";
        private ISafe _safe;
        private IFileIdGenerator _fileIdGenerator;

        [SetUp]
        public void SetUp()
        {
            _recordIdGenerator = Substitute.For<IRecordIdGenerator>();
            _fileIdGenerator = Substitute.For<IFileIdGenerator>();
            _recordIdGenerator.GetRecordId().Returns(_uniqueId);

            _safe = Substitute.For<ISafe>();

            _addOperationViewModel = new AddOperationViewModel(() => { _isDiscardActionPerformed = true; }, (x) =>
                {
                    _idAtSaveAction = x;
                }, 
                _recordIdGenerator,
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
    }
}