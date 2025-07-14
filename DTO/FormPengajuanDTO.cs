using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CIPLOK_SI_BE.DTO
{
    public class FormPengajuanDTO
    {

        [JsonPropertyName("transactionID")]
        public int TransactionID { get; set; }

        [JsonPropertyName("STATUS")]
        public string? STATUS { get; set; }

        [JsonPropertyName("StartTime")]
        public TimeSpan StartTime { get; set; }

        [JsonPropertyName("RoomName")]
        public string? RoomName { get; set; }

        [JsonPropertyName("ReservationDate")]
        public DateTime ReservationDate { get; set; }

        [JsonPropertyName("Description")]
        public string? Description { get; set; }

        [JsonPropertyName("MJRequest")]
        public string? MJRequest { get; set; }

        public List<DetailDTO>? Details { get; set; }

        public class DetailDTO
        {
            [JsonPropertyName("criteriaCode")]
            public string? CriteriaCode { get; set; }

            [JsonPropertyName("criteriaName")]
            public string? CriteriaName { get; set; }

            [JsonPropertyName("subCriteriaName")]
            public string? SubCriteriaName { get; set; }

            [JsonPropertyName("criteriaID")]
            public int CriteriaID { get; set; }

            [JsonPropertyName("subCriteriaID")]
            public int SubCriteriaID { get; set; }

            [JsonPropertyName("bobot")]
            public int? Bobot { get; set; }

            [JsonPropertyName("subCriteriaBobot")]
            public int? SubCriteriaBobot { get; set; }

            [JsonPropertyName("parameter")]
            public bool? Parameter { get; set; }
        }
    }
}
