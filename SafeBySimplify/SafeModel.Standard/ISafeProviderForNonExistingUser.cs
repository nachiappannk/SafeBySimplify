namespace SafeModel.Standard
{
    public interface ISafeProviderForNonExistingUser
    {
        bool IsUserNameValidForNonExistingUser(string userName, out string errorMessage);
        bool IsPasswordValidForNonExistingUser(string password, out string errorMessage);
        ISafe CreateSafeForNonExistingUser(string userName, string masterpassword, string password);
    }
}
