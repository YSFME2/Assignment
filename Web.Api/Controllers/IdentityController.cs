using Web.Contracts.v1.Responses.Identity;
using Web.Contracts.v1.Requests.Identity;

namespace Web.Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityServices _identityServices;
        private readonly IMapper _mapper;
        public IdentityController(IIdentityServices identityServices, IMapper mapper)
        {
            _identityServices = identityServices;
            _mapper = mapper;
        }


        /// <summary>
        /// Register with User role
        /// </summary>
        [HttpPost(ApiRoutes.Identity.Register)]
        [ProducesResponseType<AuthenticationResponse>(200)]
        [ProducesResponseType<List<ErrorResponse>>(400)]
        public async Task<IActionResult> RegisterAsync(RegisterRequest register)
        {
            var result = await _identityServices.RegisterAsync(register.ProfileName, register.Email, register.Password, register.Phone);

            if (result.IsSuccess)
            {
                AppendRefreshTokenToCookies(result.RefreshToken, result.RefreshTokenExpiration);
                return Ok(_mapper.Map<AuthenticationResponse>(result));
            }

            return BadRequest(_mapper.Map<List<ErrorResponse>>(result.Errors));
        }



        /// <summary>
        /// Register with Admin role
        /// </summary>
        [HttpPost(ApiRoutes.Identity.RegisterAsAdmin)]
        [ProducesResponseType<AuthenticationResponse>(200)]
        [ProducesResponseType<List<ErrorResponse>>(400)]
        public async Task<IActionResult> RegisterAsAdminAsync(RegisterRequest register)
        {
            var result = await _identityServices.RegisterAsAdminAsync(register.ProfileName, register.Email, register.Password, register.Phone);

            if (result.IsSuccess)
            {
                AppendRefreshTokenToCookies(result.RefreshToken, result.RefreshTokenExpiration);
                return Ok(_mapper.Map<AuthenticationResponse>(result));
            }

            return BadRequest(_mapper.Map<List<ErrorResponse>>(result.Errors));
        }



        /// <summary>
        /// Login user
        /// </summary>
        [HttpPost(ApiRoutes.Identity.Login)]
        [ProducesResponseType<AuthenticationResponse>(200)]
        [ProducesResponseType<List<ErrorResponse>>(400)]
        public async Task<IActionResult> LoginAsync(LoginRequest login)
        {
            var result = await _identityServices.LoginAsync(login.Email, login.Password);

            if (result.IsSuccess)
            {
                AppendRefreshTokenToCookies(result.RefreshToken, result.RefreshTokenExpiration);
                return Ok(_mapper.Map<AuthenticationResponse>(result));
            }

            return BadRequest(_mapper.Map<List<ErrorResponse>>(result.Errors));
        }



        /// <summary>
        ///  Create new Token using Refresh Token
        /// </summary>
        [HttpPost(ApiRoutes.Identity.RefreshToken)]
        [ProducesResponseType<AuthenticationResponse>(200)]
        [ProducesResponseType<List<ErrorResponse>>(400)]
        public async Task<IActionResult> RefreshTokenAsync(RefreshTokenRequest? request = null)
        {
            var token = request?.RefreshToken ?? Request.Cookies["RT"];
            var result = await _identityServices.RefreshTokenAsync(token);

            if (result.IsSuccess)
            {
                AppendRefreshTokenToCookies(result.RefreshToken, result.RefreshTokenExpiration);
                return Ok(_mapper.Map<AuthenticationResponse>(result));
            }

            return BadRequest(_mapper.Map<List<ErrorResponse>>(result.Errors));
        }



        /// <summary>
        /// Revoke Refresh Token
        /// </summary>
        [HttpPost(ApiRoutes.Identity.RevokeRefreshToken)]
        [ProducesResponseType<AuthenticationResponse>(200)]
        [ProducesResponseType<List<ErrorResponse>>(400)]
        public async Task<IActionResult> RevokeRefreshTokenAsync(RefreshTokenRequest? request = null)
        {
            var token = request?.RefreshToken ?? Request.Cookies["RT"];
            var result = await _identityServices.RevokeRefreshTokenAsync(token);

            return result.IsSuccess ?
                Ok(_mapper.Map<AuthenticationResponse>(result))
                : BadRequest(_mapper.Map<List<ErrorResponse>>(result.Errors));
        }



        /// <summary>
        ///  Add Role to User
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost(ApiRoutes.Identity.AddUserToRole)]
        [ProducesResponseType<ChangeUserRoleResponse>(200)]
        [ProducesResponseType<List<ErrorResponse>>(400)]
        public async Task<IActionResult> AddUserToRoleAsync(ChangeUserRoleRequest request)
        {
            var result = await _identityServices.AssignRoleAsync(request.UserId, request.Role);

            return result.IsSuccess ?
                Ok(_mapper.Map<ChangeUserRoleResponse>(result))
                : BadRequest(_mapper.Map<List<ErrorResponse>>(result.Errors));
        }




        /// <summary>
        ///  Remove Role from User
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost(ApiRoutes.Identity.RemoveUserFromRole)]
        [ProducesResponseType<ChangeUserRoleResponse>(200)]
        [ProducesResponseType<List<ErrorResponse>>( 400)]
        public async Task<IActionResult> RemoveUserFromRoleAsync(ChangeUserRoleRequest request)
        {
            var result = await _identityServices.RemoveRoleAsync(request.UserId, request.Role);

            return result.IsSuccess ?
                Ok(_mapper.Map<ChangeUserRoleResponse>(result))
                : BadRequest(_mapper.Map<List<ErrorResponse>>(result.Errors));
        }


        private void AppendRefreshTokenToCookies(string refreshToken, DateTime expiration)
        {
            Response.Cookies.Append("RT", refreshToken, new CookieOptions { Expires = expiration });
        }
    }
}
