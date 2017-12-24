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
            _settingGateway = new SettingGatewayForTests();
            _settingGateway.SetWorkingDirectory(InitialWorkingDirectory);

            var safeProvider = new SafeProvider();
            safeProvider.SettingGateway = _settingGateway;
            _hasWorkingDirectory = safeProvider;
            

            _settingsStepViewModel = new SettingsStepViewModel(_hasWorkingDirectory, () => { });
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

            Assert.AreEqual(InitialWorkingDirectory, _settingsStepViewModel.WorkSpaceDirectory);
            _workSpaceDirectoryObserver.AssertProperyHasChanged(initalValue);
            Assert.AreEqual(initalValue, _settingsStepViewModel.WorkSpaceDirectory);

            AssertTheCommandsMovedToInitialState();
        }

        [Test]
        public void When_changing_workspace_and_saving_we_go_to_initial_state_and_workspace_is_the_new_value_and_write_is_made_to_lib()
        {
            var newValueOfWorkSpaceDirectory = @"D:\TFS_New";
            _settingsStepViewModel.WorkSpaceDirectory = newValueOfWorkSpaceDirectory;

            Assume.That(_settingsStepViewModel.SaveCommand.CanExecute());
            _settingsStepViewModel.SaveCommand.Execute();
            Assert.AreEqual(newValueOfWorkSpaceDirectory,_settingGateway.GetWorkingDirectory());

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