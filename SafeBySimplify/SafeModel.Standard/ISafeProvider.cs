namespace SafeModel.Standard
{
    public interface ISafeProvider : IHasWorkingDirectory, ISafeProviderForNonExistingUser, ISafeProviderForExistingUser
    {
    }
}
