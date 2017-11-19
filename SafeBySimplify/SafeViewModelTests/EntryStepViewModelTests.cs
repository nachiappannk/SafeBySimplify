using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SafeViewModel;
using SafeViewModelTests.TestTools;

namespace SafeViewModelTests
{
    [TestFixture]
    public class EntryStepViewModelTests
    {
        #region ConfirmPasswordMismatchingCase

        [Test]
        public void When_all_signup_parameters_are_filled_and_both_passwords_dont_match_then_command_is_disabled_with_error_message()
        {
            EntryStepViewModel entryStepViewModel = new EntryStepViewModel();
            var commandObserver = entryStepViewModel.SignUpCommand.GetDelegateCommandObserver();
            var errorMessagePropertyObserver = entryStepViewModel.GetPropertyObserver<string>("ErrorMessage");

            entryStepViewModel.SignUpUserName = "SomeUserName";
            entryStepViewModel.SignUpPassword = "Password";
            entryStepViewModel.SignUpConfirmPassword = "ConfirmPassword";

            if(commandObserver.NumberOfEventsRecieved >0)commandObserver.AssetThereWasAtleastOnCanExecuteChangedEventAndCommandIsNotExecutable();
            errorMessagePropertyObserver.AssertProperyHasChanged(EntryStepViewModel.PasswordMismatchingErrorMessage);
        }

        #endregion

        #region NoErrorMessageCase
        [TestCaseSource(nameof(GetTestCaseData))]
        public void When_and_signup_paramters_are_empty_then_command_is_not_executable_and_error_message_is_empty
            (string message, List<Action<EntryStepViewModel>> actions, bool isCommandChangeExpected, bool isCommandExecutable)
        {
            EntryStepViewModel entryStepViewModel = new EntryStepViewModel();
            var commandObserver = entryStepViewModel.SignUpCommand.GetDelegateCommandObserver();
            var errorMessagePropertyObserver = entryStepViewModel.GetPropertyObserver<string>("ErrorMessage");

            foreach (var action in actions)
            {
                action.Invoke(entryStepViewModel);
            }

            commandObserver.AssetAllSendersWereCorrect();
            if (isCommandChangeExpected) Assert.AreNotEqual(0, commandObserver.NumberOfEventsRecieved);
            if (commandObserver.NumberOfEventsRecieved > 0) Assert.AreEqual(isCommandExecutable, commandObserver.ValueOfCanExecuteOnLatestEvent);

            if (errorMessagePropertyObserver.NumberOfTimesPropertyChanged > 0) Assert.True(string.IsNullOrWhiteSpace(errorMessagePropertyObserver.PropertyValue));
        }

        public static IEnumerable GetTestCaseData()
        {
            yield return new TestCaseData(
                "setting valid values for username, password and confirm password",
                new List<Action<EntryStepViewModel>>
                {
                    entryStepViewModel => entryStepViewModel.SignUpUserName = "SomeUserName",
                    entryStepViewModel => entryStepViewModel.SignUpPassword = "Password",
                    entryStepViewModel => entryStepViewModel.SignUpConfirmPassword = "Password",
                }, true, true);
            yield return new TestCaseData(
                "setting valid values for username, confirm password and password",
                new List<Action<EntryStepViewModel>>
                {
                    entryStepViewModel => entryStepViewModel.SignUpUserName = "SomeUserName",
                    entryStepViewModel => entryStepViewModel.SignUpConfirmPassword = "Password",
                    entryStepViewModel => entryStepViewModel.SignUpPassword = "Password",
                }, true, true);
            yield return new TestCaseData(
                "setting valid values for confirm password, password and userName",
                new List<Action<EntryStepViewModel>>
                {
                    entryStepViewModel => entryStepViewModel.SignUpConfirmPassword = "Password",
                    entryStepViewModel => entryStepViewModel.SignUpPassword = "Password",
                    entryStepViewModel => entryStepViewModel.SignUpUserName = "SomeUserName",
                }, true, true);

            yield return new TestCaseData(
                "unset username",
                new List<Action<EntryStepViewModel>>
                {
                    entryStepViewModel => entryStepViewModel.SignUpPassword = "Password",
                    entryStepViewModel => entryStepViewModel.SignUpUserName = "SomeUserName",
                }, false, false);

            yield return new TestCaseData(
                "unset password",
                new List<Action<EntryStepViewModel>>
                {
                    entryStepViewModel => entryStepViewModel.SignUpUserName = "UserName",
                    entryStepViewModel => entryStepViewModel.SignUpConfirmPassword = "SomeUserName",
                }, false, false);

            yield return new TestCaseData(
                "unset confirm password",
                new List<Action<EntryStepViewModel>>
                {
                    entryStepViewModel => entryStepViewModel.SignUpUserName = "UserName",
                    entryStepViewModel => entryStepViewModel.SignUpPassword = "SomeUserName",
                }, false, false);
            yield return new TestCaseData(
                "null userName",
                new List<Action<EntryStepViewModel>>
                {
                    entryStepViewModel => entryStepViewModel.SignUpUserName = null,
                    entryStepViewModel => entryStepViewModel.SignUpPassword = "SomePasword",
                    entryStepViewModel => entryStepViewModel.SignUpConfirmPassword = "SomePasword1",
                }, false, false);
            yield return new TestCaseData(
                "empty userName",
                new List<Action<EntryStepViewModel>>
                {
                    entryStepViewModel => entryStepViewModel.SignUpUserName = string.Empty,
                    entryStepViewModel => entryStepViewModel.SignUpPassword = "SomePasword",
                    entryStepViewModel => entryStepViewModel.SignUpConfirmPassword = "SomePasword",
                }, false, false);
            yield return new TestCaseData(
                "resetting username",
                new List<Action<EntryStepViewModel>>
                {
                    entryStepViewModel => entryStepViewModel.SignUpUserName = "someUserName",
                    entryStepViewModel => entryStepViewModel.SignUpPassword = "SomePasword",
                    entryStepViewModel => entryStepViewModel.SignUpConfirmPassword = "SomePasword",
                    entryStepViewModel => entryStepViewModel.SignUpUserName = string.Empty,
                }, true, false);
        }
        #endregion


    }
}