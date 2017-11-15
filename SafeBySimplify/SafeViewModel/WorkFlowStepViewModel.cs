namespace SafeViewModel
{
    public abstract class WorkFlowStepViewModel
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