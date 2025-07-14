using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CIPLOK_SI_BE.Models.Master
{
    public class TBL_SETTINGS
    {
        [Key]
        [JsonPropertyName("idSettings")]
        public int IDSettings { get; set; }

        [JsonPropertyName("codeSettings")]
        public string CodeSettings { get; set; } = string.Empty;

        [JsonPropertyName("descriptionSettings")]
        public string DescriptionSettings { get; set; } = string.Empty;

    }
}
