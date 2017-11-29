using Prism.Commands;
using SafeModel;

namespace SafeViewModel
{
    public class SingleOperationViewModel
    {
        public DelegateCommand HighlightCommand { get; set; }
    }

    public class RecordAlteringOperationViewModel : SingleOperationViewModel
    {
        public RecordAlteringOperationViewModel(RecordHeader recordHeader)
        {
        }
    }

}