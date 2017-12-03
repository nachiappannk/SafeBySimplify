using System;
using Prism.Commands;

namespace SafeViewModel
{
    public class AddOperationViewModel : SingleOperationViewModel
    {
        public AddOperationViewModel(Action discardAction)
        {
            DiscardCommand = new DelegateCommand(discardAction);
        }
        public DelegateCommand DiscardCommand { get; set; }
    }
}