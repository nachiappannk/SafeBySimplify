using System;
using Prism.Commands;
using SafeModel;

namespace SafeViewModel
{
    public class AddOperationViewModel : SingleOperationViewModel
    {
        public AddOperationViewModel(Action discardAction, Action<RecordHeader> saveAction)
        {
            DiscardCommand = new DelegateCommand(discardAction);
            SaveCommand = new DelegateCommand(() =>
            {
                saveAction.Invoke(null);
            });
        }
        public DelegateCommand DiscardCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }
    }
}