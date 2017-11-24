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

        [SetUp]
        public void SetUp()
        {
            _safeProvider = Substitute.For<ISafeProvider>();
            _safeProvider.WorkingDirectory.Returns(InitialWorkingDirectory);
            _workFlowViewModel = new WorkFlowViewModel(_safeProvider);
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

            var currentStepProperyObserver = _workFlowViewModel.GetPropertyObserver<WorkFlowStepViewModel>(nameof(_workFlowViewModel.CurrentStep));

            var currentStep = _workFlowViewModel.CurrentStep;
            var entryStepViewModel = currentStep as EntryStepViewModel;
            Assert.NotNull(entryStepViewModel, "Set Up error");
            var canExecute = entryStepViewModel.GoToSettingsCommand.CanExecute();
            Assert.AreEqual(true, canExecute, "Set Up error" + "Unable to go to settings");
            entryStepViewModel.GoToSettingsCommand.Execute();

            Assert.AreEqual(typeof(SettingsStepViewModel), currentStepProperyObserver.PropertyValue.GetType());
            Assert.AreNotEqual(0, currentStepProperyObserver.NumberOfTimesPropertyChanged);
        }



        [Test]
        public void When_in_settings_and_ok_command_is_made_then_the_entry_screen_is_settings()
        {

            var currentStepPropertyObserver = _workFlowViewModel.GetPropertyObserver<WorkFlowStepViewModel>(nameof(_workFlowViewModel.CurrentStep));

            var entryStepViewModel = _workFlowViewModel.CurrentStep as EntryStepViewModel;
            Assert.NotNull(entryStepViewModel, "Set Up error");
            var canExecute = entryStepViewModel.GoToSettingsCommand.CanExecute();
            Assert.AreEqual(true, canExecute, "Set Up error" + "Unable to go to settings");
            entryStepViewModel.GoToSettingsCommand.Execute();
            var settingsStepViewModel = _workFlowViewModel.CurrentStep as SettingsStepViewModel;
            Assert.NotNull(settingsStepViewModel, "Set Up error");
            var canExecute1 = settingsStepViewModel.OkCommand.CanExecute();
            Assert.AreEqual(true, canExecute1, "Set Up error" + "Unable to go to entry");

            currentStepPropertyObserver.ResetObserver();

            settingsStepViewModel.OkCommand.Execute();
            
            Assert.AreEqual(typeof(EntryStepViewModel), currentStepPropertyObserver.PropertyValue.GetType());
            Assert.AreNotEqual(0, currentStepPropertyObserver.NumberOfTimesPropertyChanged);
        }

        [Test]
        public void When_signed_up_with_correct_detail_then_safe_holder_is_initialized_and_app_moves_logged_in_mode()
        {
            const string validUserName = "SomeUserName";
            const string validPassword = "SomePassword";

            var safe = Substitute.For<ISafe>();
            _safeProvider.CreateSafe(validUserName, validPassword).Returns(safe);
            var errorMessage = string.Empty;
            

            var currentStepPropertyObserver = _workFlowViewModel.GetPropertyObserver<WorkFlowStepViewModel>("CurrentStep");

            var entryStepViewModel = _workFlowViewModel.CurrentStep as EntryStepViewModel;
            entryStepViewModel.SignUpViewModel.SignUpUserName = validUserName;
            entryStepViewModel.SignUpViewModel.SignUpPassword = validPassword;
            entryStepViewModel.SignUpViewModel.SignUpConfirmPassword = validPassword;
            var canExecute = entryStepViewModel.SignUpViewModel.SignUpCommand.CanExecute();
            entryStepViewModel.SignUpViewModel.SignUpCommand.Execute();

            Assert.AreEqual(typeof(OperationStepViewModel),currentStepPropertyObserver.PropertyValue.GetType());
            var operationStepViewModel = currentStepPropertyObserver.PropertyValue as OperationStepViewModel;
            Assert.AreEqual(safe, operationStepViewModel.Safe);
        }
    }



}
