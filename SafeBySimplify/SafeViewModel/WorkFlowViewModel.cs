using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Prism.Commands;
using SafeModel;
using SafeViewModel.Annotations;

namespace SafeViewModel
{
    public class WorkFlowViewModel : INotifyPropertyChanged
    {
        private readonly ISafeProvider _safeProvider;
        private EntryStepViewModel entryStepViewModel = new EntryStepViewModel();
        private readonly SettingsStepViewModel _settingsStepViewModel;

        private WorkFlowStepViewModel _currentStep;

        public WorkFlowStepViewModel CurrentStep
        {
            get { return _currentStep; }
            set
            {
                if (_currentStep != value)
                {
                    _currentStep?.OnExit();
                    _currentStep = value;
                    _currentStep?.OnEntry();
                    FirePropertyChanged();
                }
            }
        }

        public WorkFlowViewModel(ISafeProvider safeProvider)
        {
            _safeProvider = safeProvider;
            CurrentStep = entryStepViewModel;
            _settingsStepViewModel = new SettingsStepViewModel(_safeProvider);

            entryStepViewModel.GoToSettingsRequested += () =>
            {
                CurrentStep = _settingsStepViewModel;
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void FirePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

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