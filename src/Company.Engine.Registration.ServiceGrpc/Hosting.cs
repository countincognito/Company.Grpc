﻿using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using Company.Access.User.Client;
using Company.Access.User.Interface;
using Company.Access.User.InterfaceGrpc;
using Company.Engine.Registration.Impl;
using Company.Engine.Registration.Interface;
using Company.Grpc.Client;
using Company.Grpc.Common;
using Destructurama;
using Grpc.Core;
using Grpc.Core.Logging;
using MagicOnion.Server;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Zametek.Utility.Logging;

namespace Company.Engine.Registration.ServiceGrpc
{
    class Hosting
    {
        private static void Main(string[] args)
        {
            Register();
            RunHost();
        }

        private static void Register()
        {
            Serilog.ILogger serilog = new LoggerConfiguration()
                .Enrich.FromLogProxy()
                .Destructure.UsingAttributes()
                .WriteTo.Seq(EnvVars.SeqAddress())
                .CreateLogger();
            Log.Logger = serilog;

            var builder = new ContainerBuilder();

            // SSL gRPC
            var caCrt = File.ReadAllText(EnvVars.CaCrtPath());
            var sslCredentials = new SslCredentials(caCrt);

            var userAccessGrpcChannel = new Channel(
                EnvVars.Target(@"UserAccessHost", @"UserAccessPort"),
                sslCredentials);

            // Create MagicOnion dynamic client proxy
            var userAccessGrpcClient = TrackingProxy.Create<IUserAccessGrpc>(userAccessGrpcChannel);

            var userAccess = LogProxy.Create<IUserAccess>(new UserAccessClient(userAccessGrpcClient), serilog, LogType.All);

            builder.RegisterInstance<Serilog.ILogger>(serilog);
            builder.RegisterInstance<IUserAccess>(userAccess);
            builder.RegisterType<RegistrationEngine>().As<IRegistrationEngine>();
            IContainer container = builder.Build();

            // Set up the service locator
            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));

            GrpcEnvironment.SetLogger(new ConsoleLogger());
        }

        private static void RunHost()
        {
            MagicOnionServiceDefinition service = MagicOnionEngine.BuildServerServiceDefinition(
                new[] { typeof(RegistrationEngineGrpc) },
                new MagicOnionOptions(isReturnExceptionStackTraceInErrorDetail: true));

            // SSL gRPC
            string caCrt = File.ReadAllText(EnvVars.CaCrtPath());
            string serverCrt = File.ReadAllText(EnvVars.ServerCrtPath());
            string serverKey = File.ReadAllText(EnvVars.ServerKeyPath());
            var keyPair = new KeyCertificatePair(serverCrt, serverKey);
            var sslCredentials = new SslServerCredentials(new List<KeyCertificatePair>() { keyPair }, caCrt, false);

            int localPort = EnvVars.LocalPort(@"RegistrationEnginePort");

            var server = new global::Grpc.Core.Server
            {
                Services = { service },
                Ports = { new ServerPort("0.0.0.0", localPort, sslCredentials) }
            };

            // launch gRPC Server.
            server.Start();

            Log.Information($"Server listening on port {localPort}");

            var cts = new CancellationTokenSource();

            var syncTask = new TaskCompletionSource<bool>();

            System.Runtime.Loader.AssemblyLoadContext.Default.Unloading += (context) =>
            {
                Log.Information("Greeter server received kill signal...");
                cts.Cancel();
                server.ShutdownAsync().Wait();
                syncTask.SetResult(true);
            };
            syncTask.Task.Wait(-1);
            Log.Information("Greeter server stopped");
        }
    }
}
