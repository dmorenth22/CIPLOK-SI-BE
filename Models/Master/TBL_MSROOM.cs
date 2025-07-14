using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CIPLOK_SI_BE.Models.Master
{
    public class TBL_MSROOM : BaseModels
    {
        [Key]
        [Required]
        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("namaRuangan")]
        public string NamaRuangan { get; set; } = string.Empty;

        [JsonPropertyName("lokasiRuangan")]
        public string LokasiRuangan { get; set; } = string.Empty;

        [JsonPropertyName("capacity")]
        public int Capacity { get; set; } = 0;

        [JsonPropertyName("score")]
        public int Score { get; set; } = 0;

        [JsonPropertyName("startTime")]
        public DateTime? StartTime { get; set; }

        [JsonPropertyName("endTime")]

        public DateTime? EndTime { get; set; }
    }
}
