using System.Text.Json.Serialization;

namespace CIPLOK_SI_BE.DTO
{
    public class ApprovalListDTO 
    {
        [JsonPropertyName("roomName")]
        public string? RoomName { get; set; }


        [JsonPropertyName("reservationDate")]
        public DateTime ReservationDate { get; set; }

        [JsonPropertyName("startTime")]
        public TimeSpan StartTime { get; set; }
    }
}
