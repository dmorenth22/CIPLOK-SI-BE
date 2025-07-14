using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CIPLOK_SI_BE.Models.Master
{
    public class TBL_MSUSERS : BaseModels
    {
        [Key]
        [Required]
        [JsonPropertyName("id")]
        public int ID { get; set; }

        [ForeignKey("TBL_MSROLES")]
        [JsonPropertyName("roleID")]
        public int RoleID { get; set; }

        public virtual TBL_MSROLES? Role { get; set; }

        [JsonPropertyName("userName")]
        public string? UserName { get; set; }

        [JsonPropertyName("anggotaKomisi")]
        public string? AnggotaKomisi { get; set; }

        [JsonPropertyName("password")]
        public string? Password { get; set; }

        [JsonPropertyName("fullName")]
        public string? FullName { get; set; }

        [JsonPropertyName("phoneNo")]
        public string? PhoneNo { get; set; }

        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("alternatePhoneNo")]
        public string? AlternatePhoneNo { get; set; }

        [ForeignKey("Majelis")]
        [JsonPropertyName("majelesID")]
        public int? MajelisID { get; set; }

        public virtual TBL_MSMAJELIS? Majelis { get; set; }
    }


}
