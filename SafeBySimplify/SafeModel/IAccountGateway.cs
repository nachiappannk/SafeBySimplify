namespace SafeModel
{
    public interface IAccountGateway
    {
        bool IsUsernameCreatable(string username);
    }
}