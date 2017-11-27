using System;
using Prism.Commands;
using SafeModel;

namespace SafeViewModel
{
    public class OperationStepViewModel : WorkFlowStepViewModel, IHasSafe
    {
        public OperationStepViewModel(ISafe safe, Action goToEntryStepAction)
        {
            Safe = safe;
            SignOutCommand = new DelegateCommand(goToEntryStepAction);
        }

        public ISafe Safe { get; set; }
        public DelegateCommand SignOutCommand { get; set; }
    }

    public interface IHasSafe
    {
        ISafe Safe { get; set; }    
    }
}