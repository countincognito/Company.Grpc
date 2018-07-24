using Company.Common.Data;
using MagicOnion;

namespace Company.Engine.Registration.InterfaceGrpc
{
    public interface IRegistrationEngineGrpc
        : IService<IRegistrationEngineGrpc>
    {
        UnaryResult<string> RegisterMemberAsync(RegisterRequest request);
    }
}
