using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CIPLOK_SI_BE.Models.Master
{
    public class TBL_MSACTIVITY : BaseModels
    {
        [Key]
        [Required]
        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("activityName")]
        public string ActivityName { get; set; } = string.Empty;

        [JsonPropertyName("activityDesc")]
        public string ActivityDesc { get; set; } = string.Empty;


        [JsonPropertyName("score")]
        public decimal Score { get; set; } = 0;

    }
}
