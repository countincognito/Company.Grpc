using Company.Access.User.Impl;
using Company.Access.User.Interface;
using Company.Api.Rest.Impl;
using Company.Engine.Registration.Impl;
using Company.Engine.Registration.Interface;
using Company.Manager.Membership.Impl;
using Company.Manager.Membership.Interface;
using Destructurama;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using System.IO;
using System.Reflection;
using Zametek.Utility.Logging;

namespace Test.InProc.RestApi
{
    public class Program
    {
        internal readonly static string ServiceName = typeof(Program).Assembly.GetName().Name;
        internal readonly static string BuildVersion = typeof(Startup).GetTypeInfo().Assembly.GetName().Version.ToString();

        public static void Main(string[] args)
        {
            Serilog.ILogger serilog = new LoggerConfiguration()
                .Enrich.FromLogProxy()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()
                .Destructure.UsingAttributes()
                .Enrich.WithProperty(nameof(BuildVersion), BuildVersion)
                .Enrich.WithProperty(nameof(ServiceName), ServiceName)
                .WriteTo.Console()
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();
            Log.Logger = serilog;

            BuildWebHost(serilog).Run();
        }

        public static IWebHost BuildWebHost(Serilog.ILogger serilog)
        {
            var userAccess = LogProxy.Create<IUserAccess>(new UserAccess(serilog), serilog, LogType.All);
            var registrationEngine = LogProxy.Create<IRegistrationEngine>(new RegistrationEngine(userAccess, serilog), serilog, LogType.All);
            var membershipManager = LogProxy.Create<IMembershipManager>(new MembershipManager(registrationEngine, serilog), serilog, LogType.All);

            var restApiLogger = serilog;

            return new WebHostBuilder()
                .UseKestrel()
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
    }
}
