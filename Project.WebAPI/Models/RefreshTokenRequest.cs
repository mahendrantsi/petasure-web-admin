namespace Project.WebAPI.Models
{
    using System.Text.Json.Serialization;

    public class RefreshTokenRequest
    {
        [JsonPropertyName("AccessToken")]
        public string AccessToken { get; set; }
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
