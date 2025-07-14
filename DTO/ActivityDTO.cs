using System.Text.Json.Serialization;

namespace CIPLOK_SI_BE.DTO
{
    public class ActivityDTO
    {
        [JsonPropertyName("activityName")]
        public string ActivityName { get; set; } = string.Empty;

        [JsonPropertyName("activityDesc")]
        public string ActivityDesc { get; set; } = string.Empty;


        [JsonPropertyName("score")]
        public decimal Score { get; set; } = 0;
    }
}
