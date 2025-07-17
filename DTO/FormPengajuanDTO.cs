using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CIPLOK_SI_BE.DTO
{
    public class FormPengajuanDTO
    {

        [JsonPropertyName("transactionID")]
        public int TransactionID { get; set; }

        [JsonPropertyName("status")]
        public string? STATUS { get; set; }

        [JsonPropertyName("startTime")]
        public TimeSpan StartTime { get; set; }

        [JsonPropertyName("roomName")]
        public string? RoomName { get; set; }

        [JsonPropertyName("reservationDate")]
        public DateTime? ReservationDate { get; set; }

        [JsonPropertyName("reservationDateString")]
        public string? ReservationDateString { get; set; }

        [JsonPropertyName("createdDate")]
        public string CreatedDate { get; set; }

        [JsonPropertyName("createdBy")]
        public string? CreatedBy { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("mjRequest")]
        public string? MJRequest { get; set; }

        [JsonPropertyName("subCriteriaList")]
        public List<DetailDTO>? Details { get; set; }

        public class DetailDTO
        {
            [JsonPropertyName("idTrDetail")]
            public int? IDTrDetail { get; set; }


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
