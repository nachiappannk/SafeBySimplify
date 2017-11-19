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
                    action.Invoke(_signUpViewModel);
                }

                if (_commandObserver.NumberOfEventsRecieved > 0)
                    CommandAssertionExtentions.AssetThereWasAtleastOneCanExecuteChangedEventAndCommandExecutableStateIs(_commandObserver, commandExecutableState);
                if (!String.IsNullOrWhiteSpace(errorMessage))
                    PropertyChangedEventExtentions.AssertProperyHasChanged(_errorMessagePropertyObserver, errorMessage);
            }

        }
    }
}