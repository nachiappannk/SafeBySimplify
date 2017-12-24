namespace SafeViewModel
{
    public abstract class WorkFlowStepViewModel : NotifiesPropertyChanged
    {
        public virtual void OnEntry()
        {
        }
    }
}