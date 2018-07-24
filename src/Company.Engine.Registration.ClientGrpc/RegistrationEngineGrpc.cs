using Company.Common.Data;
using Company.Engine.Registration.Interface;
using Company.Engine.Registration.InterfaceGrpc;
using System;
using System.Threading.Tasks;

namespace Company.Engine.Registration.ClientGrpc
{
    public class RegistrationEngineGrpc
        : IRegistrationEngine
    {
        private IRegistrationEngineGrpc _GrpcClient;

        public RegistrationEngineGrpc(IRegistrationEngineGrpc grpcClient)
        {
            _GrpcClient = grpcClient ?? throw new ArgumentNullException(nameof(grpcClient));
        }

        public async Task<string> RegisterMemberAsync(RegisterRequest request)
        {
            return await _GrpcClient.RegisterMemberAsync(request);
        }
    }
}
