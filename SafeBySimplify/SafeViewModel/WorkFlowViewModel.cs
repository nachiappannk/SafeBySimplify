using System.ComponentModel;
using System.Runtime.CompilerServices;
using SafeModel;
using SafeViewModel.Annotations;

namespace SafeViewModel
{
    public class WorkFlowViewModel : INotifyPropertyChanged
    {
        private readonly ISafeProvider _safeProvider;
        private readonly EntryStepViewModel _entryStepViewModel;
        private readonly SettingsStepViewModel _settingsStepViewModel;
        private readonly OperationStepViewModel _operationStepViewModel;

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

            _operationStepViewModel = new OperationStepViewModel();
            _settingsStepViewModel = new SettingsStepViewModel(_safeProvider);
            _entryStepViewModel = new EntryStepViewModel(_safeProvider, _operationStepViewModel);

            CurrentStep = _entryStepViewModel;

            _entryStepViewModel.GoToSettingsRequested += () =>
            {
                CurrentStep = _settingsStepViewModel;
            };

            _entryStepViewModel.GoToOperationsRequested += () =>
            {
                CurrentStep = _operationStepViewModel;
            };

            _settingsStepViewModel.GoToEntryStepRequested += () =>
            {
                CurrentStep = _entryStepViewModel;
            };

            _operationStepViewModel.GoToEntryStepRequested += () =>
            {
                CurrentStep = _entryStepViewModel;
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void FirePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}