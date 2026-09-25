/*
 * File: ProsumersController.cs
 * Author: Mohamed Farthas
 * Description: Prosumer registration and account management. Covers the
 *              mobile-side "Prosumer Account Control" features (register,
 *              edit own profile, request deactivation) and the Backoffice
 *              web-side "Prosumer Management" features (list, update,
 *              activate/reactivate, deactivate). New registrations start
 *              as Pending and only a Backoffice user can activate them.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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
    public class ProsumersController : ControllerBase
    {
        private readonly IMongoCollection<User> _users;

        public ProsumersController(MongoDbService mongoDbService)
        {
            _users = mongoDbService.GetCollection<User>("Users");
        }

        // Maps a User document to the public-facing DTO so PasswordHash never leaves the API.
        private static ProsumerResponse ToResponse(User user) => new()
        {
            Id = user.Id ?? string.Empty,
            NIC = user.NIC ?? string.Empty,
            FullName = user.FullName,
            Email = user.Email,
            ContactNumber = user.ContactNumber,
            PropertyAddress = user.PropertyAddress ?? string.Empty,
            SolarPanelCapacityKw = user.SolarPanelCapacityKw,
            Status = user.Status.ToString(),
            CreatedAt = user.CreatedAt
        };

        // Mobile self-registration. NIC must be unique - it's the business
        // primary key for prosumers even though Mongo's own _id stays an
        // ObjectId. New accounts start Pending until a Backoffice user
        // activates them.
        [HttpPost("register")]
        public async Task<ActionResult<ProsumerResponse>> Register([FromBody] RegisterProsumerRequest request)
        {
            var existing = await _users.Find(u => u.NIC == request.NIC).FirstOrDefaultAsync();
            if (existing != null)
            {
                return Conflict(new { message = "A prosumer with this NIC is already registered." });
            }

            var prosumer = new User
            {
                NIC = request.NIC,
                FullName = request.FullName,
                Email = request.Email,
                ContactNumber = request.ContactNumber,
                PropertyAddress = request.PropertyAddress,
                SolarPanelCapacityKw = request.SolarPanelCapacityKw,
                Role = UserRole.Prosumer,
                Status = UserStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            var hasher = new PasswordHasher<User>();
            prosumer.PasswordHash = hasher.HashPassword(prosumer, request.Password);

            await _users.InsertOneAsync(prosumer);

            return Ok(ToResponse(prosumer));
        }

        // Backoffice-only: list all prosumers, optionally filtered by status
        // (e.g. ?status=Pending for the "pending activations" view).
        [HttpGet]
        [Authorize(Roles = "Backoffice")]
        public async Task<ActionResult<List<ProsumerResponse>>> GetAll([FromQuery] string? status)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Role, UserRole.Prosumer);
            if (!string.IsNullOrEmpty(status) && Enum.TryParse<UserStatus>(status, true, out var parsedStatus))
            {
                filter &= Builders<User>.Filter.Eq(u => u.Status, parsedStatus);
            }

            var prosumers = await _users.Find(filter).ToListAsync();
            return Ok(prosumers.Select(ToResponse));
        }

        // Fetch one prosumer's profile by NIC.
        [HttpGet("{nic}")]
        [Authorize]
        public async Task<ActionResult<ProsumerResponse>> GetByNic(string nic)
        {
            var prosumer = await _users.Find(u => u.NIC == nic && u.Role == UserRole.Prosumer).FirstOrDefaultAsync();
            if (prosumer == null)
            {
                return NotFound();
            }

            return Ok(ToResponse(prosumer));
        }

        // Edit profile fields. Allowed for the prosumer themselves (mobile
        // "edit profile") or a Backoffice user (web "update" profile).
        [HttpPut("{nic}")]
        [Authorize]
        public async Task<IActionResult> Update(string nic, [FromBody] UpdateProsumerRequest request)
        {
            if (!IsSelfOrBackoffice(nic))
            {
                return Forbid();
            }

            var update = Builders<User>.Update
                .Set(u => u.FullName, request.FullName)
                .Set(u => u.ContactNumber, request.ContactNumber)
                .Set(u => u.PropertyAddress, request.PropertyAddress)
                .Set(u => u.SolarPanelCapacityKw, request.SolarPanelCapacityKw);

            var result = await _users.UpdateOneAsync(u => u.NIC == nic && u.Role == UserRole.Prosumer, update);
            if (result.MatchedCount == 0)
            {
                return NotFound();
            }

            return NoContent();
        }

        // Sets status to Deactivated. Called from the mobile app's "request
        // deactivation" button (self) or the Backoffice web "deactivate"
        // action - same effect either way, so one endpoint covers both.
        [HttpPatch("{nic}/deactivate")]
        [Authorize]
        public async Task<IActionResult> Deactivate(string nic)
        {
            if (!IsSelfOrBackoffice(nic))
            {
                return Forbid();
            }

            var update = Builders<User>.Update.Set(u => u.Status, UserStatus.Deactivated);
            var result = await _users.UpdateOneAsync(u => u.NIC == nic && u.Role == UserRole.Prosumer, update);
            if (result.MatchedCount == 0)
            {
                return NotFound();
            }

            return NoContent();
        }

        // Backoffice-only: activates a Pending registration, or reactivates
        // a previously Deactivated account - both are just "set to Active",
        // per the brief ("Deactivated accounts can only be reactivated by a
        // Backoffice officer").
        [HttpPatch("{nic}/activate")]
        [Authorize(Roles = "Backoffice")]
        public async Task<IActionResult> Activate(string nic)
        {
            var update = Builders<User>.Update.Set(u => u.Status, UserStatus.Active);
            var result = await _users.UpdateOneAsync(u => u.NIC == nic && u.Role == UserRole.Prosumer, update);
            if (result.MatchedCount == 0)
            {
                return NotFound();
            }

            return NoContent();
        }

        // True if the logged-in caller is the prosumer identified by nic,
        // or a Backoffice user acting on someone else's profile.
        private bool IsSelfOrBackoffice(string nic)
        {
            var callerNic = User.FindFirstValue("nic");
            var callerRole = User.FindFirstValue(ClaimTypes.Role);
            return callerNic == nic || callerRole == "Backoffice";
        }
    }
}
