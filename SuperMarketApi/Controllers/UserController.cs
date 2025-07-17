using Microsoft.AspNetCore.Mvc;
using SuperMarketApi.Services;
using AutoMapper;
using SuperMarketApi.Filters;
using SuperMarketApi.Models;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.Contracts;
using System.Security.Claims;
using SuperMarketApi.DTOs.User;

namespace SuperMarketApi.Controllers
{
    // This controller demonstrates JWT authentication in ASP.NET Core.
    // - [Authorize] is used to protect endpoints (requires a valid JWT in the Authorization header).
    // - [AllowAnonymous] is used to allow unauthenticated access (e.g., for login/register).
    // - The login endpoint issues a JWT token on successful authentication.
    // - The token must be included as a Bearer token in the Authorization header for protected endpoints.
    //
    // Example usage:
    //   Authorization: Bearer <your-jwt-token>
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        //Endpoint for user register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(CreateUserDto newUser)
        {
            try
            {
                await _userService.RegisterAsync(newUser);

                // Automatically log in the user after registration
                var loginDto = new LoginUserDto
                {
                    UserName = newUser.UserName,
                    Password = newUser.Password
                };
                var token = await _userService.LoginAsync(loginDto);
                return Ok(new { Token = token });
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Endpoint for user login
        [HttpPost("login", Name = "Login")]
        [AllowAnonymous] // Allow unauthenticated users to login
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            try
            {
                var token = await _userService.LoginAsync(loginUserDto);
                // Return the JWT token to the client
                return Ok(new { Token = token });
            }
            catch (BadHttpRequestException ex)
            {
                return Unauthorized(new { Error = ex.Message });
            }
        }


        [HttpPost("update-user-info")]
        [Authorize]
        public async Task<IActionResult> UpdateUserInfo(UpdateUserInfoDto updateUser)
        {
            var userIdClaim = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim);
            await _userService.UpdatePersonalInfoAsync(userId, updateUser);
            return NoContent();

        }


        [HttpPost("change-user-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeUserRole(ChangeUserRoleDto changeRoleDto)
        {
            // Only Admins can change user roles
            try
            {
                await _userService.ChangeUserRoleAsync(changeRoleDto);
                return NoContent();
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Change the password of the currently logged-in user.
        /// Request body must include CurrentPassword, NewPassword, and RepeatNewPassword.
        /// </summary>
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var userIdClaim = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim);
            try
            {
                await _userService.ChangePasswordAsync(userId, dto);
                return NoContent();
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-user-info")]
        [Authorize]
        public async Task<IActionResult> GetMyInfo()
        {
            var userIdClaim = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim);
            var user = await _userService.GetPersonalInfoAsync(userId);
            if (user == null) return NotFound();
            return Ok(user);
        }
    }
}