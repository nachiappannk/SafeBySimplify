namespace SafeModel
{
    public interface ISafeProvider : IHasWorkingDirectory , ISafeProviderForNonExistingUser , ISafeProviderForExistingUser
    {
    }

    public interface ISafeProviderForNonExistingUser
    {
        bool IsUserNameValidForNonExistingUser(string userName, out string errorMessage);
        bool IsPasswordValidForNonExistingUser(string password, out string errorMessage);
        ISafe CreateSafeForNonExistingUser(string userName, string password);
    }

    public interface ISafeProviderForExistingUser
    {
        bool IsUserNameValidForExistingUser(string userName, out string errorMessage);
        bool IsPasswordValidForExistingUser(string password, out string errorMessage);
        bool TryCreateSafeForExistingUser(string userName, string password, out ISafe safe);
    }

    public interface ISafe
    {

    }
}