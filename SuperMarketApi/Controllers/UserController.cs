using Microsoft.AspNetCore.Mvc;
using SuperMarketApi.Services;
using AutoMapper;
using SuperMarketApi.Filters;
using SuperMarketApi.Models;

namespace SuperMarketApi.Controllers
{
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

        // Example endpoint to test authorization
        [HttpGet("test-admin")]
        [Authorization(RoleEnum.Admin)]
        public IActionResult TestAdmin()
        {
            return Ok("You are authorized as Admin!");
        }

        // Endpoint for user login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginUserDto loginUserDto)
        {
            try
            {
                var token = _userService.Login(loginUserDto);
                return Ok(new { Token = token });
            }
            catch (BadHttpRequestException ex)
            {
                return Unauthorized(new { Error = ex.Message });
            }
        }
    }
} 