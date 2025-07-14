using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CIPLOK_SI_BE.Models.Master
{
    public class TBL_MSMAJELIS : BaseModels
    {
        [Key]
        [Required]
        [JsonPropertyName("id")]
        public int ID { get; set; }


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
