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
            public class SunnyTests : SignUpTests
            {
                [TestCaseSource(nameof(GetTestCaseData))]
                public void
                    When_and_signup_paramters_are_empty_then_command_is_not_executable_and_error_message_is_empty
                    (List<Action<EntryStepViewModel>> actions)
                {

                    foreach (var action in actions)
                    {
                        action.Invoke(_entryStepViewModel);
                    }

                    _commandObserver.AssetAllSendersWereCorrect();
                    Assert.AreNotEqual(0, _commandObserver.NumberOfEventsRecieved);
                    if (_commandObserver.NumberOfEventsRecieved > 0)
                        Assert.AreEqual(true, _commandObserver.ValueOfCanExecuteOnLatestEvent);
                    if (_errorMessagePropertyObserver.NumberOfTimesPropertyChanged > 0)
                        Assert.True(string.IsNullOrWhiteSpace(_errorMessagePropertyObserver.PropertyValue));
                }

                public static IEnumerable GetTestCaseData()
                {
                    yield return new TestCaseData(
                        new List<Action<EntryStepViewModel>>
                        {
                            entryStepViewModel => entryStepViewModel.SignUpUserName = ValidUserName,
                            entryStepViewModel => entryStepViewModel.SignUpPassword = ValidPassword,
                            entryStepViewModel => entryStepViewModel.SignUpConfirmPassword = ValidPassword,
                        });
                    yield return new TestCaseData(
                        new List<Action<EntryStepViewModel>>
                        {
                            entryStepViewModel => entryStepViewModel.SignUpUserName = ValidUserName,
                            entryStepViewModel => entryStepViewModel.SignUpConfirmPassword = ValidPassword,
                            entryStepViewModel => entryStepViewModel.SignUpPassword = ValidPassword,
                        });
                    yield return new TestCaseData(
                        new List<Action<EntryStepViewModel>>
                        {
                            entryStepViewModel => entryStepViewModel.SignUpConfirmPassword = ValidPassword,
                            entryStepViewModel => entryStepViewModel.SignUpPassword = ValidPassword,
                            entryStepViewModel => entryStepViewModel.SignUpUserName = ValidUserName,
                        });

                }
            }
        }
    }
}