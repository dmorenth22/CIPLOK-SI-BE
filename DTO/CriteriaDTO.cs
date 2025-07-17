using CIPLOK_SI_BE.Models.Master;
using System.Text.Json.Serialization;

namespace CIPLOK_SI_BE.DTO
{
    public class CriteriaDTO
    {
        [JsonPropertyName("idHeaderCriteria")]
        public int? IDHeaderCriteria { get; set; }


        [JsonPropertyName("criteriaName")]
        public string? CriteriaName { get; set; }

        [JsonPropertyName("bobot")]
        public int Bobot { get; set; }

        [JsonPropertyName("parameter")]
        public string? Parameter { get; set; }


        [JsonPropertyName("criteriaCode")]
        public string? CriteriaCode { get; set; }

        [JsonPropertyName("subCriteriaList")]
        public List<SubCriteriaDTO>? SubCriteria { get; set; }

        public class SubCriteriaDTO
        {
            [JsonPropertyName("idTrDetail")]
            public int? IDTrDetail { get; set; }
            [JsonPropertyName("idSubCriteria")]
            public int? IDSubCriteria { get; set; }

            [JsonPropertyName("idCriteria")]
            public int? IDCriteria { get; set; }

            [JsonPropertyName("subCriteriaName")]
            public string? SubCriteriaName { get; set; }

            [JsonPropertyName("subCriteriaBobot")]
            public int SubCriteriaBobot { get; set; }
        }
    }
}
