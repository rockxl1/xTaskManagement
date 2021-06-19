using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xTask.Core.Interfaces;
using xTask.Infrastructure.Identity;
using xTask.WebAPI.Entities;

namespace xTask.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        ITokenClaimsService _tokenService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(ITokenClaimsService tokenService, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] Register model)
        {
            var result = await _userManager.CreateAsync(new ApplicationUser()
            {
                LockoutEnabled = false,
                Email = model.Email,
                UserName = model.LoginName
            }, model.Password);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors.ToString());
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticateResponse>> Login([FromBody] LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, true, false);
            AuthenticateResponse response = new AuthenticateResponse();
            if (result.Succeeded)
            {
                var expires = DateTime.Now.AddDays(1);
                string token = await _tokenService.GetTokenAsync(model.Username, expires);
                response.Token = token;
                response.Expires = expires;
                return Ok(response);
            }
            else
            {
                return Unauthorized();
            }
        }

    }
}
