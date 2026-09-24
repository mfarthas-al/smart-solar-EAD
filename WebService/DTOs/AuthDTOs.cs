/*
 * File: AuthDTOs.cs
 * Author: Mohamed Farthas
 * Description: Request/response shapes for the login endpoint. Kept separate
 *              from the User model so the password hash and other internal
 *              fields are never accidentally serialized back to a client.
 */

namespace WebService.DTOs
{
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? NIC { get; set; }
    }
}
