using System.Threading.Tasks;

namespace Company.Access.User.Interface
{
    public interface IUserAccess
    {
        Task<bool> CheckUserExistsAsync(string name);

        Task<string> CreateUserAsync(string name);
    }
}
