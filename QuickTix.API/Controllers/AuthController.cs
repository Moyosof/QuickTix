using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickTix.API.Entities.DTOs;
using QuickTix.API.Filters.ActionFilters;
using QuickTix.API.Repositories.Interfaces;

namespace QuickTix.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserAuth _userAuth;
        private readonly ILoggerManager _loggerManager;
        private readonly IMapper _mapper;
        public AuthController(IUserAuth userAuth, ILoggerManager loggerManager, IMapper mapper)
        {
            _userAuth = userAuth;
            _loggerManager = loggerManager;
            _mapper = mapper;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto userRegistration)
        {
            var userResult = await _userAuth.RegisterUserAsync(userRegistration);
            return !userResult.Succeeded ? new BadRequestObjectResult(userResult) : StatusCode(201);
        }


        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserLoginDto user)
        {
            return !await _userAuth.ValidateUserAsync(user)
                ? Unauthorized() : Ok(new { Token = await _userAuth.CreateTokenAsync() });
        }


    }
}
