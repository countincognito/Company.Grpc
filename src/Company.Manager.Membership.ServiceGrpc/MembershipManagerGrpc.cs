using CommonServiceLocator;
using Company.Common.Data;
using Company.Grpc.Server;
using Company.Manager.Membership.Interface;
using Company.Manager.Membership.InterfaceGrpc;
using MagicOnion;
using MagicOnion.Server;
using Serilog;
using Zametek.Utility.Logging;

namespace Company.Manager.Membership.ServiceGrpc
{
    [MagicOnionTrackingContextFilter]
    public class MembershipManagerGrpc
        : ServiceBase<IMembershipManagerGrpc>, IMembershipManagerGrpc
    {
        private IMembershipManager _Impl;

        public MembershipManagerGrpc()
        {
            var serilog = ServiceLocator.Current.GetInstance<ILogger>();
            var membershipManager = ServiceLocator.Current.GetInstance<IMembershipManager>();
            _Impl = LogProxy.Create<IMembershipManager>(membershipManager, serilog, LogType.All);
        }

        public UnaryResult<string> RegisterMemberAsync(RegisterRequest request)
        {
            return new UnaryResult<string>(_Impl.RegisterMemberAsync(request));
        }
    }
}
