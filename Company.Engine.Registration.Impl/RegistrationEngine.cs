using Company.Access.User.Interface;
using Company.Common.Data;
using Company.Engine.Registration.Interface;
using Serilog;
using System;
using System.Threading.Tasks;
using Zametek.Utility.Logging;

namespace Company.Engine.Registration.Impl
{
    [DiagnosticLogging(LogActive.On)]
    public class RegistrationEngine
        : IRegistrationEngine
    {
        private readonly IUserAccess _UserAccess;
        private readonly ILogger _Logger;

        public RegistrationEngine(
            IUserAccess userAccess,
            ILogger logger)
        {
            _UserAccess = userAccess ?? throw new ArgumentNullException(nameof(userAccess));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> RegisterMemberAsync(RegisterRequest request)
        {
            _Logger.Information($"{nameof(RegisterMemberAsync)} Invoked");
            _Logger.Information($"{nameof(RegisterMemberAsync)} {request.Name}");

            // Check if user already exists or not.
            bool userExists = await _UserAccess.CheckUserExistsAsync(request.Name).ConfigureAwait(false);

            string result = "Failed";
            if (!userExists)
            {
                result = await _UserAccess.CreateUserAsync(request.Name).ConfigureAwait(false);
            }

            // Do other stuff.....

            return $"\r\n    RegistrationEngine.RegisterMemberAsync -> {result}";
        }
    }
}
