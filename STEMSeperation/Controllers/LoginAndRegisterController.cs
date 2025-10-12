using DatabaseLayerLogic.Models;
using BusinessLayerLogic.ExternalProcesses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.ViewModels;
using PresentationLayer.Commons;
using System.Globalization;
using BusinessLayerLogic.Services;
using STEMSeperation.Helpers;
using Azure.Identity;
using BusinessLayerLogic.Services.Contracts;
using DatabaseLayerLogic.Security;
using Azure.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Google.Apis.Auth;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginAndRegisterController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IConfiguration _config;
        private readonly IConsoleAppRunner _consoleAppRunner;
        public LoginAndRegisterController(IUserService userService, IPasswordHasher passwordHasher, IConfiguration config, IConsoleAppRunner consoleAppRunner)
        {
            _userService = userService;
            _passwordHasher = passwordHasher;
            _config = config; 
            _consoleAppRunner = consoleAppRunner;
        }

        [HttpPost]
        [Route("SignUp")]
        public async Task<ActionResult<User>> Register([FromBody] RegisterVM registerAndLoginVM)
        {
            var IsExist = await _userService.GetUserByUsername(registerAndLoginVM.UserName);
            if (IsExist.Count!=0)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            var salt = _passwordHasher.GenerateSalt();
            var hash = _passwordHasher.GeneratePasswordHash(registerAndLoginVM.Password, salt);

            User user = new User
            {
                UserName = registerAndLoginVM.UserName,
                PasswordHash = hash,
                SaltValue = salt,
                UserCreatedOn = DateTime.Now,
                Email = registerAndLoginVM.Email,
                FirstName = registerAndLoginVM.FirstName, 
                LastName = registerAndLoginVM.LastName, 
            };
            try
            {
                await _userService.CreateUserAsync(user.UserName,user.PasswordHash,user.SaltValue,user.UserCreatedOn);
                return Ok(new Response { Status = "Success", Message = "User created successfully!" });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Internal Server Error" });
            }
            
        }

        [HttpPost]
        [Route("SignIn")]
        public async Task<ActionResult<User>> SignIn([FromBody] LoginVM signInBody)
        {
            string token = ""; 
            var user = await _userService.GetUserByUsername(signInBody.UserName);
            if (user.Count==0)
            {
                return NotFound();
            }
            else
            {
                var (storedHash, storedSalt) = await _userService.GetUserPasswordHashAndSaltValue(signInBody.UserName);
                bool hashVerified = _passwordHasher.VerifyPassword(signInBody.Password, storedSalt, storedHash);
                if (hashVerified)
                {
                    token = JwtTokenGenerator.GenerateJwtToken(user[0].UserName, _config);
                    return Ok(new { username = user[0].UserName, JwtToken = token });
                }
                else {
                    return NotFound();
                }
                //token = JwtTokenGenerator.GenerateJwtToken(user[0].UserName,_config); 
                //return Ok(new{username=user[0].UserName,JwtToken=token }); 
            }
        }


        [HttpPost("GoogleSignIn")]
        public async Task<IActionResult> GoogleSignIn([FromBody] GoogleSignInRequest request)
        {
            try
            {
                // Verify the Google ID token sent from React
                var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);

                string email = payload.Email;
                string name = payload.Name;

                // Use the new service method to get or create the user
                var user = await _userService.GetOrCreateGoogleUserAsync(
                    email,
                    name?.Split(' ').FirstOrDefault(),
                    name?.Split(' ').Skip(1).FirstOrDefault() ?? ""
                );

                // Generate JWT token
                var token = JwtTokenGenerator.GenerateJwtToken(email, _config);

                return Ok(new { JwtToken = token, Email = email });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Google token verification failed", Error = ex.Message });
            }
        }

        public class GoogleSignInRequest
        {
            public string IdToken { get; set; }
        }

    }
}
