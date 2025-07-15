using System.Text.Json.Serialization;

namespace CIPLOK_SI_BE.DTO
{
    public class MajelistDataDTO
    {
        [JsonPropertyName("majelisID")]
        public int? MajelisID { get; set; }

        [JsonPropertyName("userID")]
        public int? UserID { get; set; }

        [JsonPropertyName("codePnt")]
        public string? CodePnt { get; set; }

        [JsonPropertyName("fullName")]
        public string? FullName { get; set; }

        [JsonPropertyName("jabatanPenatua")]
        public string JabatanPenatua { get; set; }= string.Empty;

        [JsonPropertyName("phoneNo")]
        public string? PhoneNo { get; set; }

        [JsonPropertyName("alamatPenatua")]
        public string? AlamatPenatua { get; set; }

        [JsonPropertyName("startDate")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public DateTime? EndDate { get; set; }
    }
}
