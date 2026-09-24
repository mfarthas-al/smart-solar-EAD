/*
 * File: AuthController.cs
 * Author: Mohamed Farthas
 * Description: Shared login endpoint used by every client (Backoffice web,
 *              Grid Operator web + mobile, Prosumer mobile). One endpoint,
 *              role is read from the stored user document and returned in
 *              the token/response so each client can redirect accordingly.
 */

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using WebService.DTOs;
using WebService.Models;
using WebService.Services;

namespace WebService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMongoCollection<User> _users;
        private readonly JwtTokenService _jwtTokenService;

        public AuthController(MongoDbService mongoDbService, JwtTokenService jwtTokenService)
        {
            _users = mongoDbService.GetCollection<User>("Users");
            _jwtTokenService = jwtTokenService;
        }

        // Validates email + password against the Users collection and, on
        // success, issues a JWT the client stores and sends on every later
        // request (Authorization: Bearer <token>).
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var user = await _users.Find(u => u.Email == request.Email).FirstOrDefaultAsync();
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            if (user.Status != UserStatus.Active)
            {
                return Unauthorized(new { message = $"Account is {user.Status}. Contact a Backoffice officer." });
            }

            var hasher = new PasswordHasher<User>();
            var verifyResult = hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (verifyResult == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            var token = _jwtTokenService.GenerateToken(user);

            return Ok(new LoginResponse
            {
                Token = token,
                UserId = user.Id ?? string.Empty,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString(),
                NIC = user.NIC
            });
        }
    }
}
