using System.Text.Json.Serialization;

namespace CIPLOK_SI_BE.DTO
{
    public class MajelistDataDTO
    {
        [JsonPropertyName("codePenatua")]
        public string CodePenatua { get; set; }  = string.Empty;

        [JsonPropertyName("namaPenatua")]
        public string NamaPenatua { get; set; }= string.Empty;

        [JsonPropertyName("jabatanPenatua")]
        public string JabatanPenatua { get; set; }= string.Empty;

        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }= string.Empty;

        [JsonPropertyName("startDate")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public DateTime? EndDate { get; set; }
    }
}
