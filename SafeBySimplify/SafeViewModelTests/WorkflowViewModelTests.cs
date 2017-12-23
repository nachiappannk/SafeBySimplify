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


using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using SafeModel;
using SafeViewModel;
using SafeViewModelTests.TestTools;

namespace SafeViewModelTests
{
    [TestFixture]
    public class WorkflowViewModelTests
    {
        private string InitialWorkingDirectory = @"D:\TFS";
        WorkFlowViewModel _workFlowViewModel;
        private ISafeProvider _safeProvider;
        private ViewModelPropertyObserver<WorkFlowStepViewModel> _currentStepProperyObserver;

        [SetUp]
        public void SetUp()
        {
            _safeProvider = Substitute.For<ISafeProvider>();
            _safeProvider.WorkingDirectory.Returns(InitialWorkingDirectory);
            _safeProvider.GetUserNames().Returns(new List<string>() {"one", "two"});
            _workFlowViewModel = new WorkFlowViewModel(_safeProvider);

            _currentStepProperyObserver = _workFlowViewModel
                .GetPropertyObserver<WorkFlowStepViewModel>(nameof(_workFlowViewModel.CurrentStep));

        }




        [Test]
        public void First_step_is_entry_step()
        {
            Assert.AreEqual(typeof(EntryStepViewModel),_workFlowViewModel.CurrentStep.GetType());
        }


        [Test]
        public void When_in_entry_screen_and_go_to_settings_command_is_executed_then_the_app_moves_to_settings()
        {
            MoveFromEntryStepToSettingStep();
            Assert.AreEqual(typeof(SettingsStepViewModel), _currentStepProperyObserver.PropertyValue.GetType());
            Assert.AreNotEqual(0, _currentStepProperyObserver.NumberOfTimesPropertyChanged);
        }

        [Test]
        public void When_signed_up_with_correct_detail_then_operation_step_is_initialized_and_app_moves_operation_step_with_correct_username()
        {
            const string validUserName = "SomeUserName";
            const string validPassword = "SomePassword";

            var safe = Substitute.For<ISafe>();
            _safeProvider.CreateSafeForNonExistingUser(validUserName, validPassword, validPassword).Returns(safe);

            _safeProvider.StubUserNameValidity(validUserName, true, string.Empty);
            _safeProvider.StubPasswordNameValidity(validPassword, true, string.Empty);

            MoveFromEntryStepToOperationStepBySigningUp(validUserName, validPassword);
            Assert.AreEqual(typeof(OperationStepViewModel), _currentStepProperyObserver.PropertyValue.GetType());
            var operationStepViewModel = _currentStepProperyObserver.PropertyValue as OperationStepViewModel;
            Assert.AreEqual(validUserName, operationStepViewModel.UserName);
            // ReSharper disable once PossibleNullReferenceException
            Assert.AreEqual(safe, operationStepViewModel.Safe);
        }


        [Test]
        public void When_signed_in_with_correct_detail_then_operation_step_is_initialized_and_app_moves_operation_step_with_correct_username()
        {
            const string validUserName = "SomeUserName";
            const string validPassword = "SomePassword";

            var safe = Substitute.For<ISafe>();
            _safeProvider.StubCreateSafeForExistingUser(validUserName, validPassword, safe);

            AssumeInEntryStepAndThenGoToOperationsBySignIn(validUserName, validPassword);
            Assert.AreEqual(typeof(OperationStepViewModel), _currentStepProperyObserver.PropertyValue.GetType());
            var operationStepViewModel = _currentStepProperyObserver.PropertyValue as OperationStepViewModel;
            Assert.AreEqual(validUserName, operationStepViewModel.UserName);
            // ReSharper disable once PossibleNullReferenceException
            Assert.AreEqual(safe, operationStepViewModel.Safe);
        }




        [Test]
        public void When_in_settings_and_ok_command_is_made_then_the_app_moves_to_entry()
        {
            MoveFromEntryStepToSettingStep();
            _currentStepProperyObserver.ResetObserver();
            AssumeInSettingStepAndThenGoToEntryStep();
            Assert.AreEqual(typeof(EntryStepViewModel), _currentStepProperyObserver.PropertyValue.GetType());
            Assert.AreNotEqual(0, _currentStepProperyObserver.NumberOfTimesPropertyChanged);
        }





        [Test]
        public void When_in_operation_screen_and_logged_out_then_app_returns_to_entry_screen()
        {
            const string validUserName = "SomeUserName";
            const string validPassword = "SomePassword";

            var safe = Substitute.For<ISafe>();
            _safeProvider.CreateSafeForNonExistingUser(validUserName, validPassword, validPassword).Returns(safe);

            _safeProvider.StubUserNameValidity(validUserName, true, string.Empty);
            _safeProvider.StubPasswordNameValidity(validPassword, true, string.Empty);
            MoveFromEntryStepToOperationStepBySigningUp(validUserName, validPassword);


            var operationStepViewModel = _workFlowViewModel.CurrentStep as OperationStepViewModel;

            Assert.AreEqual(true,operationStepViewModel.SignOutCommand.CanExecute());
            operationStepViewModel.SignOutCommand.Execute();
            Assert.IsNull(operationStepViewModel.Safe);
            Assert.AreEqual(typeof(EntryStepViewModel),_currentStepProperyObserver.PropertyValue.GetType());

        }



        private void AssumeInSettingStepAndThenGoToEntryStep()
        {
            var settingsStepViewModel = _workFlowViewModel.CurrentStep as SettingsStepViewModel;
            Assume.That(settingsStepViewModel != null, "Not in setting step");
            // ReSharper disable once PossibleNullReferenceException
            var canExecute = settingsStepViewModel.OkCommand.CanExecute();
            Assume.That(canExecute, "Unable to command okay");
            settingsStepViewModel.OkCommand.Execute();
        }


        private void MoveFromEntryStepToSettingStep()
        {
            var entryStepViewModel = _workFlowViewModel.CurrentStep as EntryStepViewModel;
            Assume.That(entryStepViewModel != null, "Not in entry step");
            // ReSharper disable once PossibleNullReferenceException
            var canExecute = entryStepViewModel.GoToSettingsCommand.CanExecute();
            Assume.That(canExecute, "go to settings command is disabled");
            entryStepViewModel.GoToSettingsCommand.Execute();
        }

        private void AssumeInEntryStepAndThenGoToOperationsBySignIn(string validUserName, string validPassword)
        {
            var entryStepViewModel = _workFlowViewModel.CurrentStep as EntryStepViewModel;
            Assume.That(entryStepViewModel != null, "Not in entry step");
            // ReSharper disable once PossibleNullReferenceException
            var signInViewModel = entryStepViewModel.SignInViewModel;
            signInViewModel.SignInUserName = validUserName;
            signInViewModel.SignInPassword = validPassword;
            var canExecute = signInViewModel.SignInCommand.CanExecute();
            Assume.That(canExecute, "Unable to hit the signup button");
            signInViewModel.SignInCommand.Execute();
        }

        private void MoveFromEntryStepToOperationStepBySigningUp(string validUserName, string validPassword)
        {
            var entryStepViewModel = _workFlowViewModel.CurrentStep as EntryStepViewModel;
            Assume.That(entryStepViewModel != null, "Not in entry step");
            // ReSharper disable once PossibleNullReferenceException
            var signUpViewModel = entryStepViewModel.SignUpViewModel;
            signUpViewModel.SignUpUserName = validUserName;
            signUpViewModel.SignUpPassword = validPassword;
            signUpViewModel.SignUpConfirmPassword = validPassword;
            var canExecute = signUpViewModel.SignUpCommand.CanExecute();
            Assume.That(canExecute, "Unable to hit the signup button");
            signUpViewModel.SignUpCommand.Execute();
        }
    }
}
