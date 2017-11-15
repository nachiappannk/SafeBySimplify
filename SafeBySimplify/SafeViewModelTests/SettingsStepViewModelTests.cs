using System;
using System.Windows.Input;
using NSubstitute;
using NUnit.Framework;
using Prism.Commands;
using SafeModel;
using SafeViewModel;

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
            var saveCommandEventInfoFactory = _settingsStepViewModel.SaveCommand.GetCommandEventInfoFactory();
            var discardCommandEventInfoFactory = _settingsStepViewModel.DiscardCommand.GetCommandEventInfoFactory();
            var okCommandEventInfoFactory = _settingsStepViewModel.OkCommand.GetCommandEventInfoFactory();
            _settingsStepViewModel.WorkSpaceDirectory = @"D:\TFS_New";
            saveCommandEventInfoFactory.AssertCommandEventHappenedWithCorrectParameters(true);
            discardCommandEventInfoFactory.AssertCommandEventHappenedWithCorrectParameters(true);
            okCommandEventInfoFactory.AssertCommandEventHappenedWithCorrectParameters(false);
        }

        [Test]
        public void When_changing_the_workspace_and_discarding_changes_we_go_to_initial_state()
        {
            var initalValue = _settingsStepViewModel.WorkSpaceDirectory;
            _hasWorkingDirectory.ClearReceivedCalls();

            _settingsStepViewModel.WorkSpaceDirectory = @"D:\TFS_New";

            var saveCommandEventInfoFactory = _settingsStepViewModel.SaveCommand.GetCommandEventInfoFactory();
            var discardCommandEventInfoFactory = _settingsStepViewModel.DiscardCommand.GetCommandEventInfoFactory();
            var okCommandEventInfoFactory = _settingsStepViewModel.OkCommand.GetCommandEventInfoFactory();
            var s = _settingsStepViewModel.GetPropertyChangedEventInfoFactory<string>(nameof(_settingsStepViewModel.WorkSpaceDirectory));

            Assume.That(_settingsStepViewModel.DiscardCommand.CanExecute());
            _settingsStepViewModel.DiscardCommand.Execute();

            _hasWorkingDirectory.DidNotReceive().WorkingDirectory = Arg.Any<string>();

            var x = s.Invoke();

            Assert.True(x.EventReceived);
            Assert.AreEqual(initalValue, x.Value);

            Assert.AreEqual(initalValue, _settingsStepViewModel.WorkSpaceDirectory);

            saveCommandEventInfoFactory.AssertCommandEventHappenedWithCorrectParameters(false);
            discardCommandEventInfoFactory.AssertCommandEventHappenedWithCorrectParameters(false);
            okCommandEventInfoFactory.AssertCommandEventHappenedWithCorrectParameters(true);
        }


        [Test]
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

    public static  class CommandAssertionExtentions
    {
        public static Func<CommandEventInfo> GetCommandEventInfoFactory(this DelegateCommand command)
        {
            CommandEventInfo eventInfo = new CommandEventInfo();
            eventInfo.WasEventReceived = false;
            eventInfo.WasTheSenderCorrect = false;
            command.CanExecuteChanged += (obj, e) =>
            {
                if (command == obj) eventInfo.WasTheSenderCorrect = true;
                eventInfo.ValueOfCanExecuteOnEvent = command.CanExecute();
                eventInfo.WasEventReceived = true;
            };

            return () => new CommandEventInfo()
            {
                ValueOfCanExecuteOnEvent = eventInfo.ValueOfCanExecuteOnEvent,
                WasTheSenderCorrect = eventInfo.WasTheSenderCorrect,
                WasEventReceived = eventInfo.WasEventReceived
            };
        }

        public static void AssertCommandEventHappenedWithCorrectParameters(this Func<CommandEventInfo> commandEventInfoFactory,
            bool expectedValueOfCanExecute)
        {
            var commandEventInfo = commandEventInfoFactory.Invoke();
            Assert.True(commandEventInfo.WasEventReceived);
            Assert.True(commandEventInfo.WasTheSenderCorrect);
            Assert.AreEqual(expectedValueOfCanExecute, commandEventInfo.ValueOfCanExecuteOnEvent);
        }
    }

    public class CommandEventInfo
    {
        public bool ValueOfCanExecuteOnEvent { get; set; }
        public bool WasEventReceived { get; set; }
        public bool WasTheSenderCorrect { get; set; }
    }
}