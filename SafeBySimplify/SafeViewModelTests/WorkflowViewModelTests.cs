/*
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
    public partial class WorkflowViewModelTests
    {
        private string InitialWorkingDirectory = @"D:\TFS";
        WorkFlowViewModel _workFlowViewModel;
        private ISafeProvider _safeProvider;
        private ViewModelPropertyObserver<WorkFlowStepViewModel> _currentStepProperyObserver;
        private readonly List<string> _validUserNames = new List<string>() {"one", "two"};

        [SetUp]
        public void SetUp()
        {
            _safeProvider = Substitute.For<ISafeProvider>();
            _safeProvider.WorkingDirectory.Returns(InitialWorkingDirectory);
            _safeProvider.GetUserNames().Returns(_validUserNames);

            _workFlowViewModel = new WorkFlowViewModel(_safeProvider);

            _currentStepProperyObserver = _workFlowViewModel
                .GetPropertyObserver<WorkFlowStepViewModel>(nameof(_workFlowViewModel.CurrentStep));

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

        private void MoveFromEntryStepToOperationStepBySigningIn(string validUserName, string validPassword)
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
