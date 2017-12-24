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
            public void When_username_in_signup_form_invalid_then_command_is_disabled_with_error_message()
            {
                SafeProviderForNonExistingUser.StubUserNameValidity(InvalidUserName, false, InvalidUserNameErrorMessage);

                SignUpViewModel.SignUpUserName = InvalidUserName;
                SignUpViewModel.SignUpPassword = ValidPassword;
                SignUpViewModel.SignUpConfirmPassword = ValidPassword;

                if(CommandObserver.NumberOfEventsRecieved > 0)CommandObserver.AssertTheCommandBecameNonExecutable();
                Assert.AreEqual(false, SignUpViewModel.SignUpCommand.CanExecute());
                ErrorMessagePropertyObserver.AssertProperyHasChanged(InvalidUserNameErrorMessage);
            }

            [Test]
            public void When_invalid_password_in_signup_form_is_invalid_then_command_is_disabled_with_error_message()
            {

                SafeProviderForNonExistingUser.StubPasswordNameValidity(InvalidPassword, false, InvalidPasswordErrorMessage);

                SignUpViewModel.SignUpUserName = ValidUserName;
                SignUpViewModel.SignUpPassword = InvalidPassword;
                SignUpViewModel.SignUpConfirmPassword = InvalidPassword;

                if (CommandObserver.NumberOfEventsRecieved > 0) CommandObserver.AssertTheCommandBecameNonExecutable();
                Assert.AreEqual(false, SignUpViewModel.SignUpCommand.CanExecute());
                ErrorMessagePropertyObserver.AssertProperyHasChanged(InvalidPasswordErrorMessage);
            }
        }
    }
}