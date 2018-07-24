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

        public MagicOnionTrackingContextFilterAttribute(Func<ServiceContext, Task> next)
            : base(next)
        {
        }

        public async override Task Invoke(ServiceContext context)
        {
            Metadata headers = context?.CallContext?.RequestHeaders ?? new Metadata();
            TrackingHelper.ProcessHeaders(headers);
            await Next?.Invoke(context);
        }
    }
}
