using BomDeBoca.Client.services.interfaces;
using BomDeBoca.Shared.Entities;
using BomDeBoca.Server.model;
using BomDeBoca.Shared.dto;
using BomDeBoca.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;
using System.Text.Json;

namespace BomDeBoca.Server.Controllers {
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signManager;
        private readonly IConfiguration _configuration;
        private readonly AppDBContext _context;

        public AuthenticationController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signManager,
            IConfiguration configuration,
            AppDBContext context)
        {
            _userManager = userManager;
            _signManager = signManager;
            _configuration = configuration;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LocalStorageDTO>> LoginUser([FromBody] LoginDTO user) {
            //var result = await _signManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);
            object _user;
            if (user.UserType == UserType.CLIENT)
                _user = await _context.Clients.FirstOrDefaultAsync(u => u.CPF == user.CpfCnpj && u.Password == user.Password);
            else
                _user = await _context.Companies.FirstOrDefaultAsync(u => u.CNPJ == user.CpfCnpj && u.Password == user.Password);
            if (_user != null)
            {
                Token token = GenerateToken(_user);
                return new LocalStorageDTO() { Token = token, User = _user };
            }
            return BadRequest(new { message = "Login Inválido" });
        }

        private Token GenerateToken(object user)
        {
            List<Claim> claims;
            if (user is Shared.Entities.Client)
            {
                var clientJson = JsonSerializer.Serialize((Shared.Entities.Client)user);
                claims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, ((Shared.Entities.Client)user).ID.ToString()),
                    new Claim("client",clientJson),
                    new Claim(ClaimTypes.Role, "Client"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
            }
            else
            {
                var companyJson = JsonSerializer.Serialize((Shared.Entities.Company)user);
                claims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, ((Shared.Entities.Company)user).ID.ToString()),
                    new Claim("company",companyJson),
                    new Claim(ClaimTypes.Role, "Company"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(2);
            var message = "Token JWT criado com sucesso";

            JwtSecurityToken token = new JwtSecurityToken
            (
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new Token()
            {
                Code = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                Message = message
            };
        }
    }
}
