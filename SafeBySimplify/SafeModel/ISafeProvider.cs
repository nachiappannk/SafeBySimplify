namespace SafeModel
{
    public interface ISafeProvider : IHasWorkingDirectory , ISafeProviderForNonExistingUser
    {
    }

    public interface ISafeProviderForNonExistingUser
    {
        bool IsUserNameValidForNonExistingUser(string userName, out string errorMessage);
        bool IsPasswordValidForNonExistingUser(string password, out string errorMessage);
    }
}