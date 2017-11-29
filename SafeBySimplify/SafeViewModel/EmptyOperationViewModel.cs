using Prism.Commands;
using SafeModel;

namespace SafeViewModel
{
    public class EmptyOperationViewModel : SingleOperationViewModel
    {
        public EmptyOperationViewModel(RecordHeader recordHeader)
        {
            HighlightCommand = new DelegateCommand(() =>
            {
                
            });
        }
    }
}