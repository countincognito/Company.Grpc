using CommonServiceLocator;
using Company.Access.User.Interface;
using Company.Access.User.InterfaceGrpc;
using Company.Grpc.Server;
using MagicOnion;
using MagicOnion.Server;
using Serilog;
using Zametek.Utility.Logging;

namespace Company.Access.User.ServiceGrpc
{
    [MagicOnionTrackingContextFilter]
    public class UserAccessGrpc
        : ServiceBase<IUserAccessGrpc>, IUserAccessGrpc
    {
        private IUserAccess _Impl;

        public UserAccessGrpc()
        {
            var serilog = ServiceLocator.Current.GetInstance<ILogger>();
            var userAccess = ServiceLocator.Current.GetInstance<IUserAccess>();
            _Impl = LogProxy.Create(userAccess, serilog, LogType.All);
        }

        public UnaryResult<bool> CheckUserExistsAsync(string name)
        {
            return new UnaryResult<bool>(_Impl.CheckUserExistsAsync(name));
        }

        public UnaryResult<string> CreateUserAsync(string name)
        {
            return new UnaryResult<string>(_Impl.CreateUserAsync(name));
        }
    }
}
