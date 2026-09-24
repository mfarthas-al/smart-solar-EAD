/*
 * File: JwtTokenService.cs
 * Author: Mohamed Farthas
 * Description: Issues signed JWTs for successful logins. The token carries
 *              the user's id, role, and (for Prosumers) NIC as claims, so
 *              controllers can authorize by role without a DB round-trip.
 *              A 7-day default expiry keeps a browser/device logged in
 *              across refreshes and restarts during development and normal use.
 */

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebService.Models;

namespace WebService.Services
{
    public class JwtTokenService
    {
        private readonly JwtSettings _settings;

        public JwtTokenService(IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
        }

        // Builds a signed JWT for the given user. Called once, right after a
        // successful password check in AuthController.
        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id ?? string.Empty),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.Role.ToString()),
                new("fullName", user.FullName)
            };

            if (!string.IsNullOrEmpty(user.NIC))
            {
                claims.Add(new Claim("nic", user.NIC));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_settings.ExpiryDays),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
