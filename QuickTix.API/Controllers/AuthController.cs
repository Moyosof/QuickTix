using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HouseMate.API.Entities.DTOs;
using HouseMate.API.Filters.ActionFilters;
using HouseMate.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using HouseMate.API.Entities.Enums;
using RTools_NTS.Util;

namespace HouseMate.API.Controllers
{
    [Route("api/v1/[controller]")]
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

        [HttpPost("register_user")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto userRegistration)
        {
            var userResult = await _userAuth.RegisterUserAsync(userRegistration);
            if(userResult.Succeeded)
            {
                var userDetails = await _userAuth.GetUserByUsername(userRegistration.Email);
                return Ok(new {User = userDetails });

                //return Ok(userResult);
            }
            return BadRequest(userResult);
        }


        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginDto user)
        {
            var userLogin = await _userAuth.ValidateUserAsync(user);
            if (userLogin.result == LoginResult.Success)
            {
                var userDetails = await _userAuth.GetUserByUsername(user.Email);
                var token = await _userAuth.CreateTokenAsync(user);
                return Ok(new { Token = token, User = userDetails });
            }

            return BadRequest(userLogin.message);
            //return !await _userAuth.ValidateUserAsync(user)
            //    ? Unauthorized() : Ok(new { Token = await _userAuth.CreateTokenAsync() });
        }


    }
}
