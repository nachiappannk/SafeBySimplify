using System;
using Prism.Commands;

namespace SafeViewModel
{
    public class EntryStepViewModel : WorkFlowStepViewModel
    {
        public event Action GoToSettingsRequested;

        public DelegateCommand GoToSettingsCommand;

        public EntryStepViewModel()
        {
            GoToSettingsCommand = new DelegateCommand(() =>
            {
                GoToSettingsRequested?.Invoke();
            });
        }
    }
}