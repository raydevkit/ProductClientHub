using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductClientHub.API.Services
{
    public class JwtTokenService
    {
        private readonly string _key;
        private readonly string? _issuer;
        private readonly string? _audience;
        private readonly int _minutes;

        public JwtTokenService()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            _key = config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured");
            _issuer = config["Jwt:Issuer"];
            _audience = config["Jwt:Audience"];
            _minutes = int.TryParse(config["Jwt:AccessTokenMinutes"], out var m) ? m : 60;
        }

        public (string Token, DateTime ExpiresAtUtc) CreateToken(Guid userId, string email, string name, string role)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new(JwtRegisteredClaimNames.Email, email),
                new("name", name),
                new(ClaimTypes.Role, role),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_minutes);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }
    }
}