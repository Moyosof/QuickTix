using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickTix.API.Entities.DTOs;
using QuickTix.API.Entities.Services;
using QuickTix.API.Filters.ActionFilters;
using QuickTix.API.Helpers;
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
        private readonly IEmailClient _emailClient;
        public AuthController(IUserAuth userAuth, ILoggerManager loggerManager, IMapper mapper, IEmailClient emailClient)
        {
            _userAuth = userAuth;
            _loggerManager = loggerManager;
            _mapper = mapper;
            _emailClient = emailClient;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<object> RegisterUser([FromBody] UserRegistrationDto userRegistration)
        {
            var userResult = await _userAuth.RegisterUserAsync(userRegistration);

            // create OTP for user
            string userOTP = Util.GenerateRandomDigits(6);
            string hashedOTP = Util.HashOTP(userOTP);

            // send confirm email otp
            string mailSubject = "Confirm Your Email - QuickTix";
            string mailBody = "Enter this OTP: " + userOTP + " to verify your email";

            MailRequestService mailRequest = new MailRequestService()
            {
                To = userRegistration.Email,
                Subject = mailSubject,
                Body = mailBody
            };

            await _emailClient.SendEmailAsync(mailRequest);

            return "Confirm your email";
            //return !userResult.Succeeded ? new BadRequestObjectResult(userResult) : StatusCode(201);
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
