/*
 * File: JwtSettings.cs
 * Author: Mohamed Farthas
 * Description: Strongly-typed binding for the "JwtSettings" section in
 *              appsettings.json — used to sign and validate login tokens.
 */

namespace WebService.Models
{
    public class JwtSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpiryDays { get; set; } = 7;
    }
}
