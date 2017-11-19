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
        public partial class SignUpTests : EntryStepViewModelTests
        {
            public class PasswordsNonMatchingTests : SignUpTests
            {
                protected const string AnotherValidPassword = "Password1";

                public static IEnumerable GetErrorMessageTestCases()
                {
                    yield return new TestCaseData(
                        new List<Action<EntryStepViewModel>>
                        {
                            entryStepViewModel => entryStepViewModel.SignUpUserName = ValidUserName,
                            entryStepViewModel => entryStepViewModel.SignUpPassword = ValidPassword,
                            entryStepViewModel => entryStepViewModel.SignUpConfirmPassword = AnotherValidPassword,
                        }, EntryStepViewModel.PasswordMismatchingErrorMessage, false);

                    yield return new TestCaseData(
                        new List<Action<EntryStepViewModel>>
                        {
                            entryStepViewModel => entryStepViewModel.SignUpConfirmPassword = ValidPassword,
                            entryStepViewModel => entryStepViewModel.SignUpUserName = ValidUserName,
                            entryStepViewModel => entryStepViewModel.SignUpPassword = ValidPassword,
                            entryStepViewModel => entryStepViewModel.SignUpConfirmPassword = AnotherValidPassword,
                        }, EntryStepViewModel.PasswordMismatchingErrorMessage, false);

                    yield return new TestCaseData(
                        new List<Action<EntryStepViewModel>>
                        {
                            entryStepViewModel => entryStepViewModel.SignUpPassword = ValidPassword,
                            entryStepViewModel => entryStepViewModel.SignUpConfirmPassword = AnotherValidPassword,
                            entryStepViewModel => entryStepViewModel.SignUpUserName = ValidUserName,
                            entryStepViewModel => entryStepViewModel.SignUpConfirmPassword = ValidPassword,
                        }, string.Empty, true);
                }

                [TestCaseSource(nameof(GetErrorMessageTestCases))]
                public void When_all_signup_parameters_are_filled_and_both_passwords_dont_match_then_command_is_disabled_with_error_message
                    (List<Action<EntryStepViewModel>> actions, string errorMessage, bool commandExecutableState)
                {
                    foreach (var action in actions)
                    {
                        action.Invoke(_entryStepViewModel);
                    }

                    if (_commandObserver.NumberOfEventsRecieved > 0)
                        _commandObserver.AssetThereWasAtleastOneCanExecuteChangedEventAndCommandExecutableStateIs(commandExecutableState);
                    if (!string.IsNullOrWhiteSpace(errorMessage))
                        _errorMessagePropertyObserver.AssertProperyHasChanged(errorMessage);
                }

            }
        }
    }
}