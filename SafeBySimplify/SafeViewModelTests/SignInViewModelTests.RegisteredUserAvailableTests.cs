using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using SafeModel;
using SafeViewModel;
using SafeViewModelTests.TestTools;

namespace SafeViewModelTests
{
    public partial class SignInViewModelTests
    {
        public partial class RegisteredUserAvailableTests : SignInViewModelTests
        {
            private List<string> _registeredUserNames;
            private CommandObserver _commandObserver;
            private ViewModelPropertyObserver<string> _errorMessagePropertyObserver;

            [SetUp]
            public void SetUp()
            {
                _registeredUserNames = new List<string> {"UserName1", "UserName2", "UserName3",};
                _safeProviderForExistingUser.GetUserNames().Returns(_registeredUserNames);
                _signInViewModel = new SignInViewModel(_safeProviderForExistingUser);
                _commandObserver = _signInViewModel.SignInCommand.GetDelegateCommandObserver();
                _errorMessagePropertyObserver = _signInViewModel.GetPropertyObserver<string>(nameof(_signInViewModel.ErrorMessage));

            }

            [Test]
            public void When_there_are_user_accounts_then_sign_in_is_enabled()
            {
                Assert.AreEqual(true, _signInViewModel.IsEnabled);
            }

            [Test]
            public void When_there_are_user_accounts_then_same_usernames_are_available_for_login()
            {
                CollectionAssert.AreEquivalent(_registeredUserNames, _signInViewModel.AvailableUserNames);
            }

            [Test]
            public void When_there_are_user_accounts_then_first_user_name_is_available_for_login()
            {
                Assert.AreEqual(_registeredUserNames.ElementAt(0), _signInViewModel.SignInUserName);
            }

            [Test]
            public void At_the_begining_password_is_empty()
            {
                Assert.AreEqual(string.Empty, _signInViewModel.SignInPassword);
            }

            [Test]
            public void When_password_is_entered_then_sign_in_command_is_enabled()
            {
                _signInViewModel.SignInPassword = "SomePassword";
                Assert.True(_commandObserver.ValueOfCanExecuteOnLatestEvent);
            }

            [Test]
            public void When_password_is_removed_then_sign_in_command_is_disabled()
            {
                _signInViewModel.SignInPassword = "SomePassword";
                _signInViewModel.SignInPassword = "";
                Assert.False(_commandObserver.ValueOfCanExecuteOnLatestEvent);
            }

            [Test]
            public void When_logged_with_wrong_password_then_error_message_is_shown()
            {
                SignInWithPassword("WrongPassword");

                Assert.AreEqual(SignInViewModel.WrongPasswordErrorMessage, _errorMessagePropertyObserver.PropertyValue);
            }

            [Test]
            public void When_user_name_is_modified_password_is_cleared()
            {
                _signInViewModel.SignInPassword = "Password";
                _signInViewModel.SignInUserName = _signInViewModel.AvailableUserNames.ElementAt(1);

                Assert.AreEqual(string.Empty,_signInViewModel.SignInPassword);
            }

            [Test]
            public void When_logged_with_wrong_password_and_modified_password_then_no_error_message_is_shown()
            {
                SignInWithPassword("WrongPassword");

                Assume.That(SignInViewModel.WrongPasswordErrorMessage == _errorMessagePropertyObserver.PropertyValue);
                _signInViewModel.SignInPassword = "SomeOtherPassword";
                Assert.AreEqual(string.Empty, _errorMessagePropertyObserver.PropertyValue);
            }

            [Test]
            public void When_logged_with_wrong_password_and_modified_username_then_no_error_message_is_shown()
            {
                SignInWithPassword("WrongPassword");

                Assume.That(SignInViewModel.WrongPasswordErrorMessage == _errorMessagePropertyObserver.PropertyValue);
                _signInViewModel.SignInUserName = _signInViewModel.AvailableUserNames.ElementAt(1);
                Assert.AreEqual(string.Empty, _errorMessagePropertyObserver.PropertyValue);
            }

            private void SignInWithPassword(string signInPassword)
            {
                _signInViewModel.SignInPassword = signInPassword;
                Assume.That(_signInViewModel.SignInCommand.CanExecute());
                _signInViewModel.SignInCommand.Execute();
            }
        }
    }
}