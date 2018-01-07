using System.Collections.Generic;

namespace SafeModel.Standard
{
    public interface ISafeProviderForExistingUser
    {
        bool TryCreateSafeForExistingUser(string userName, string password, out ISafe safe);
        List<string> GetUserNames();
    }
}
