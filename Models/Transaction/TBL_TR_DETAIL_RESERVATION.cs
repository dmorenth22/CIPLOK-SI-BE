using CIPLOK_SI_BE.Models.Master;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CIPLOK_SI_BE.Models.Transaction
{
    public class TBL_TR_DETAIL_RESERVATION :BaseModels
    {
        [Key]
        [JsonPropertyName("IDTRDetail")]
        public int IDTRDetail { get; set; }


        [ForeignKey("TBL_TR_HEADER_RESERVATION")]
        [JsonPropertyName("TransactionID")]
        public int TransactionID { get; set; } 

        [JsonPropertyName("CriteriaCode")]
        public string? CriteriaCode { get; set; }

        [JsonPropertyName("CriteriaName")]
        public string? CriteriaName { get; set; }

        [JsonPropertyName("Bobot")]
        public int? Bobot { get; set; }

        [JsonPropertyName("Parameter")]
        public bool? Parameter { get; set; }

        [JsonPropertyName("SubCriteriaName")]
        public string? SubCriteriaName { get; set; }

        [JsonPropertyName("SubCriteriaBobot")]
        public int? SubCriteriaBobot { get; set; }

        [ForeignKey("TBL_MSCRITERIA")]
        [JsonPropertyName("CriteriaID")]
        public int? CriteriaID { get; set; }


        [ForeignKey("TBL_MSSUBCRITERIA")]
        [JsonPropertyName("SubCriteriaID")]
        public int? SubCriteriaID { get; set; }

 
    }
}
