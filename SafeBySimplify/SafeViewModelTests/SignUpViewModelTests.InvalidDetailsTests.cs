using NSubstitute;
using NUnit.Framework;
using SafeViewModelTests.TestTools;

namespace SafeViewModelTests
{
    public partial class SignUpViewModelTests
    {
        public class InvalidDetailsTests : SignUpViewModelTests
        {
            public const string InvalidUserName = "%sss";
            public const string InvalidUserNameErrorMessage = "The user name is invalid";
            public const string InvalidPassword = "12345678";
            public const string InvalidPasswordErrorMessage = "The password is too weak";

            [Test]
            public void When_username_in_signup_form_then_command_is_disabled_with_error_message()
            {
                _safeProviderForNonExistingUser.StubUserNameValidity(InvalidUserName, false, InvalidUserNameErrorMessage);

                _signUpViewModel.SignUpUserName = InvalidUserName;
                _signUpViewModel.SignUpPassword = ValidPassword;
                _signUpViewModel.SignUpConfirmPassword = ValidPassword;

                if(_commandObserver.NumberOfEventsRecieved > 0)_commandObserver.AssertTheCommandBecameNonExecutable();
                Assert.AreEqual(false, _signUpViewModel.SignUpCommand.CanExecute());
                _errorMessagePropertyObserver.AssertProperyHasChanged(InvalidUserNameErrorMessage);
            }

            [Test]
            public void When_invalid_password_in_signup_form_then_command_is_disabled_with_error_message()
            {

                _safeProviderForNonExistingUser.StubPasswordNameValidity(InvalidPassword, false, InvalidPasswordErrorMessage);

                _signUpViewModel.SignUpUserName = ValidUserName;
                _signUpViewModel.SignUpPassword = InvalidPassword;
                _signUpViewModel.SignUpConfirmPassword = InvalidPassword;

                if (_commandObserver.NumberOfEventsRecieved > 0) _commandObserver.AssertTheCommandBecameNonExecutable();
                Assert.AreEqual(false, _signUpViewModel.SignUpCommand.CanExecute());
                _errorMessagePropertyObserver.AssertProperyHasChanged(InvalidPasswordErrorMessage);
            }
        }
    }
}