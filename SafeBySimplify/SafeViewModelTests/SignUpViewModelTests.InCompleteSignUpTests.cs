﻿/*
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
        public class InCompleteSignUpTests : SignUpViewModelTests
        {
            [TestCaseSource(nameof(GetTestCaseData))]
            public void When_and_signup_paramters_are_empty_then_command_is_not_executable_and_error_message_is_empty
                (string message, List<Action<SignUpViewModel>> actions, bool isCommandChangeExpected, bool isCommandExecutable)
            {

                foreach (var action in actions)
                {
                    action.Invoke(SignUpViewModel);
                }

                CommandObserver.AssetAllSendersWereCorrect();
                if (isCommandChangeExpected) Assert.AreNotEqual(0, CommandObserver.NumberOfEventsRecieved);
                if (CommandObserver.NumberOfEventsRecieved > 0) Assert.AreEqual(isCommandExecutable, CommandObserver.ValueOfCanExecuteOnLatestEvent);
                if (ErrorMessagePropertyObserver.NumberOfTimesPropertyChanged > 0) Assert.True(String.IsNullOrWhiteSpace(ErrorMessagePropertyObserver.PropertyValue));
            }

            public static IEnumerable GetTestCaseData()
            {
                yield return new TestCaseData(
                    "unset confirm password 1",
                    new List<Action<SignUpViewModel>>
                    {
                        signUpViewModel => signUpViewModel.SignUpPassword = ValidPassword,
                        signUpViewModel => signUpViewModel.SignUpUserName = ValidUserName,
                    }, false, false);
                yield return new TestCaseData(
                    "unset confirm password 2",
                    new List<Action<SignUpViewModel>>
                    {
                        signUpViewModel => signUpViewModel.SignUpUserName = ValidUserName,
                        signUpViewModel => signUpViewModel.SignUpPassword = ValidPassword,
                    }, false, false);
                yield return new TestCaseData(
                    "unset password",
                    new List<Action<SignUpViewModel>>
                    {
                        signUpViewModel => signUpViewModel.SignUpUserName = ValidUserName,
                        signUpViewModel => signUpViewModel.SignUpConfirmPassword = ValidPassword,
                    }, false, false);
                yield return new TestCaseData(
                    "null userName",
                    new List<Action<SignUpViewModel>>
                    {
                        signUpViewModel => signUpViewModel.SignUpUserName = null,
                        signUpViewModel => signUpViewModel.SignUpPassword = ValidPassword,
                        signUpViewModel => signUpViewModel.SignUpConfirmPassword = ValidPassword,
                    }, false, false);
                yield return new TestCaseData(
                    "empty userName",
                    new List<Action<SignUpViewModel>>
                    {
                        signUpViewModel => signUpViewModel.SignUpUserName = String.Empty,
                        signUpViewModel => signUpViewModel.SignUpPassword = ValidPassword,
                        signUpViewModel => signUpViewModel.SignUpConfirmPassword = ValidPassword,
                    }, false, false);
                yield return new TestCaseData(
                    "resetting username as empty",
                    new List<Action<SignUpViewModel>>
                    {
                        signUpViewModel => signUpViewModel.SignUpUserName = ValidUserName,
                        signUpViewModel => signUpViewModel.SignUpPassword = ValidPassword,
                        signUpViewModel => signUpViewModel.SignUpConfirmPassword = ValidPassword,
                        signUpViewModel => signUpViewModel.SignUpUserName = String.Empty,
                    }, true, false);
            }
        }
    }
}