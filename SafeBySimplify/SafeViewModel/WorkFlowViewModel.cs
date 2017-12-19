using System.ComponentModel;
using System.Runtime.CompilerServices;
using SafeModel;
using SafeViewModel.Annotations;

namespace SafeViewModel
{
    public class WorkFlowViewModel : INotifyPropertyChanged
    {
        private readonly ISafeProvider _safeProvider;

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
            GoToEntryStep();
        }

        private void GoToOperationStep(ISafe safe, string userName)
        {
            var operationStepViewModel = new OperationStepViewModel(safe, GoToEntryStep);
            operationStepViewModel.UserName = userName;
            CurrentStep = operationStepViewModel;
        }

        private void GoToSettingStep()
        {
            CurrentStep = new SettingsStepViewModel(_safeProvider, GoToEntryStep);
        }

        private void GoToEntryStep()
        {
            CurrentStep = new EntryStepViewModel(_safeProvider, GoToSettingStep, GoToOperationStep);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void FirePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}