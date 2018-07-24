using AutoMapper;
using Company.Api.Rest.Data;
using Company.Common.Data;
using Company.Manager.Membership.Interface;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Zametek.Utility;

namespace Company.Api.Rest.Service
{
    [Route("api/[controller]")]
    public class UsersController
        : Controller
    {
        private readonly IMapper _Mapper;
        private readonly IMembershipManager _MembershipManager;
        private readonly ILogger _Logger;

        public UsersController(
            IMapper mapper,
            IMembershipManager membershipManager,
            ILogger logger)
        {
            _Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _MembershipManager = membershipManager ?? throw new ArgumentNullException(nameof(membershipManager));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            Debug.Assert(TrackingContext.Current != null);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Post([FromBody]RegisterRequestDto requestDto)
        {
            _Logger.Information($"{nameof(Post)} Invoked");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var request = _Mapper.Map<RegisterRequest>(requestDto);
                string result = await _MembershipManager.RegisterMemberAsync(request).ConfigureAwait(false);
                if (!string.IsNullOrWhiteSpace(result))
                {
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _Logger.Error(ex, "Error caught in the controller class.");
            }
            return BadRequest(HttpStatusCode.BadRequest);
        }
    }
}
