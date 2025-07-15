using System.Text.Json.Serialization;

namespace CIPLOK_SI_BE.DTO
{
    public class UserDTO
    {
        [JsonPropertyName("userID")]
        public int? UserID { get; set; }
        [JsonPropertyName("email")]
        public string? Email { get; set; }
     
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

    }
}
