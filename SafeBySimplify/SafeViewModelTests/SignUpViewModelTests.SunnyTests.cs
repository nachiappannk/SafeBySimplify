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
        public class SunnyTests : SignUpViewModelTests
        {
            [TestCaseSource(nameof(GetTestCaseData))]
            public void
                When_and_signup_paramters_are_empty_then_command_is_not_executable_and_error_message_is_empty
                (List<Action<SignUpViewModel>> actions)
            {

                foreach (var action in actions)
                {
                    action.Invoke(_signUpViewModel);
                }

                CommandAssertionExtentions.AssetAllSendersWereCorrect(_commandObserver);
                Assert.AreNotEqual(0, _commandObserver.NumberOfEventsRecieved);
                if (_commandObserver.NumberOfEventsRecieved > 0)
                    Assert.AreEqual(true, _commandObserver.ValueOfCanExecuteOnLatestEvent);
                if (_errorMessagePropertyObserver.NumberOfTimesPropertyChanged > 0)
                    Assert.True(String.IsNullOrWhiteSpace(_errorMessagePropertyObserver.PropertyValue));
            }

            public static IEnumerable GetTestCaseData()
            {
                yield return new TestCaseData(
                    new List<Action<SignUpViewModel>>
                    {
                        signUpViewModel => signUpViewModel.SignUpUserName = ValidUserName,
                        signUpViewModel => signUpViewModel.SignUpPassword = ValidPassword,
                        signUpViewModel => signUpViewModel.SignUpConfirmPassword = ValidPassword,
                    });
                yield return new TestCaseData(
                    new List<Action<SignUpViewModel>>
                    {
                        signUpViewModel => signUpViewModel.SignUpUserName = ValidUserName,
                        signUpViewModel => signUpViewModel.SignUpConfirmPassword = ValidPassword,
                        signUpViewModel => signUpViewModel.SignUpPassword = ValidPassword,
                    });
                yield return new TestCaseData(
                    new List<Action<SignUpViewModel>>
                    {
                        signUpViewModel => signUpViewModel.SignUpConfirmPassword = ValidPassword,
                        signUpViewModel => signUpViewModel.SignUpPassword = ValidPassword,
                        signUpViewModel => signUpViewModel.SignUpUserName = ValidUserName,
                    });

            }
        }
    }
}