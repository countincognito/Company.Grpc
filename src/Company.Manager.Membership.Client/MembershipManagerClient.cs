using Company.Common.Data;
using Company.Manager.Membership.Interface;
using Company.Manager.Membership.InterfaceGrpc;
using System;
using System.Threading.Tasks;

namespace Company.Manager.Membership.Client
{
    public class MembershipManagerClient
        : IMembershipManager
    {
        private IMembershipManagerGrpc _GrpcClient;

        public MembershipManagerClient(IMembershipManagerGrpc grpcClient)
        {
            _GrpcClient = grpcClient ?? throw new ArgumentNullException(nameof(grpcClient));
        }

        public async Task<string> RegisterMemberAsync(RegisterRequest request)
        {
            return await _GrpcClient.RegisterMemberAsync(request);
        }
    }
}
