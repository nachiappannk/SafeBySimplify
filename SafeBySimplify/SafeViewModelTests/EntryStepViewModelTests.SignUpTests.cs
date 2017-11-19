using NUnit.Framework;
using SafeViewModel;
using SafeViewModelTests.TestTools;

namespace SafeViewModelTests
{
    public partial class EntryStepViewModelTests
    {
        public partial class SignUpTests : EntryStepViewModelTests
        {
            protected const string ValidUserName = "SomeUserName";
            protected const string ValidPassword = "Password";


            protected EntryStepViewModel _entryStepViewModel;
            protected CommandObserver _commandObserver;
            protected ViewModelPropertyObserver<string> _errorMessagePropertyObserver;

            [SetUp]
            public void SetUp()
            {
                _entryStepViewModel = new EntryStepViewModel();
                _commandObserver = _entryStepViewModel.SignUpCommand.GetDelegateCommandObserver();
                _errorMessagePropertyObserver =
                    _entryStepViewModel.GetPropertyObserver<string>(nameof(_entryStepViewModel.SignUpErrorMessage));
            }
        }
    }
}