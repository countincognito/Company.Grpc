using Company.Common.Data;
using System.Threading.Tasks;

namespace Company.Engine.Registration.Interface
{
    public interface IRegistrationEngine
    {
        Task<string> RegisterMemberAsync(RegisterRequest request);
    }
}
