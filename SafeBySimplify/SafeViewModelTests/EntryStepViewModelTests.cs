using System;
using NUnit.Framework;
using SafeViewModel;

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
            var commandEventInfoFactory = entryStepViewModel.SignUpCommand.GetCommandEventInfoFactory();

            entryStepViewModel.SignUpUserName = userName;
            entryStepViewModel.SignUpPassword = password;
            entryStepViewModel.SignUpConfirmPassword = confirmPassword;

            var comand = commandEventInfoFactory.Invoke();
            Assert.AreEqual(expected,comand.WasEventReceived);
            Assert.AreEqual(expected, comand.ValueOfCanExecuteOnEvent);
            if(expected)Assert.True(comand.WasTheSenderCorrect);
            //Assert.AreEqual(expected, entryStepViewModel.SignUpCommand.CanExecute());
        }

        public static void UserNameSetter(EntryStepViewModel viewModel, string value)
        {
            viewModel.SignUpUserName = value;
        }
    }
}