using System;
using Prism.Commands;
using SafeModel;

namespace SafeViewModel
{
    public class OperationStepViewModel : WorkFlowStepViewModel
    {
        public OperationStepViewModel(ISafe safe, Action goToEntryStepAction)
        {
            Safe = safe;
            SignOutCommand = new DelegateCommand(goToEntryStepAction);
        }

        public ISafe Safe { get; set; }
        public DelegateCommand SignOutCommand { get; set; }
    }
}