/*
 * File: BookingSlot.cs
 * Author: Mohamed Farthas
 * Description: "EnergyBookingSlots" collection — the timetable of bookable
 *              time windows a Station publishes. A Reservation is always made
 *              against one of these slots, not directly against a Station.
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebService.Models
{
    public enum SlotStatus
    {
        Available,
        FullyBooked,
        Closed
    }

    public class BookingSlot
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        // FK -> Station.Id
        [BsonElement("stationId")]
        public string StationId { get; set; } = string.Empty;

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [BsonElement("startTime")]
        public string StartTime { get; set; } = string.Empty; // "HH:mm"

        [BsonElement("endTime")]
        public string EndTime { get; set; } = string.Empty;   // "HH:mm"

        [BsonElement("availableCapacityKwh")]
        public double AvailableCapacityKwh { get; set; }

        [BsonElement("status")]
        [BsonRepresentation(BsonType.String)]
        public SlotStatus Status { get; set; } = SlotStatus.Available;
    }
}
