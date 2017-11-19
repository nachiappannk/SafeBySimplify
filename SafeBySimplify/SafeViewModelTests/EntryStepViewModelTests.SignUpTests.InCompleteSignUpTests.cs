using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SafeViewModel;
using SafeViewModelTests.TestTools;

namespace SafeViewModelTests
{
    public partial class EntryStepViewModelTests
    {
        public partial class SignUpTests
        {
            public class InCompleteSignUpTests : SignUpTests
            {
                [TestCaseSource(nameof(GetTestCaseData))]
                public void When_and_signup_paramters_are_empty_then_command_is_not_executable_and_error_message_is_empty
                    (string message, List<Action<EntryStepViewModel>> actions, bool isCommandChangeExpected, bool isCommandExecutable)
                {

                    foreach (var action in actions)
                    {
                        action.Invoke(_entryStepViewModel);
                    }

                    _commandObserver.AssetAllSendersWereCorrect();
                    if (isCommandChangeExpected) Assert.AreNotEqual(0, _commandObserver.NumberOfEventsRecieved);
                    if (_commandObserver.NumberOfEventsRecieved > 0) Assert.AreEqual(isCommandExecutable, _commandObserver.ValueOfCanExecuteOnLatestEvent);
                    if (_errorMessagePropertyObserver.NumberOfTimesPropertyChanged > 0) Assert.True(string.IsNullOrWhiteSpace(_errorMessagePropertyObserver.PropertyValue));
                }

                public static IEnumerable GetTestCaseData()
                {
                    yield return new TestCaseData(
                        "unset username",
                        new List<Action<EntryStepViewModel>>
                        {
                            entryStepViewModel => entryStepViewModel.SignUpPassword = ValidPassword,
                            entryStepViewModel => entryStepViewModel.SignUpUserName = ValidUserName,
                        }, false, false);

                    yield return new TestCaseData(
                        "unset password",
                        new List<Action<EntryStepViewModel>>
                        {
                            entryStepViewModel => entryStepViewModel.SignUpUserName = ValidUserName,
                            entryStepViewModel => entryStepViewModel.SignUpConfirmPassword = ValidPassword,
                        }, false, false);

                    yield return new TestCaseData(
                        "unset confirm password",
                        new List<Action<EntryStepViewModel>>
                        {
                            entryStepViewModel => entryStepViewModel.SignUpUserName = ValidUserName,
                            entryStepViewModel => entryStepViewModel.SignUpPassword = ValidPassword,
                        }, false, false);
                    yield return new TestCaseData(
                        "null userName",
                        new List<Action<EntryStepViewModel>>
                        {
                            entryStepViewModel => entryStepViewModel.SignUpUserName = null,
                            entryStepViewModel => entryStepViewModel.SignUpPassword = ValidPassword,
                            entryStepViewModel => entryStepViewModel.SignUpConfirmPassword = ValidPassword,
                        }, false, false);
                    yield return new TestCaseData(
                        "empty userName",
                        new List<Action<EntryStepViewModel>>
                        {
                            entryStepViewModel => entryStepViewModel.SignUpUserName = string.Empty,
                            entryStepViewModel => entryStepViewModel.SignUpPassword = ValidPassword,
                            entryStepViewModel => entryStepViewModel.SignUpConfirmPassword = ValidPassword,
                        }, false, false);
                    yield return new TestCaseData(
                        "resetting username",
                        new List<Action<EntryStepViewModel>>
                        {
                            entryStepViewModel => entryStepViewModel.SignUpUserName = ValidUserName,
                            entryStepViewModel => entryStepViewModel.SignUpPassword = ValidPassword,
                            entryStepViewModel => entryStepViewModel.SignUpConfirmPassword = ValidPassword,
                            entryStepViewModel => entryStepViewModel.SignUpUserName = string.Empty,
                        }, true, false);
                }
            }
        }
    }
}