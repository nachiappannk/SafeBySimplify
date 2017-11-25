using System.Collections.Generic;

namespace SafeModel
{
    public interface ISafeProvider : IHasWorkingDirectory , ISafeProviderForNonExistingUser , ISafeProviderForExistingUser
    {
    }

    public interface ISafeProviderForNonExistingUser
    {
        bool IsUserNameValidForNonExistingUser(string userName, out string errorMessage);
        bool IsPasswordValidForNonExistingUser(string password, out string errorMessage);
        ISafe CreateSafeForNonExistingUser(string userName, string masterpassword, string password);
    }

    public interface ISafeProviderForExistingUser
    {
        bool TryCreateSafeForExistingUser(string userName, string password, out ISafe safe);
        List<string> GetUserNames();
    }

    public interface ISafe
    {

    }
}