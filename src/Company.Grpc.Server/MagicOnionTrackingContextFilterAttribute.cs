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
        public MagicOnionTrackingContextFilterAttribute()
            : base(null)
        {
        }

        public MagicOnionTrackingContextFilterAttribute(Func<ServiceContext, ValueTask> next)
            : base(next)
        {
        }

        public override ValueTask Invoke(ServiceContext context)
        {
            Metadata headers = context?.CallContext?.RequestHeaders ?? new Metadata();
            TrackingHelper.ProcessHeaders(headers);
            return Next.Invoke(context);
        }
    }
}
