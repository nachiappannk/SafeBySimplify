using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _workFlowViewModel = new WorkFlowViewModel(_safeProvider);

            _currentStepProperyObserver = _workFlowViewModel
                .GetPropertyObserver<WorkFlowStepViewModel>(nameof(_workFlowViewModel.CurrentStep));

        }

        [Test]
        public void First_step_is_the_entry_screen_with_signin_and_signup_options()
        {
            Assert.AreEqual(typeof(EntryStepViewModel),_workFlowViewModel.CurrentStep.GetType());
            Assert.True(_workFlowViewModel.CurrentStep.IsActive);
        }


        [Test]
        public void When_in_entry_screen_and_asked_to_go_to_settings_then_the_current_screen_is_settings()
        {
            AssumeInEntryStepAndThenGoToSettings();
            Assert.AreEqual(typeof(SettingsStepViewModel), _currentStepProperyObserver.PropertyValue.GetType());
            Assert.AreNotEqual(0, _currentStepProperyObserver.NumberOfTimesPropertyChanged);
        }

        [Test]
        public void When_in_settings_and_ok_command_is_made_then_the_entry_screen_is_settings()
        {
            AssumeInEntryStepAndThenGoToSettings();
            _currentStepProperyObserver.ResetObserver();
            AssumeInSettingStepAndThenGoToEntryStep();
            Assert.AreEqual(typeof(EntryStepViewModel), _currentStepProperyObserver.PropertyValue.GetType());
            Assert.AreNotEqual(0, _currentStepProperyObserver.NumberOfTimesPropertyChanged);
        }

        private void AssumeInSettingStepAndThenGoToEntryStep()
        {
            var settingsStepViewModel = _workFlowViewModel.CurrentStep as SettingsStepViewModel;
            Assume.That(settingsStepViewModel != null, "Not in setting step");
            var canExecute = settingsStepViewModel.OkCommand.CanExecute();
            Assume.That(canExecute, "Unable to command okay");
            settingsStepViewModel.OkCommand.Execute();
        }


        private void AssumeInEntryStepAndThenGoToSettings()
        {
            var entryStepViewModel = _workFlowViewModel.CurrentStep as EntryStepViewModel;
            Assume.That(entryStepViewModel != null, "Not in entry step");
            var canExecute = entryStepViewModel.GoToSettingsCommand.CanExecute();
            Assume.That(canExecute, "go to settings command is disabled");
            entryStepViewModel.GoToSettingsCommand.Execute();
        }

        [Test]
        public void When_signed_up_with_correct_detail_then_safe_holder_is_initialized_and_app_moves_logged_in_mode()
        {
            const string validUserName = "SomeUserName";
            const string validPassword = "SomePassword";

            var safe = Substitute.For<ISafe>();
            _safeProvider.CreateSafe(validUserName, validPassword).Returns(safe);
            string errorMessage = String.Empty;

            _safeProvider.StubUserNameValidity(validUserName, true, string.Empty);
            _safeProvider.StubPasswordNameValidity(validPassword, true, string.Empty);

            AssumeInEntryStepAndThenGoToOperationsBySignUp(validUserName, validPassword);
            Assert.AreEqual(typeof(OperationStepViewModel),_currentStepProperyObserver.PropertyValue.GetType());
            var operationStepViewModel = _currentStepProperyObserver.PropertyValue as OperationStepViewModel;
            Assert.AreEqual(safe, operationStepViewModel.Safe);
        }

        private void AssumeInEntryStepAndThenGoToOperationsBySignUp(string validUserName, string validPassword)
        {
            var entryStepViewModel = _workFlowViewModel.CurrentStep as EntryStepViewModel;
            Assume.That(entryStepViewModel != null, "Not in entry step");
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
