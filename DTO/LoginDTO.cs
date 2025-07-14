using System.Text.Json.Serialization;

namespace CIPLOK_SI_BE.DTO
{
    public class LoginDTO
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string? Password { get; set; }
    }
}
