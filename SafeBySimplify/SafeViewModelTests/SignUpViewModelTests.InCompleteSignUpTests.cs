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
                    "unset username",
                    new List<Action<SignUpViewModel>>
                    {
                        signUpViewModel => signUpViewModel.SignUpPassword = ValidPassword,
                        signUpViewModel => signUpViewModel.SignUpUserName = ValidUserName,
                    }, false, false);

                yield return new TestCaseData(
                    "unset password",
                    new List<Action<SignUpViewModel>>
                    {
                        signUpViewModel => signUpViewModel.SignUpUserName = ValidUserName,
                        signUpViewModel => signUpViewModel.SignUpConfirmPassword = ValidPassword,
                    }, false, false);

                yield return new TestCaseData(
                    "unset confirm password",
                    new List<Action<SignUpViewModel>>
                    {
                        signUpViewModel => signUpViewModel.SignUpUserName = ValidUserName,
                        signUpViewModel => signUpViewModel.SignUpPassword = ValidPassword,
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
                    "resetting username",
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