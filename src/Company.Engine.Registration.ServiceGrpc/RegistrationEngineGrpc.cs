using CommonServiceLocator;
using Company.Common.Data;
using Company.Engine.Registration.Interface;
using Company.Engine.Registration.InterfaceGrpc;
using Company.Grpc.Server;
using MagicOnion;
using MagicOnion.Server;
using Serilog;
using Zametek.Utility.Logging;

namespace Company.Engine.Registration.ServiceGrpc
{
    [MagicOnionTrackingContextFilter]
    public class RegistrationEngineGrpc
        : ServiceBase<IRegistrationEngineGrpc>, IRegistrationEngineGrpc
    {
        private IRegistrationEngine _Impl;

        public RegistrationEngineGrpc()
        {
            var serilog = ServiceLocator.Current.GetInstance<ILogger>();
            var registrationEngine = ServiceLocator.Current.GetInstance<IRegistrationEngine>();
            _Impl = LogProxy.Create<IRegistrationEngine>(registrationEngine, serilog, LogType.All);
        }

        public UnaryResult<string> RegisterMemberAsync(RegisterRequest request)
        {
            return new UnaryResult<string>(_Impl.RegisterMemberAsync(request));
        }
    }
}
