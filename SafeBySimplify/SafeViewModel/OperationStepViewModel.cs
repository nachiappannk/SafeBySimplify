using SafeModel;

namespace SafeViewModel
{
    public class OperationStepViewModel : WorkFlowStepViewModel, IHasSafe
    {
        public ISafe Safe { get; set; }
    }

    public interface IHasSafe
    {
        ISafe Safe { get; set; }    
    }
}