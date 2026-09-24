/*
 * File: MongoDbSettings.cs
 * Author: Mohamed Farthas
 * Description: Strongly-typed binding for the "MongoDbSettings" section in
 *              appsettings.json / appsettings.Development.json. Injected via
 *              IOptions<MongoDbSettings> wherever the connection details are needed.
 */

namespace WebService.Models
{
    public class MongoDbSettings
    {
        // Mongo connection URI (Atlas SRV string in Development, placeholder in the committed base file).
        public string ConnectionString { get; set; } = string.Empty;

        // Name of the database that holds all four collections (Users, SolarStationInfo, EnergyBookingSlots, EnergyReservation).
        public string DatabaseName { get; set; } = string.Empty;
    }
}
