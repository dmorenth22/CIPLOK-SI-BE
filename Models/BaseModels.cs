using System.Text.Json.Serialization;

namespace CIPLOK_SI_BE.Models
{
    public class BaseModels
    {
        [JsonIgnore]
        [JsonPropertyName("CREATED_BY")]
        public string? CREATED_BY { get; set; }


        [JsonIgnore]
        [JsonPropertyName("CREATED_DATE")]
        public DateTime? CREATED_DATE { get; set; }

        [JsonIgnore]
        [JsonPropertyName("LAST_UPDATED_BY")]
        public string? LAST_UPDATED_BY { get; set; }

        [JsonIgnore]
        [JsonPropertyName("LAST_UPDATED_DATE")]
        public DateTime? LAST_UPDATED_DATE { get; set; }

        public BaseModels()
        {
            var indonesianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            CREATED_DATE = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, indonesianTimeZone);
            LAST_UPDATED_DATE = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, indonesianTimeZone);
        }
    }

}
