using System.Text.Json.Serialization;

namespace CIPLOK_SI_BE.DTO
{
    public class LoginDTO
    {
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string? Password { get; set; }
    }
}
