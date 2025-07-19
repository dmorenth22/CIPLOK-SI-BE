using System.Text.Json.Serialization;

public class FinalizeDTO
{
    [JsonPropertyName("transactionID")]
    public int TransactionID { get; set; }

    [JsonPropertyName("reservationDate")]
    public string? ReservationDate { get; set; }

    [JsonPropertyName("startTime")]
    public TimeSpan StartTime { get; set; }

    [JsonPropertyName("roomName")]
    public string RoomName { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }
}
