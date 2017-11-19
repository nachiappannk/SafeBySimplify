using System;
using System.Linq;
using NUnit.Framework;
using SafeViewModel;
using SafeViewModelTests.TestTools;

namespace SafeViewModelTests
{
    [TestFixture]
    public class EntryStepViewModelTests
    {
        [TestCase("useName","password", "password", true)]
        [TestCase("", "password", "password", false)]
        [TestCase("userName", "", "password", false)]
        [TestCase("useName", "password", "", false)]
        public void When_sign_up_user_name_is_empty_sign_up_disabled(string userName, string password, string confirmPassword, bool expected)
        {
            EntryStepViewModel entryStepViewModel = new EntryStepViewModel();
            var commandObserver = entryStepViewModel.SignUpCommand.GetDelegateCommandObserver();

            entryStepViewModel.SignUpUserName = userName;
            entryStepViewModel.SignUpPassword = password;
            entryStepViewModel.SignUpConfirmPassword = confirmPassword;

            Assert.AreEqual(expected, commandObserver.NumberOfEventsRecieved > 0);
            Assert.AreEqual(expected, commandObserver.ValueOfCanExecuteOnLatestEvent);
            commandObserver.AssetAllSendersWereCorrect();
        }

        public static void UserNameSetter(EntryStepViewModel viewModel, string value)
        {
            viewModel.SignUpUserName = value;
        }
    }
}