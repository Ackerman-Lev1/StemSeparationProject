﻿using DatabaseLayerLogic.Models;
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
        public async Task<ActionResult<User>> Register([FromBody] RegisterAndLoginVM registerAndLoginVM)
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
                UserCreatedOn = DateTime.Now
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
        public async Task<ActionResult<User>> SignIn([FromBody] RegisterAndLoginVM signInBody)
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

    }
}
