using System;
using Prism.Commands;
using SafeModel;

namespace SafeViewModel
{
    public class EntryStepViewModel : WorkFlowStepViewModel
    {
        
        public event Action GoToSettingsRequested;
        public event Action GoToOperationsRequested;

        public DelegateCommand GoToSettingsCommand;

        public SignUpViewModel SignUpViewModel { get; set; }

        public SignInViewModel SignInViewModel { get; set; }

        private void OnLoginCompletion()
        {
            GoToOperationsRequested?.Invoke();            
        }

        public EntryStepViewModel(
            ISafeProvider safeProvider,
            IHasSafe hasSafe)
        {
            GoToSettingsCommand = new DelegateCommand(() =>
            {
                GoToSettingsRequested?.Invoke();
            }); 

            SignUpViewModel = new SignUpViewModel(safeProvider, hasSafe, OnLoginCompletion);
            SignInViewModel = new SignInViewModel(safeProvider, hasSafe, OnLoginCompletion);
        }


        
    }
}