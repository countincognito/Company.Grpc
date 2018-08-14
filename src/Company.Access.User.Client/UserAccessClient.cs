using Company.Access.User.Interface;
using Company.Access.User.InterfaceGrpc;
using System;
using System.Threading.Tasks;

namespace Company.Access.User.Client
{
    public class UserAccessClient
        : IUserAccess
    {
        private IUserAccessGrpc _GrpcClient;

        public UserAccessClient(IUserAccessGrpc grpcClient)
        {
            _GrpcClient = grpcClient ?? throw new ArgumentNullException(nameof(grpcClient));
        }

        public async Task<bool> CheckUserExistsAsync(string name)
        {
            return await _GrpcClient.CheckUserExistsAsync(name);
        }

        public async Task<string> CreateUserAsync(string name)
        {
            return await _GrpcClient.CreateUserAsync(name);
        }
    }
}
