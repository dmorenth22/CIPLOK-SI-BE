using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CIPLOK_SI_BE.DTO
{
    public class MajelisDTO
    {

        [JsonPropertyName("fullName")]
        public string? FullName { get; set; }

        [JsonPropertyName("userID")]
        public int? userID { get; set; }


        [JsonPropertyName("phoneNo")]
        public string? PhoneNo { get; set; }

      
        [JsonPropertyName("codePnt")]
        public string? CodePnt { get; set; }

    
        [JsonPropertyName("jabatanPenatua")]
        public string? JabatanPenatua { get; set; }

        [JsonPropertyName("startDate")]

        public DateTime StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; set; }

    }
}
