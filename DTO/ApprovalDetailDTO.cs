using System.Dynamic;
using System.Text.Json.Serialization;

namespace CIPLOK_SI_BE.DTO
{
    public class ApprovalDetailDTO
    {

        [JsonPropertyName("reservationData")]
        public List<ExpandoObject>? ReservationData { get; set; }

        [JsonPropertyName("scoreData")]
        public List<ExpandoObject> ScoreData { get; set; } = new();

        //public class ReservationHeader
        //{
        //    [JsonPropertyName("transactionID")]
        //    public int TransactionID { get; set; }

        //    [JsonPropertyName("reservationDate")]
        //    public DateTime ReservationDate { get; set; }


        //    [JsonPropertyName("startTime")]
        //    public TimeSpan StartTime { get; set; }

        //    [JsonPropertyName("description")]
        //    public string? Description { get; set; }

        //    [JsonPropertyName("mjMengetahui")]
        //    public string? MJMengetahui { get; set; }


        //    [JsonPropertyName("peminjam")]
        //    public string? Peminjam { get; set; }

        //    [JsonPropertyName("jenisKegiatan")]
        //    public string? JenisKegiatan { get; set; }

        //    [JsonPropertyName("durasi")]
        //    public string? Durasi { get; set; }

        //    [JsonPropertyName("rutin")]
        //    public string? Rutin { get; set; }

        //}




    }
}
