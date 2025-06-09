using DatabaseLayerLogic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.ViewModels;
using PresentationLayer.Commons;
using System.Globalization;

namespace PresentationLayer.Controllers
{
    public class LoginAndRegisterController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        public LoginAndRegisterController(UserManager<User> userManager) 
        { 
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<User>> Register([FromBody]RegisterAndLoginVM registerAndLoginVM)
        {
            var IsExist = await userManager.FindByNameAsync(registerAndLoginVM.UserName);
            if (IsExist != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
            User user = new User
            {
                UserName = registerAndLoginVM.UserName,
                Password = registerAndLoginVM.Password
            };
            var result = await userManager.CreateAsync(user, registerAndLoginVM.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }
    }
}
