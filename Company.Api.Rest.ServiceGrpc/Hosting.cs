using Company.Api.Rest.Impl;
using Company.Grpc.Client;
using Company.Grpc.Common;
using Company.Manager.Membership.ClientGrpc;
using Company.Manager.Membership.Interface;
using Company.Manager.Membership.InterfaceGrpc;
using Destructurama;
using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.IO;
using System.Net;
using Zametek.Utility.Logging;

namespace Company.Api.Rest.ServiceGrpc
{
    class Hosting
    {
        public static IWebHost BuildWebHost(ILogger serilog)
        {
            // SSL gRPC
            var caCrt = File.ReadAllText(EnvVars.CaCrtPath());
            var sslCredentials = new SslCredentials(caCrt);

            var membershipManagerChannel = new Channel(
                EnvVars.Target(@"MembershipManagerHost", @"MembershipManagerPort"),
                sslCredentials);

            // Create MagicOnion dynamic client proxy
            var membershipManagerClient = TrackingProxy.Create<IMembershipManagerGrpc>(membershipManagerChannel);

            var membershipManager = LogProxy.Create<IMembershipManager>(new MembershipManagerGrpc(membershipManagerClient), serilog, LogType.All);

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
                        .AddSingleton<IMembershipManager>(membershipManager)
                        .AddSingleton<ILogger>(serilog))
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();
        }

        static void Main(string[] args)
        {
            ILogger serilog = new LoggerConfiguration()
                .Enrich.FromLogProxy()
                .Destructure.UsingAttributes()
                .WriteTo.Seq(EnvVars.SeqAddress())
                .CreateLogger();
            Log.Logger = serilog;

            BuildWebHost(serilog).Run();
        }
    }
}
