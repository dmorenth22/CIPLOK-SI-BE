using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CIPLOK_SI_BE.Models.Master
{
    public class TBL_MSROLES 
    {
        [Key]
        [Required]

        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("roleName")]
        public string? RoleName { get; set; }
    }
}
