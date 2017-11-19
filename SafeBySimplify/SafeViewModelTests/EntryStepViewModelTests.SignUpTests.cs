using NSubstitute;
using NUnit.Framework;
using SafeModel;
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

            protected ISafeProviderForNonExistingUser _safeProviderForNonExistingUser;

            [SetUp]
            public void SetUp()
            {
                _safeProviderForNonExistingUser = CreateSafeProviderForNonExistingUser();
                _entryStepViewModel = new EntryStepViewModel(_safeProviderForNonExistingUser);
                _commandObserver = _entryStepViewModel.SignUpCommand.GetDelegateCommandObserver();
                _errorMessagePropertyObserver =
                    _entryStepViewModel.GetPropertyObserver<string>(nameof(_entryStepViewModel.SignUpErrorMessage));
            }

            private static ISafeProviderForNonExistingUser CreateSafeProviderForNonExistingUser()
            {
                var safeProviderForNonExistingUser = Substitute.For<ISafeProviderForNonExistingUser>();

                string value = "";
                safeProviderForNonExistingUser.IsUserNameValidForNonExistingUser(ValidUserName, out value)
                    .Returns(x =>
                    {
                        x[1] = string.Empty;
                        return true;
                    });
                return safeProviderForNonExistingUser;
            }

            public class InvalidDetailsTests : SignUpTests
            {
                public const string InvalidUserName = "%sss";
                public const string InvalidUserNameErrorMessage = "The user name is invalid";
                [Test]
                public void When_invalid_details_are_in_signup_form_then_command_is_disabled_with_error_message()
                {
                    string value = "";
                    _safeProviderForNonExistingUser.IsUserNameValidForNonExistingUser(InvalidUserName, out value)
                        .Returns(x =>
                        {
                            x[1] = InvalidUserNameErrorMessage;
                            return false;
                        });


                    _entryStepViewModel.SignUpUserName = InvalidUserName;
                    _entryStepViewModel.SignUpPassword = ValidPassword;
                    _entryStepViewModel.SignUpConfirmPassword = ValidPassword;

                    if(_commandObserver.NumberOfEventsRecieved > 0)_commandObserver.AssetThereWasAtleastOneCanExecuteChangedEventAndCommandIsNotExecutable();
                    Assert.AreEqual(false, _entryStepViewModel.SignUpCommand.CanExecute());
                }
            }
        }
    }
}