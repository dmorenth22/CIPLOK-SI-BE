using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CIPLOK_SI_BE.Models.Master
{
    public class TBL_MSCRITERIA : BaseModels
    {


        [Key]
        [Required]
        [JsonPropertyName("idCriteria")]
        public int IDCriteria { get; set; }

        [JsonPropertyName("criteriaCode")]
        public string? CriteriaCode { get; set; }

        [JsonPropertyName("CriteriaName")]
        public string? CriteriaName { get; set; }


        [JsonPropertyName("bobot")]
        public int Bobot { get; set; }

        [JsonPropertyName("parameter")]
        public bool Parameter { get; set; }

    }
}
