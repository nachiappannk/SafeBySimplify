using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Prism.Commands;
using SafeModel;
using SafeViewModel.Annotations;

namespace SafeViewModel
{
    public class SettingsStepViewModel : WorkFlowStepViewModel, INotifyPropertyChanged
    {
        public event Action GoToEntryStepRequested;

        private readonly IHasWorkingDirectory _hasWorkingDirectory;

        private bool _isInSavedState = true;

        private bool IsInSavedState
        {
            get { return _isInSavedState; }
            set
            {
                if (_isInSavedState != value)
                {
                    _isInSavedState = value;
                    OkCommand.RaiseCanExecuteChanged();
                    SaveCommand.RaiseCanExecuteChanged();
                    DiscardCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public DelegateCommand SaveCommand { get; set; }

        public DelegateCommand DiscardCommand { get; set; }

        public DelegateCommand OkCommand { get; set; }

        private string _savedValueOfWorkingDirectory;

        private string _workingDirectory;

        public string WorkSpaceDirectory
        {
            get { return _workingDirectory; }
            set
            {
                if(value != _workingDirectory)
                { 
                    _workingDirectory = value;
                    OnPropertyChanged();
                }
                IsInSavedState = _savedValueOfWorkingDirectory == _workingDirectory;
            }
        }

        public SettingsStepViewModel(IHasWorkingDirectory hasWorkingDirectory)
        {
            _hasWorkingDirectory = hasWorkingDirectory;
            SaveCommand = new DelegateCommand(Save, () => !IsInSavedState);
            DiscardCommand = new DelegateCommand(Discard, () => !IsInSavedState);
            OkCommand = new DelegateCommand(Ok, ()=> IsInSavedState);
        }

        private void Discard()
        {
            WorkSpaceDirectory = _savedValueOfWorkingDirectory;
            IsInSavedState = true;
        }

        private void Save()
        {
            if (!IsInSavedState) _hasWorkingDirectory.WorkingDirectory = WorkSpaceDirectory;
            _savedValueOfWorkingDirectory = WorkSpaceDirectory;
            IsInSavedState = true;
        }

        private void Ok()
        {
            GoToEntryStepRequested?.Invoke();
        }

        public override void OnEntry()
        {
            base.OnEntry();
            GoToSavedState();
        }

        private void GoToSavedState()
        {
            _savedValueOfWorkingDirectory = _hasWorkingDirectory.WorkingDirectory;
            WorkSpaceDirectory = _savedValueOfWorkingDirectory;
            IsInSavedState = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}