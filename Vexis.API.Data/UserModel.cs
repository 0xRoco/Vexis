using System.Text.Json.Serialization;

namespace Vexis.API.Data;

public class UserModel
{
    [JsonPropertyName("username")] public string Username { get; set; }
    [JsonPropertyName("email")] public string Email { get; set; }
    [JsonPropertyName("phoneNumber")] public string PhoneNumber { get; set; }

    [JsonPropertyName("profilePictureUrl")]
    public string ProfilePictureUrl { get; set; }

    [JsonPropertyName("isEmailVerified")] public bool IsEmailVerified { get; set; }

    [JsonPropertyName("isPhoneNumberVerified")]
    public bool IsPhoneNumberVerified { get; set; }
}