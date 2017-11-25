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
        private CommandObserver _saveCommandObserver;
        private CommandObserver _discardCommandObserver;
        private CommandObserver _okayCommandObserver;
        private ViewModelPropertyObserver<string> _workSpaceDirectoryObserver;
        private ISettingGateway _settingGateway;

        [SetUp]
        public void SetUp()
        {
            //_hasWorkingDirectory = Substitute.For<IHasWorkingDirectory>();
            _settingGateway = Substitute.For<ISettingGateway>();
            _hasWorkingDirectory = new SafeProvider(_settingGateway);
            _settingGateway.IsWorkingDirectoryAvailable().Returns(true);
            _settingGateway.GetWorkingDirectory().Returns(InitialWorkingDirectory);
            //_hasWorkingDirectory.WorkingDirectory = InitialWorkingDirectory;

            _settingsStepViewModel = new SettingsStepViewModel(_hasWorkingDirectory);
            _settingsStepViewModel.OnEntry();

            _saveCommandObserver = _settingsStepViewModel.SaveCommand.GetDelegateCommandObserver();
            _discardCommandObserver = _settingsStepViewModel.DiscardCommand.GetDelegateCommandObserver();
            _okayCommandObserver = _settingsStepViewModel.OkCommand.GetDelegateCommandObserver();

            _workSpaceDirectoryObserver = _settingsStepViewModel
                .GetPropertyObserver<string>(nameof(_settingsStepViewModel.WorkSpaceDirectory));

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
            _settingsStepViewModel.WorkSpaceDirectory = @"D:\TFS_New";
            AssertTheCommandsMovedToModifiedState();
        }


        [Test]
        public void When_changing_workspace_and_discarding_we_go_to_initial_state_and_workspace_is_with_old_value_and_no_write_is_made_to_lib()
        {
            var initalValue = InitialWorkingDirectory;
            _settingsStepViewModel.WorkSpaceDirectory = @"D:\TFS_New";

            Assume.That(_settingsStepViewModel.DiscardCommand.CanExecute(),"Discard is in disabled state");
            _settingsStepViewModel.DiscardCommand.Execute();
            _settingGateway.DidNotReceive().PutWorkingDirectory(Arg.Any<string>());
            _workSpaceDirectoryObserver.AssertProperyHasChanged(initalValue);
            Assert.AreEqual(initalValue, _settingsStepViewModel.WorkSpaceDirectory);

            AssertTheCommandsMovedToInitialState();
        }



        [Test]
        public void When_changing_workspace_and_saving_we_go_to_initial_state_and_workspace_is_the_new_value_and_write_is_made_to_lib()
        {
            //_hasWorkingDirectory.ClearReceivedCalls();


            var newValueOfWorkSpaceDirectory = @"D:\TFS_New";
            _settingsStepViewModel.WorkSpaceDirectory = newValueOfWorkSpaceDirectory;

            Assume.That(_settingsStepViewModel.SaveCommand.CanExecute());
            _settingsStepViewModel.SaveCommand.Execute();
            _settingGateway.Received().PutWorkingDirectory(newValueOfWorkSpaceDirectory);

            Assert.AreEqual(newValueOfWorkSpaceDirectory, _settingsStepViewModel.WorkSpaceDirectory);
            AssertTheCommandsMovedToInitialState();
        }

        [Test]
        public void When_changing_the_workspace_and_discarding_changes_working_directory_is_same_as_initial()
        {
            _settingsStepViewModel.WorkSpaceDirectory = @"D:\TFS_New";

            Assume.That(_settingsStepViewModel.DiscardCommand.CanExecute());
            _settingsStepViewModel.DiscardCommand.Execute();

            _workSpaceDirectoryObserver.AssertProperyHasChanged(InitialWorkingDirectory);
            AssertTheCommandsMovedToInitialState();
        }

        private void AssertTheCommandsMovedToModifiedState()
        {
            _saveCommandObserver.AssertTheCommandBecameExecutable();
            _discardCommandObserver.AssertTheCommandBecameExecutable();
            _okayCommandObserver.AssertTheCommandBecameNonExecutable();
        }

        private void AssertTheCommandsMovedToInitialState()
        {
            _saveCommandObserver.AssertTheCommandBecameNonExecutable();
            _discardCommandObserver.AssertTheCommandBecameNonExecutable();
            _okayCommandObserver.AssertTheCommandBecameExecutable();
        }

        private void AssetTheCommandsAreInUnmodifiedState()
        {
            Assert.False(_settingsStepViewModel.SaveCommand.CanExecute());
            Assert.False(_settingsStepViewModel.DiscardCommand.CanExecute());
            Assert.True(_settingsStepViewModel.OkCommand.CanExecute());
        }
    }
}