using CIPLOK_SI_BE.Models.Master;
using System.Text.Json.Serialization;

namespace CIPLOK_SI_BE.DTO
{
    public class CriteriaDTO
    {
        [JsonPropertyName("criteriaName")]
        public string? CriteriaName { get; set; }

        [JsonPropertyName("bobot")]
        public int Bobot { get; set; }

        [JsonPropertyName("parameter")]
        public bool Parameter { get; set; }

        public List<SubCriteriaDTO>? SubCriteria { get; set; }

        public class SubCriteriaDTO
        {
            [JsonPropertyName("idSubCriteria")]
            public int IDSubCriteria { get; set; }

            [JsonPropertyName("idCriteria")]
            public int IDCriteria { get; set; }

            [JsonPropertyName("subCriteriaName")]
            public string? SubCriteriaName { get; set; }

            [JsonPropertyName("subCriteriaBobot")]
            public int SubCriteriaBobot { get; set; }
        }
    }
}
