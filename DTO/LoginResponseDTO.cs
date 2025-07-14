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
    }
}
