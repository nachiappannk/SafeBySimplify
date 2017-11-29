using Prism.Commands;
using SafeModel;

namespace SafeViewModel
{
    public class SingleOperationViewModel
    {
        public SingleOperationViewModel(RecordHeader recordHeader)
        {

        }

        public DelegateCommand HighlightCommand { get; set; }
    }

    public class EmptyOperationViewModel : SingleOperationViewModel
    {
        public EmptyOperationViewModel(RecordHeader recordHeader) : base(recordHeader)
        {
            HighlightCommand = new DelegateCommand(() => { });
        }
    }

    public class RecordAlteringOperationViewModel : SingleOperationViewModel
    {
        public RecordAlteringOperationViewModel(RecordHeader recordHeader) : base(recordHeader)
        {
        }
    }

}