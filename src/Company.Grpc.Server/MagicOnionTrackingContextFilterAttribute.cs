using Company.Grpc.Common;
using Grpc.Core;
using MagicOnion.Server;
using System;
using System.Threading.Tasks;

namespace Company.Grpc.Server
{
    public class MagicOnionTrackingContextFilterAttribute
        : MagicOnionFilterAttribute
    {
        public override ValueTask Invoke(ServiceContext context, Func<ServiceContext, ValueTask> next)
        {
            Metadata headers = context?.CallContext?.RequestHeaders ?? new Metadata();
            TrackingHelper.ProcessHeaders(headers);
            return next?.Invoke(context) ?? new ValueTask();
        }
    }
}
