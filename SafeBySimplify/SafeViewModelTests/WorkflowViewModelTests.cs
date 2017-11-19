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
         
            var propertyChangedEventInfoFactory = _workFlowViewModel.GetPropertyChangedEventInfoFactory<WorkFlowStepViewModel>(nameof(_workFlowViewModel.CurrentStep));
            
            var currentStep = _workFlowViewModel.CurrentStep;
            var entryStepViewModel = currentStep as EntryStepViewModel;
            Assert.NotNull(entryStepViewModel, "Set Up error");
            var canExecute = entryStepViewModel.GoToSettingsCommand.CanExecute();
            Assert.AreEqual(true, canExecute, "Set Up error"+"Unable to go to settings");
            entryStepViewModel.GoToSettingsCommand.Execute();

            
            var propertyChangedEventInfo = propertyChangedEventInfoFactory.Invoke();
            Assert.AreEqual(typeof(SettingsStepViewModel), propertyChangedEventInfo.Value.GetType());
            Assert.AreEqual(true, propertyChangedEventInfo.EventReceived);
        }


        [Test]
        public void When_in_settings_and_ok_command_is_made_then_the_entry_screen_is_settings()
        {

            
            var entryStepViewModel = _workFlowViewModel.CurrentStep as EntryStepViewModel;
            Assert.NotNull(entryStepViewModel, "Set Up error");
            var canExecute = entryStepViewModel.GoToSettingsCommand.CanExecute();
            Assert.AreEqual(true, canExecute, "Set Up error" + "Unable to go to settings");
            entryStepViewModel.GoToSettingsCommand.Execute();
            var settingsStepViewModel = _workFlowViewModel.CurrentStep as SettingsStepViewModel;
            Assert.NotNull(settingsStepViewModel, "Set Up error");
            var canExecute1 = settingsStepViewModel.OkCommand.CanExecute();
            Assert.AreEqual(true, canExecute1, "Set Up error" + "Unable to go to entry");

            var propertyChangedEventInfoFactory = _workFlowViewModel.GetPropertyChangedEventInfoFactory<WorkFlowStepViewModel>(nameof(_workFlowViewModel.CurrentStep));


            settingsStepViewModel.OkCommand.Execute();
            var propertyChangedEventInfo = propertyChangedEventInfoFactory.Invoke();
            Assert.AreEqual(typeof(EntryStepViewModel), propertyChangedEventInfo.Value.GetType());
            Assert.AreEqual(true, propertyChangedEventInfo.EventReceived);
        }

        [Test]
        public void When_signed_up_with_correct_detail_then_safe_holder_is_initialized_and_app_moves_logged_in_mode()
        {
            var entryStepViewModel = _workFlowViewModel.CurrentStep as EntryStepViewModel;
            entryStepViewModel.SignUpUserName = "SomeUserName";
            entryStepViewModel.SignUpPassword = "SomePassword";
            entryStepViewModel.SignUpConfirmPassword = "SomePassword";
            var canExecute = entryStepViewModel.SignUpCommand.CanExecute();
            entryStepViewModel.SignUpCommand.Execute();
        }
    }



}
