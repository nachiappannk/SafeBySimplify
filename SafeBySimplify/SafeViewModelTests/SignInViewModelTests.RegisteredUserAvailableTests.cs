/*
MIT License

Copyright(c) 2017 Nachiappan

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using SafeViewModel;
using SafeViewModelTests.TestTools;

namespace SafeViewModelTests
{
    public partial class SignInViewModelTests
    {
        public class RegisteredUserAvailableTests : SignInViewModelTests
        {
            private List<string> _registeredUserNames;
            private CommandObserver _commandObserver;
            private ViewModelPropertyObserver<string> _errorMessagePropertyObserver;

            [SetUp]
            public void RegisteredUserAvailableTestsSetUp()
            {
                _registeredUserNames = new List<string> {"UserName1", "UserName2", "UserName3",};
                _safeProviderForExistingUser.GetUserNames().Returns(_registeredUserNames);
                SignInViewModel = new SignInViewModel(_safeProviderForExistingUser, (safe, n) => { });
                _commandObserver = SignInViewModel.SignInCommand.GetDelegateCommandObserver();
                _errorMessagePropertyObserver = SignInViewModel.GetPropertyObserver<string>(nameof(SignInViewModel.ErrorMessage));

            }

            [Test]
            public void When_there_are_user_accounts_then_sign_in_is_enabled()
            {
                Assert.AreEqual(true, SignInViewModel.IsEnabled);
            }

            [Test]
            public void When_there_are_user_accounts_then_same_usernames_are_available_for_login()
            {
                CollectionAssert.AreEquivalent(_registeredUserNames, SignInViewModel.AvailableUserNames);
            }

            [Test]
            public void When_there_are_user_accounts_then_first_user_name_is_available_for_login()
            {
                Assert.AreEqual(_registeredUserNames.ElementAt(0), SignInViewModel.SignInUserName);
            }

            [Test]
            public void At_the_begining_password_is_empty()
            {
                Assert.AreEqual(string.Empty, SignInViewModel.SignInPassword);
            }

            [Test]
            public void When_password_is_entered_then_sign_in_command_is_enabled()
            {
                SignInViewModel.SignInPassword = "SomePassword";
                Assert.True(_commandObserver.ValueOfCanExecuteOnLatestEvent);
            }

            [Test]
            public void When_password_is_removed_then_sign_in_command_is_disabled()
            {
                SignInViewModel.SignInPassword = "SomePassword";
                SignInViewModel.SignInPassword = "";
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
                SignInViewModel.SignInPassword = "Password";
                SignInViewModel.SignInUserName = SignInViewModel.AvailableUserNames.ElementAt(1);

                Assert.AreEqual(string.Empty,SignInViewModel.SignInPassword);
            }

            [Test]
            public void When_logged_with_wrong_password_and_modified_password_then_no_error_message_is_shown()
            {
                SignInWithPassword("WrongPassword");

                Assume.That(SignInViewModel.WrongPasswordErrorMessage == _errorMessagePropertyObserver.PropertyValue);
                SignInViewModel.SignInPassword = "SomeOtherPassword";
                Assert.AreEqual(string.Empty, _errorMessagePropertyObserver.PropertyValue);
            }

            [Test]
            public void When_logged_with_wrong_password_and_modified_username_then_no_error_message_is_shown()
            {
                SignInWithPassword("WrongPassword");

                Assume.That(SignInViewModel.WrongPasswordErrorMessage == _errorMessagePropertyObserver.PropertyValue);
                SignInViewModel.SignInUserName = SignInViewModel.AvailableUserNames.ElementAt(1);
                Assert.AreEqual(string.Empty, _errorMessagePropertyObserver.PropertyValue);
            }

            private void SignInWithPassword(string signInPassword)
            {
                SignInViewModel.SignInPassword = signInPassword;
                Assume.That(SignInViewModel.SignInCommand.CanExecute());
                SignInViewModel.SignInCommand.Execute();
            }
        }
    }
}