using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CIPLOK_SI_BE.DTO
{
    public class MajelisDTO
    {

        [Required]
        [JsonPropertyName("fullName")]
        public string? FullName { get; set; }

        [Required]
        [JsonPropertyName("codePnt")]
        public string? CodePnt { get; set; }

        [Required]
        [JsonPropertyName("jabatanPenatua")]
        public string? JabatanPenatua { get; set; }

        [JsonPropertyName("startDate")]

        public DateTime StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; set; }

    }
}
