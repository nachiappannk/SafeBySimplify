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

using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SafeViewModel;
using SafeViewModelTests.TestTools;

namespace SafeViewModelTests
{
    public partial class SignUpViewModelTests
    {
        public class PasswordsNonMatchingTests : SignUpViewModelTests
        {
            protected const string AnotherValidPassword = "Password1";

            public static IEnumerable GetErrorMessageTestCases()
            {
                yield return new TestCaseData(
                    new List<Action<SignUpViewModel>>
                    {
                        signUpViewModel => signUpViewModel.SignUpUserName = ValidUserName,
                        signUpViewModel => signUpViewModel.SignUpPassword = ValidPassword,
                        signUpViewModel => signUpViewModel.SignUpConfirmPassword = AnotherValidPassword,
                    }, SignUpViewModel.PasswordMismatchingErrorMessage, false);

                yield return new TestCaseData(
                    new List<Action<SignUpViewModel>>
                    {
                        signUpViewModel => signUpViewModel.SignUpConfirmPassword = ValidPassword,
                        signUpViewModel => signUpViewModel.SignUpUserName = ValidUserName,
                        signUpViewModel => signUpViewModel.SignUpPassword = ValidPassword,
                        signUpViewModel => signUpViewModel.SignUpConfirmPassword = AnotherValidPassword,
                    }, SignUpViewModel.PasswordMismatchingErrorMessage, false);

                yield return new TestCaseData(
                    new List<Action<SignUpViewModel>>
                    {
                        signUpViewModel => signUpViewModel.SignUpPassword = ValidPassword,
                        signUpViewModel => signUpViewModel.SignUpConfirmPassword = AnotherValidPassword,
                        signUpViewModel => signUpViewModel.SignUpUserName = ValidUserName,
                        signUpViewModel => signUpViewModel.SignUpConfirmPassword = ValidPassword,
                    }, String.Empty, true);
            }

            [TestCaseSource(nameof(GetErrorMessageTestCases))]
            public void When_all_signup_parameters_are_filled_and_both_passwords_dont_match_then_command_is_disabled_with_error_message
                (List<Action<SignUpViewModel>> actions, string errorMessage, bool commandExecutableState)
            {
                foreach (var action in actions)
                {
                    action.Invoke(SignUpViewModel);
                }

                if (CommandObserver.NumberOfEventsRecieved > 0)
                    CommandObserver.AssetThereWasAtleastOneCanExecuteChangedEventAndCommandExecutableStateIs(commandExecutableState);
                if (!String.IsNullOrWhiteSpace(errorMessage))
                    ErrorMessagePropertyObserver.AssertProperyHasChanged(errorMessage);
            }
        }
    }
}