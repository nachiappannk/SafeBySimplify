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

            SignUpCommand = new DelegateCommand(() =>
                {

                }, () => canSignUp);
        }

        private bool canSignUp = false;

        private string _signUpUserName;

        private void ComputeCanSignUp()
        {
            var newValue = false;
            if (string.IsNullOrWhiteSpace(SignUpUserName)) newValue = false;
            else if (string.IsNullOrWhiteSpace(SignUpPassword)) newValue = false;
            else if (string.IsNullOrWhiteSpace(SignUpConfirmPassword)) newValue = false;
            else newValue = true;
            if (newValue != canSignUp)
            {
                canSignUp = newValue;
                SignUpCommand.RaiseCanExecuteChanged();
            }
        }

        public string SignUpUserName
        {
            get { return _signUpUserName; }
            set {
                if (_signUpUserName != value)
                {
                    _signUpUserName = value;
                    ComputeCanSignUp();
                }
            }
        }

        

        private string _signUpPassword;

        public string SignUpPassword
        {
            get { return _signUpPassword; }
            set {
                if (_signUpPassword != value)
                {
                    _signUpPassword = value;
                    ComputeCanSignUp();
                }
            }
        }

        private string _signUpConfirmPassword;

        public string SignUpConfirmPassword
        {
            get { return _signUpConfirmPassword; }
            set {
                if (_signUpConfirmPassword != value)
                {
                    _signUpConfirmPassword = value;
                    ComputeCanSignUp();
                }
            }
        }

        public DelegateCommand SignUpCommand { get; set; }
        
    }
}