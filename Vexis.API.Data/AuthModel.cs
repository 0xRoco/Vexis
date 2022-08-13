namespace Vexis.API.Data;

public record AuthModel
{
    public string UsernameOrEmail { get; set; }
    public string Password { get; set; }
}