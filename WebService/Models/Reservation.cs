/*
 * File: Reservation.cs
 * Author: Mohamed Farthas
 * Description: "EnergyReservation" collection — a Prosumer's actual booking
 *              against a BookingSlot. CreatedAt anchors the 7-day scheduling
 *              rule; SlotDateTime (denormalized from the slot at creation
 *              time) anchors the 12-hour update/cancel notice rule so it can
 *              be checked without a second lookup.
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebService.Models
{
    public enum ReservationStatus
    {
        Pending,
        Approved,
        Completed,
        Cancelled
    }

    public class Reservation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        // FK -> User.NIC (the Prosumer who booked it)
        [BsonElement("prosumerNIC")]
        public string ProsumerNIC { get; set; } = string.Empty;

        // FK -> Station.Id
        [BsonElement("stationId")]
        public string StationId { get; set; } = string.Empty;

        // FK -> BookingSlot.Id
        [BsonElement("slotId")]
        public string SlotId { get; set; } = string.Empty;

        // Denormalized from the slot at booking time so the 12-hour rule can be
        // checked directly against the reservation without re-fetching the slot.
        [BsonElement("slotDateTime")]
        public DateTime SlotDateTime { get; set; }

        [BsonElement("requestedEnergyKwh")]
        public double RequestedEnergyKwh { get; set; }

        [BsonElement("status")]
        [BsonRepresentation(BsonType.String)]
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

        // Needed to enforce "must be scheduled within 7 days" on creation.
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Generated once the reservation is Approved; scanned by a Grid Operator.
        [BsonElement("transactionQrCode")]
        public string? TransactionQrCode { get; set; }

        // Optional — audit trail of which operator completed the transfer.
        [BsonElement("finalizedByOperatorId")]
        public string? FinalizedByOperatorId { get; set; }

        [BsonElement("actualEnergyTransferredKwh")]
        public double? ActualEnergyTransferredKwh { get; set; }

        [BsonElement("notes")]
        public string? Notes { get; set; }
    }
}
