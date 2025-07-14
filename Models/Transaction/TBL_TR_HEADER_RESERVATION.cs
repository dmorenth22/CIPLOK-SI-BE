using CIPLOK_SI_BE.Models.Master;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CIPLOK_SI_BE.Models.Transaction
{
    public class TBL_TR_HEADER_RESERVATION : BaseModels
    {
        [Key]
        [JsonPropertyName("TransactionID")]
        public int TransactionID { get; set; }

        [JsonPropertyName("RoomName")]
        public string? RoomName { get; set; }

        [JsonPropertyName("ReservationDate")]
        public DateTime ReservationDate { get; set; }

        [JsonPropertyName("StartTime")]
        public TimeSpan StartTime { get; set; }

        [JsonPropertyName("STATUS")]
        public string? STATUS { get; set; }

        [JsonPropertyName("Description")]
        public string? Description { get; set; }

        [JsonPropertyName("MJRequest")]
        public string? MJRequest { get; set; }
    }
}
