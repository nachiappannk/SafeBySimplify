using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using SafeModel;
using SafeViewModel;

namespace SafeViewModelTests
{
    [TestFixture]
    public class WorkflowTests
    {
        WorkFlowViewModel _workFlowViewModel;
        private ISafe _safe;
        private string InitialWorkingDirectory = @"D:\TFS";

        [SetUp]
        public void SetUp()
        {
            _safe = Substitute.For<ISafe>();
            _safe.WorkingDirectory.Returns(InitialWorkingDirectory);
            _workFlowViewModel = new WorkFlowViewModel(_safe);
        }

        [Test]
        public void First_step_is_the_entry_screen_with_signin_and_signup_options()
        {
            Assert.AreEqual(typeof(EntryStepViewModel),_workFlowViewModel.CurrentStep.GetType());
        }

        

        [Test]
        public void When_in_entry_screen_and_asked_to_go_to_settings_then_the_current_screen_is_settings()
        {
            var eventRecieved = false;
            _workFlowViewModel.PropertyChanged += (obj, e) =>
            {
                Assert.AreEqual(_workFlowViewModel, obj);
                Assert.AreEqual(nameof(_workFlowViewModel.CurrentStep),e.PropertyName);
                var newCurrentStep = _workFlowViewModel.CurrentStep;
                Assert.AreEqual(typeof(SettingsStepViewModel), newCurrentStep.GetType());
                eventRecieved = true;
            };

            var currentStep = _workFlowViewModel.CurrentStep;
            var entryStepViewModel = currentStep as EntryStepViewModel;
            Assert.NotNull(entryStepViewModel, "Set Up error");
            var canExecute = entryStepViewModel.GoToSettingsCommand.CanExecute();
            Assert.AreEqual(true, canExecute, "Unable to go to settings");
            entryStepViewModel.GoToSettingsCommand.Execute();

            Assert.AreEqual(true, eventRecieved);
        }

        [Test]
        public void On_going_to_setting_screen_the_correct_workspace_is_shown_in_the_setting()
        {
            var currentStep = _workFlowViewModel.CurrentStep;
            var entryStepViewModel = currentStep as EntryStepViewModel;
            Assert.NotNull(entryStepViewModel, "Set Up error");
            var canExecute = entryStepViewModel.GoToSettingsCommand.CanExecute();
            Assert.AreEqual(true, canExecute, "Unable to go to settings");
            entryStepViewModel.GoToSettingsCommand.Execute();

            var currentStep1 = _workFlowViewModel.CurrentStep;
            var settingStepViewModel = currentStep1 as SettingsStepViewModel;
            Assert.NotNull(settingStepViewModel, "Set Up error");
            var workSpaceDirectory = settingStepViewModel.WorkSpaceDirectory;
            Assert.AreEqual(InitialWorkingDirectory,workSpaceDirectory);

        }

    }


}
