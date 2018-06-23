using MagicOnion;

namespace Company.Access.User.InterfaceGrpc
{
    public interface IUserAccessGrpc
        : IService<IUserAccessGrpc>
    {
        UnaryResult<bool> CheckUserExistsAsync(string name);

        UnaryResult<string> CreateUserAsync(string name);
    }
}
