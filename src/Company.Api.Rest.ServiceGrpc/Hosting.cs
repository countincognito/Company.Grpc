using Company.Api.Rest.Impl;
using Company.Grpc.Client;
using Company.Grpc.Common;
using Company.Manager.Membership.Client;
using Company.Manager.Membership.Interface;
using Company.Manager.Membership.InterfaceGrpc;
using Destructurama;
using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using System.IO;
using System.Net;
using System.Reflection;
using Zametek.Utility.Logging;

namespace Company.Api.Rest.ServiceGrpc
{
    class Hosting
    {
        internal readonly static string ServiceName = typeof(Program).Assembly.GetName().Name;
        internal readonly static string BuildVersion = typeof(Startup).GetTypeInfo().Assembly.GetName().Version.ToString();

        public static IWebHost BuildWebHost(Serilog.ILogger serilog)
        {
            // SSL gRPC
            var caCrt = File.ReadAllText(EnvVars.CaCrtPath());
            var sslCredentials = new SslCredentials(caCrt);

            var membershipManagerGrpcChannel = new Channel(
                EnvVars.Target(@"MembershipManagerHost", @"MembershipManagerPort"),
                sslCredentials);

            // Create MagicOnion dynamic client proxy
            var membershipManagerGrpcClient = TrackingProxy.Create<IMembershipManagerGrpc>(membershipManagerGrpcChannel);

            var membershipManager = LogProxy.Create<IMembershipManager>(new MembershipManagerClient(membershipManagerGrpcClient), serilog, LogType.All);

            var restApiLogger = serilog;

            return new WebHostBuilder()
                .UseKestrel(options =>
                {
                    options.Listen(
                        IPAddress.Any,
                        EnvVars.LocalPort(@"RestApiPort"),
                        listenOptions => listenOptions.UseHttps(EnvVars.ServerPfxPath(), EnvVars.CrtPassword()));
                })
                .ConfigureServices(
                    services => services
                    .AddLogging()
                        .AddSingleton(membershipManager)
                        .AddSingleton(restApiLogger)
                        .AddSingleton<ILoggerFactory>(_ => new SerilogLoggerFactory(restApiLogger)))
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();
        }

        static void Main(string[] args)
        {
            Serilog.ILogger serilog = new LoggerConfiguration()
                .Enrich.FromLogProxy()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()
                .Destructure.UsingAttributes()
                .Enrich.WithProperty(nameof(BuildVersion), BuildVersion)
                .Enrich.WithProperty(nameof(ServiceName), ServiceName)
                .WriteTo.Console()
                .WriteTo.Seq(EnvVars.SeqAddress())
                .CreateLogger();
            Log.Logger = serilog;

            BuildWebHost(serilog).Run();
        }
    }
}
