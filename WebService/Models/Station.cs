/*
 * File: Station.cs
 * Author: Mohamed Farthas
 * Description: "SolarStationInfo" collection — a microgrid node / solar hub.
 *              Owned/managed by Backoffice; battery slot availability is
 *              updated by the assigned Grid Operator.
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebService.Models
{
    public enum StationStatus
    {
        Active,
        Inactive
    }

    // Embedded document — GPS coordinates of a station.
    public class GeoLocation
    {
        [BsonElement("latitude")]
        public double Latitude { get; set; }

        [BsonElement("longitude")]
        public double Longitude { get; set; }
    }

    // Embedded document — the days/hours a station is open for trading.
    public class OperatingSchedule
    {
        [BsonElement("openTime")]
        public string OpenTime { get; set; } = "08:00";

        [BsonElement("closeTime")]
        public string CloseTime { get; set; } = "18:00";

        [BsonElement("operatingDays")]
        public List<string> OperatingDays { get; set; } = new();
    }

    public class Station
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("location")]
        public GeoLocation Location { get; set; } = new();

        [BsonElement("capacityKw")]
        public double CapacityKw { get; set; }

        [BsonElement("totalBatterySlots")]
        public int TotalBatterySlots { get; set; }

        [BsonElement("availableBatterySlots")]
        public int AvailableBatterySlots { get; set; }

        [BsonElement("schedule")]
        public OperatingSchedule Schedule { get; set; } = new();

        [BsonElement("status")]
        [BsonRepresentation(BsonType.String)]
        public StationStatus Status { get; set; } = StationStatus.Active;

        // Optional — which Grid Operator is responsible for this station.
        [BsonElement("assignedOperatorId")]
        public string? AssignedOperatorId { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
