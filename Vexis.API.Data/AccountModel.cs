using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable CS8618

namespace Vexis.API.Data;

[Table("accounts")]
public class AccountModel
{
    [Column("id")] public int Id { get; set; }
    [Column("username")] public string Username { get; set; }
    [Column("email")] public string Email { get; set; }
    [Column("password_hash")] public string PasswordHash { get; set; }
    [Column("phone_number")] public string PhoneNumber { get; set; }
    [Column("email_code")] public string EmailCode { get; set; }
    [Column("phone_number_code")] public string PhoneNumberCode { get; set; }
    [Column("login_token")] public string LoginToken { get; set; }
    [Column("password_reset_token")] public string PasswordResetToken { get; set; }
    [Column("profile_picture_url")] public string ProfilePictureUrl { get; set; }
    [Column("is_email_verified")] public bool IsEmailVerified { get; set; }
    [Column("is_phone_number_verified")] public bool IsPhoneNumberVerified { get; set; }
    [Column("is_deleted")] public bool IsDeleted { get; set; }
    [Column("created_at")] public DateTime CreatedAt { get; set; }
    [Column("updated_at")] public DateTime UpdatedAt { get; set; }
    [Column("last_login_at")] public DateTime LastLoginAt { get; set; }
    [Column("last_login_ip")] public string LastLoginIp { get; set; }
    [Column("last_login_hwid")] public string LastLoginHwid { get; set; }
}