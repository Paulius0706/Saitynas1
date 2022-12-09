using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TimeT.Auth.Model.AuthDtos;
using TimeT.Auth;
using TimeT.Auth.Model;
using TimeT.Data.Entities;

namespace TimeT.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<TimeTUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(UserManager<TimeTUser> userManager, IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("register", Name ="RegisterUser")]
        public async Task<ActionResult> Register(RegisterUserDto registerUserDto)
        {
            var user = await _userManager.FindByNameAsync(registerUserDto.UserName);
            if (user != null) return BadRequest("Request invalid.");

            var newUser = new TimeTUser
            {
                Email = registerUserDto.Email,
                UserName = registerUserDto.UserName
            };
            var createUserResult = await _userManager.CreateAsync(newUser, registerUserDto.Password);
            if (!createUserResult.Succeeded)
                return BadRequest("Could not create a user.");

            // add choice between service user and client user
            await _userManager.AddToRoleAsync(newUser, UserRoles.ServiceUser);
            return Ok();
            return CreatedAtAction(
                $"api/login/",
                new LoginDto(newUser.UserName, registerUserDto.Password)
                );

        }

        [HttpPost("login", Name = "LoginUser")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null) return BadRequest("User name or password is invalid.");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid) return BadRequest("User name or password is invalid.");
            
            // valid user
            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _jwtTokenService.CreateAccessToken(user.UserName, user.Id, roles);

            return Ok(new SuccessfulLoginDto(accessToken));
        }
    }
}
