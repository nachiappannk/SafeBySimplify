using System;
using NUnit.Framework;

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
        }
    }
}