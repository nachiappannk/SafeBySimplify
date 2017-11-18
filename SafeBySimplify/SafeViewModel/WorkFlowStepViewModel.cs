namespace SafeViewModel
{
    public abstract class WorkFlowStepViewModel : NotifiesPropertyChanged
    {
        public bool IsActive { get; private set; }

        public virtual void OnEntry()
        {
            IsActive = true;
        }

        public virtual void OnExit()
        {
            IsActive = false;
        }
    }
}