/*
 * File: ProsumerDTOs.cs
 * Author: Mohamed Farthas
 * Description: Request/response shapes for the Prosumer endpoints. Kept
 *              separate from the User model so PasswordHash is never
 *              accidentally serialized back to a client.
 */

namespace WebService.DTOs
{
    public class RegisterProsumerRequest
    {
        public string NIC { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string PropertyAddress { get; set; } = string.Empty;
        public double? SolarPanelCapacityKw { get; set; }
    }

    public class UpdateProsumerRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string PropertyAddress { get; set; } = string.Empty;
        public double? SolarPanelCapacityKw { get; set; }
    }

    public class ProsumerResponse
    {
        public string Id { get; set; } = string.Empty;
        public string NIC { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string PropertyAddress { get; set; } = string.Empty;
        public double? SolarPanelCapacityKw { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
