/*
 * File: User.cs
 * Author: Mohamed Farthas
 * Description: Single "Users" collection covering all three actor types
 *              (Backoffice, GridOperator, Prosumer), distinguished by Role.
 *              Prosumer-only fields are nullable since Backoffice/GridOperator
 *              documents don't use them, and vice versa for AssignedStationId.
 *
 *              Design note: MongoDB's own _id stays an auto-generated ObjectId
 *              for every document (simplest, driver-default behaviour). NIC is
 *              kept as a separate, required-and-unique field for Prosumers and
 *              is what the API/business logic treats as their primary lookup
 *              key (e.g. GET /api/prosumers/{nic}) — i.e. "NIC as primary key"
 *              is enforced at the business/uniqueness level, not by replacing
 *              Mongo's internal _id.
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebService.Models
{
    public enum UserRole
    {
        Backoffice,
        GridOperator,
        Prosumer
    }

    public enum UserStatus
    {
        Pending,     // Prosumer just registered, awaiting Backoffice activation
        Active,
        Deactivated
    }

    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        // --- Fields common to every user type ---
        [BsonElement("fullName")]
        public string FullName { get; set; } = string.Empty;

        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("passwordHash")]
        public string PasswordHash { get; set; } = string.Empty;

        [BsonElement("contactNumber")]
        public string ContactNumber { get; set; } = string.Empty;

        [BsonElement("role")]
        [BsonRepresentation(BsonType.String)]
        public UserRole Role { get; set; }

        [BsonElement("status")]
        [BsonRepresentation(BsonType.String)]
        public UserStatus Status { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // --- Prosumer-only fields (null for Backoffice/GridOperator) ---
        [BsonElement("nic")]
        public string? NIC { get; set; }

        [BsonElement("propertyAddress")]
        public string? PropertyAddress { get; set; }

        [BsonElement("solarPanelCapacityKw")]
        public double? SolarPanelCapacityKw { get; set; }

        // --- GridOperator-only field (null for other roles) ---
        [BsonElement("assignedStationId")]
        public string? AssignedStationId { get; set; }
    }
}
