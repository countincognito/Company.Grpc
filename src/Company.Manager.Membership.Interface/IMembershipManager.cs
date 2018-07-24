using Company.Common.Data;
using System.Threading.Tasks;

namespace Company.Manager.Membership.Interface
{
    public interface IMembershipManager
    {
        Task<string> RegisterMemberAsync(RegisterRequest request);
    }
}
