using Grpc.Core;
using MagicOnion.Client;

namespace Company.Grpc.Client
{
    public class TrackingProxy
    {
        public static I Create<I>(Channel channel) where I : class, MagicOnion.IService<I>
        {
            return MagicOnionClient.Create<I>(new TrackingContextCallInvoker(channel));
        }
    }
}
