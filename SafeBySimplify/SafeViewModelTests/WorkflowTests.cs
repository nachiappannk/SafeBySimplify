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
    }
}
