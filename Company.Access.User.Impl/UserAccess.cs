using Company.Access.User.Interface;
using Serilog;
using System;
using System.Threading.Tasks;
using Zametek.Utility.Logging;

namespace Company.Access.User.Impl
{
    [DiagnosticLogging(LogActive.On)]
    public class UserAccess
        : IUserAccess
    {
        private readonly ILogger _Logger;

        public UserAccess(ILogger logger)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> CheckUserExistsAsync([DiagnosticLogging(LogActive.Off)]string name)
        {
            _Logger.Information($@"{nameof(CheckUserExistsAsync)} Invoked");
            _Logger.Information($@"{nameof(CheckUserExistsAsync)} {name}");

            await Task.Delay(new Random().Next(100, 200)).ConfigureAwait(false);

            return await Task.FromResult(false).ConfigureAwait(false);
        }

        [return: DiagnosticLogging(LogActive.Off)]
        public async Task<string> CreateUserAsync(string name)
        {
            _Logger.Information($@"{nameof(CreateUserAsync)} Invoked");
            _Logger.Information($@"{nameof(CreateUserAsync)} {name}");

            if (string.CompareOrdinal(name, "ThrowException") == 0)
            {
                throw new Exception("Throw Exception just to make things interesting.");
            }

            await Task.Delay(new Random().Next(100, 200)).ConfigureAwait(false);

            return await Task.FromResult($"\r\n        UserAccess.CreateUserAsync -> {name} -> {DateTime.UtcNow}").ConfigureAwait(false);
        }
    }
}
