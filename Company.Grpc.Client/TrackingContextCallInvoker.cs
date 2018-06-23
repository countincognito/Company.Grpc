using Company.Grpc.Common;
using Grpc.Core;
using Grpc.Core.Utils;

namespace Company.Grpc.Client
{
    public class TrackingContextCallInvoker
        : CallInvoker
    {
        private readonly Channel m_Channel;

        /// <summary>
        /// Initializes a new instance of the TrackingContextCallInvoker class.
        /// </summary>
        /// <param name="channel">Channel to use.</param>
        public TrackingContextCallInvoker(Channel channel)
        {
            m_Channel = GrpcPreconditions.CheckNotNull(channel);
        }

        /// <summary>
        /// Invokes a simple remote call in a blocking fashion.
        /// </summary>
        public override TResponse BlockingUnaryCall<TRequest, TResponse>(
            Method<TRequest, TResponse> method,
            string host,
            CallOptions options,
            TRequest request)
        {
            CallOptions updateOptions = ProcessOptions(options);
            var call = CreateCall(method, host, updateOptions);
            return Calls.BlockingUnaryCall(call, request);
        }

        /// <summary>
        /// Invokes a simple remote call asynchronously.
        /// </summary>
        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
            Method<TRequest, TResponse> method,
            string host,
            CallOptions options,
            TRequest request)
        {
            CallOptions updateOptions = ProcessOptions(options);
            var call = CreateCall(method, host, updateOptions);
            return Calls.AsyncUnaryCall(call, request);
        }

        /// <summary>
        /// Invokes a server streaming call asynchronously.
        /// In server streaming scenario, client sends on request and server responds with a stream of responses.
        /// </summary>
        public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(
            Method<TRequest, TResponse> method,
            string host,
            CallOptions options,
            TRequest request)
        {
            CallOptions updateOptions = ProcessOptions(options);
            var call = CreateCall(method, host, updateOptions);
            return Calls.AsyncServerStreamingCall(call, request);
        }

        /// <summary>
        /// Invokes a client streaming call asynchronously.
        /// In client streaming scenario, client sends a stream of requests and server responds with a single response.
        /// </summary>
        public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(
            Method<TRequest, TResponse> method,
            string host,
            CallOptions options)
        {
            CallOptions updateOptions = ProcessOptions(options);
            var call = CreateCall(method, host, updateOptions);
            return Calls.AsyncClientStreamingCall(call);
        }

        /// <summary>
        /// Invokes a duplex streaming call asynchronously.
        /// In duplex streaming scenario, client sends a stream of requests and server responds with a stream of responses.
        /// The response stream is completely independent and both side can be sending messages at the same time.
        /// </summary>
        public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(
            Method<TRequest, TResponse> method,
            string host,
            CallOptions options)
        {
            CallOptions updateOptions = ProcessOptions(options);
            var call = CreateCall(method, host, updateOptions);
            return Calls.AsyncDuplexStreamingCall(call);
        }

        /// <summary>
        /// Creates call invocation details for given method.
        /// </summary>
        protected virtual CallInvocationDetails<TRequest, TResponse> CreateCall<TRequest, TResponse>(
            Method<TRequest, TResponse> method,
            string host,
            CallOptions options)
            where TRequest : class
            where TResponse : class
        {
            CallOptions updateOptions = ProcessOptions(options);
            return new CallInvocationDetails<TRequest, TResponse>(m_Channel, method, host, updateOptions);
        }

        private static CallOptions ProcessOptions(CallOptions options)
        {
            Metadata headers = options.Headers ?? new Metadata();
            CallOptions updatedOptions = options.WithHeaders(TrackingHelper.ProcessHeaders(headers));
            return updatedOptions;
        }
    }
}
