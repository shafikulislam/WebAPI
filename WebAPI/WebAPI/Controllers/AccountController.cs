using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Interfaces;
using WebAPI.Models.ViewModel;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private IAccountService _service;
        public IConfiguration _configuration;

        public AccountController(IConfiguration configuration, IAccountService service)
        {
            _configuration = configuration;
            _service = service;
        }


        // POST: api/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("Register")]
        public async Task<ActionResult<UserInfoViewModel>> Register(RegisterViewModel userInfo)
        {
            var userLogged = await _service.Register(userInfo);

			if (userLogged != null)
			{
                return await Login(new LoginViewModel() { Email = userLogged.Email, Password = userLogged.Password });
			}
			else
			{
                return NotFound();
			}

        }


        // POST: api/Login
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("Login")]
        public async Task<ActionResult<UserInfoViewModel>> Login(LoginViewModel userInfo)
        {
            var userLogged = await _service.Login(userInfo);

            if (userLogged != null)
            {
                //create claims details based on the user information
                var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Email", userLogged.Email),
                    new Claim("Password", userLogged.Password)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

                new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }
            else
            {
                return NotFound();
            }

        }

    }
}
