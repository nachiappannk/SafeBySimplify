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
        private readonly ISafe _safe;
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
                    _currentStep = value;
                    FirePropertyChanged();
                }
            }
        }

        public WorkFlowViewModel(ISafe safe)
        {
            _safe = safe;
            CurrentStep = entryStepViewModel;
            _settingsStepViewModel = new SettingsStepViewModel(_safe);


            entryStepViewModel.GoToSettingsRequested += () =>
            {
                entryStepViewModel.OnExit();
                _settingsStepViewModel.OnEntry();
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

    public abstract class WorkFlowStepViewModel
    {
        public virtual void OnEntry()
        {

        }

        public virtual void OnExit()
        {

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

    public class SettingsStepViewModel : WorkFlowStepViewModel
    {
        private readonly IHasWorkingDirectory _hasWorkingDirectory;

        public SettingsStepViewModel(IHasWorkingDirectory hasWorkingDirectory)
        {
            _hasWorkingDirectory = hasWorkingDirectory;
        }

        public override void OnEntry()
        {
            WorkSpaceDirectory = _hasWorkingDirectory.WorkingDirectory;
        }

        public string WorkSpaceDirectory { get; set; }
    }
}