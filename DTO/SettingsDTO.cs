using System.Text.Json.Serialization;

namespace CIPLOK_SI_BE.DTO
{
    public class SettingsDTO
    {
        [JsonPropertyName("codeSettings")]
        public string? CodeSettings {  get; set; }

        [JsonPropertyName("descriptionSettings")]
        public string? DescriptionSettings { get; set; }
    }
}
