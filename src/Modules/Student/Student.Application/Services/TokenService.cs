using Core.Application.Options;
using MassTransit;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Student.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtOptions _jwtOptions;

        public TokenService(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string GenerateToken(string username, Guid id, string email, string role)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim("UserName", username),
                new Claim("Email", email),
                new Claim("Role", role),
                new Claim("Id",id.ToString())
            });

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateJwtSecurityToken(issuer: _jwtOptions.Issuer, audience: _jwtOptions.Audience, subject: claimsIdentity, notBefore: DateTime.Now, expires: DateTime.Now.AddHours(1), signingCredentials: creds);

            return tokenHandler.WriteToken(token);
        }
    }
}
