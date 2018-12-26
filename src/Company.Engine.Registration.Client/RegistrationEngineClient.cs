using Company.Common.Data;
using Company.Engine.Registration.Interface;
using Company.Engine.Registration.InterfaceGrpc;
using System;
using System.Threading.Tasks;

namespace Company.Engine.Registration.Client
{
    public class RegistrationEngineClient
        : IRegistrationEngine
    {
        private IRegistrationEngineGrpc _GrpcClient;

        public RegistrationEngineClient(IRegistrationEngineGrpc grpcClient)
        {
            _GrpcClient = grpcClient ?? throw new ArgumentNullException(nameof(grpcClient));
        }

        public async Task<string> RegisterMemberAsync(RegisterRequest request)
        {
            return await _GrpcClient.RegisterMemberAsync(request);
        }
    }
}
