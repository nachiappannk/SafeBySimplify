namespace SafeViewModel
{
    public abstract class WorkFlowStepViewModel : NotifiesPropertyChanged
    {
        public virtual void OnEntry()
        {
        }

        public virtual void OnExit()
        {
        }
    }
}