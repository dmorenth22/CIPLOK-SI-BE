using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CIPLOK_SI_BE.Models.Master
{
    public class TBL_MSSUBCRITERIA : BaseModels
    {
        [Key]
        [JsonPropertyName("idSubCriteria")]
        [Required]
        public int IDSubCriteria { get; set; }


        [ForeignKey("TBL_MSCRITERIA")]
        [JsonPropertyName("idCriteria")]
        public int IDCriteria { get; set; }

        [JsonPropertyName("subCriteriaName")]
        public string? SubCriteriaName { get; set; }

        [JsonPropertyName("subCriteriaBobot")]
        public int SubCriteriaBobot { get; set; }

    }
}
