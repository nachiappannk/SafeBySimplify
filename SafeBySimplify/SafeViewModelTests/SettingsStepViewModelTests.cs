using System.Windows.Input;
using NSubstitute;
using NUnit.Framework;
using SafeModel;
using SafeViewModel;
using SafeViewModelTests.TestTools;

namespace SafeViewModelTests
{
    [TestFixture]
    public class SettingsStepViewModelTests
    {
        private string InitialWorkingDirectory = @"D:\TFS";
        private SettingsStepViewModel _settingsStepViewModel;
        private IHasWorkingDirectory _hasWorkingDirectory;

        [SetUp]
        public void SetUp()
        {
            _hasWorkingDirectory = Substitute.For<IHasWorkingDirectory>();
            _hasWorkingDirectory.WorkingDirectory = InitialWorkingDirectory;
            
            _settingsStepViewModel = new SettingsStepViewModel(_hasWorkingDirectory);
            _settingsStepViewModel.OnEntry();
        }

        [Test]
        public void On_going_to_setting_screen_the_correct_workspace_is_shown_in_the_setting()
        {
            Assert.AreEqual(InitialWorkingDirectory, _settingsStepViewModel.WorkSpaceDirectory);
        }

        [Test]
        public void On_going_to_settings_the_commands_are_in_unmodified_state()
        {
            AssetTheCommandsAreInUnmodifiedState();
        }

        [Test]
        public void When_changing_the_workspace_the_commands_change_to_modified_state()
        {
            var saveCommandObserver = _settingsStepViewModel.SaveCommand.GetDelegateCommandObserver();
            var discardCommandObserver = _settingsStepViewModel.DiscardCommand.GetDelegateCommandObserver();
            var okayCommandObserver = _settingsStepViewModel.OkCommand.GetDelegateCommandObserver();
            _settingsStepViewModel.WorkSpaceDirectory = @"D:\TFS_New";
            saveCommandObserver.AssetThereWasAtleastOnCanExecuteChangedEventAndCommandIsExecutable();
            discardCommandObserver.AssetThereWasAtleastOnCanExecuteChangedEventAndCommandIsExecutable();
            okayCommandObserver.AssetThereWasAtleastOnCanExecuteChangedEventAndCommandIsNotExecutable();
        }

        [Test]
        public void When_changing_workspace_and_discarding_we_go_to_initial_state_and_workspace_is_with_old_value_and_no_write_is_made_to_lib()
        {
            var initalValue = _settingsStepViewModel.WorkSpaceDirectory;
            _hasWorkingDirectory.ClearReceivedCalls();
            var saveCommandObserver = _settingsStepViewModel.SaveCommand.GetDelegateCommandObserver();
            var discardCommandObserver = _settingsStepViewModel.DiscardCommand.GetDelegateCommandObserver();
            var okCommandObserver = _settingsStepViewModel.OkCommand.GetDelegateCommandObserver();
            var workSpaceDirectoryObserver = _settingsStepViewModel.GetPropertyObserver<string>(nameof(_settingsStepViewModel.WorkSpaceDirectory));


            _settingsStepViewModel.WorkSpaceDirectory = @"D:\TFS_New";



            Assume.That(_settingsStepViewModel.DiscardCommand.CanExecute());
            _settingsStepViewModel.DiscardCommand.Execute();
            _hasWorkingDirectory.DidNotReceive().WorkingDirectory = Arg.Any<string>();

            workSpaceDirectoryObserver.AssertProperyHasChanged(initalValue);


            Assert.AreEqual(initalValue, _settingsStepViewModel.WorkSpaceDirectory);

            saveCommandObserver.AssetThereWasAtleastOnCanExecuteChangedEventAndCommandIsNotExecutable();
            discardCommandObserver.AssetThereWasAtleastOnCanExecuteChangedEventAndCommandIsNotExecutable();
            okCommandObserver.AssetThereWasAtleastOnCanExecuteChangedEventAndCommandIsExecutable();
        }


        [Test]
        public void When_changing_workspace_and_saving_we_go_to_initial_state_and_workspace_is_the_new_value_and_write_is_made_to_lib()
        {
            var initalValue = _settingsStepViewModel.WorkSpaceDirectory;
            _hasWorkingDirectory.ClearReceivedCalls();
            var saveCommandObserver = _settingsStepViewModel.SaveCommand.GetDelegateCommandObserver();
            var discardCommandObserver = _settingsStepViewModel.DiscardCommand.GetDelegateCommandObserver();
            var okCommandObserver = _settingsStepViewModel.OkCommand.GetDelegateCommandObserver();
            var propertyChangedEventInfoFactory = _settingsStepViewModel.GetPropertyObserver<string>(nameof(_settingsStepViewModel.WorkSpaceDirectory));


            var newValueOfWorkSpaceDirectory = @"D:\TFS_New";
            _settingsStepViewModel.WorkSpaceDirectory = newValueOfWorkSpaceDirectory;



            Assume.That(_settingsStepViewModel.SaveCommand.CanExecute());
            _settingsStepViewModel.SaveCommand.Execute();
            _hasWorkingDirectory.Received().WorkingDirectory = newValueOfWorkSpaceDirectory;

            Assert.AreEqual(newValueOfWorkSpaceDirectory, _settingsStepViewModel.WorkSpaceDirectory);


            saveCommandObserver.AssetThereWasAtleastOnCanExecuteChangedEventAndCommandIsNotExecutable();
            discardCommandObserver.AssetThereWasAtleastOnCanExecuteChangedEventAndCommandIsNotExecutable();
            okCommandObserver.AssetThereWasAtleastOnCanExecuteChangedEventAndCommandIsExecutable();
        }



        [Test, Ignore("Feature not developed")]
        public void When_changing_the_workspace_and_discarding_changes_working_directory_is_same_as_initial()
        {
            

            _settingsStepViewModel.WorkSpaceDirectory = @"D:\TFS_New";

            Assume.That(_settingsStepViewModel.DiscardCommand.CanExecute());
            _settingsStepViewModel.DiscardCommand.Execute();

            
        }



        private void AssetTheCommandsAreInUnmodifiedState()
        {
            Assert.False(_settingsStepViewModel.SaveCommand.CanExecute());
            Assert.False(_settingsStepViewModel.DiscardCommand.CanExecute());
            Assert.True(_settingsStepViewModel.OkCommand.CanExecute());
        }
    }

    
}