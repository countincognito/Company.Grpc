using Company.Common.Data;
using MagicOnion;

namespace Company.Manager.Membership.InterfaceGrpc
{
    public interface IMembershipManagerGrpc
        : IService<IMembershipManagerGrpc>
    {
        UnaryResult<string> RegisterMemberAsync(RegisterRequest request);
    }
}
