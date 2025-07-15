using System.Text.Json.Serialization;

namespace CIPLOK_SI_BE.DTO
{
    public class LoginResponseDTO
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;

        [JsonPropertyName("fullName")]
        public string FullName { get; set; } = string.Empty;
        
        [JsonPropertyName("roleName")]
        public string RoleName { get; set; } = string.Empty;


        [JsonPropertyName("jabatanPenatua")]
        public string JabatanPenatua { get; set; } = string.Empty;

        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;


        [JsonPropertyName("phoneNo")]
        public string PhoneNo { get; set; } = string.Empty;

        [JsonPropertyName("anggotaKomisi")]
        public string AnggotaKomisi { get; set; } = string.Empty;
    }
}
