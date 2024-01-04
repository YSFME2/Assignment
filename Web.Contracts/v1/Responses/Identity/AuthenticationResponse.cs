using System.Text.Json.Serialization;

namespace Web.Contracts.v1.Responses.Identity
{
    public record AuthenticationResponse(string Token, DateTime Expiration,  DateTime RefreshTokenExpiration)
    {
        [JsonIgnore]
        public string RefreshToken { get; init; }
    }
}
