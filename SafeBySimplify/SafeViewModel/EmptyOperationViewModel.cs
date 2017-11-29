using System;
using Prism.Commands;
using SafeModel;

namespace SafeViewModel
{
    public class EmptyOperationViewModel : SingleOperationViewModel
    {
        public EmptyOperationViewModel(Action highlightAction)
        {
            HighlightCommand = new DelegateCommand(highlightAction);
        }
    }
}