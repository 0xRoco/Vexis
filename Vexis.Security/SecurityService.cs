using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;

namespace Vexis.Security;

public static class SecurityService
{
    public static Task<string> GetHash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
        var hash = pbkdf2.GetBytes(20);
        var hashBytes = new byte[36];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);
        return Task.FromResult(Convert.ToBase64String(hashBytes));
    }

    public static Task<bool> VerifyHash(string password, string hash)
    {
        var hashBytes = Convert.FromBase64String(hash);

        var salt = new byte[16];
        Array.Copy(hashBytes, 0, salt, 0, 16);

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
        var bytes = pbkdf2.GetBytes(20);

        for (var i = 0; i < 20; i++)
            if (hashBytes[i + 16] != bytes[i])
                return Task.FromResult(false);
        return Task.FromResult(true);
    }

    public static string SecureStringToString(SecureString value)
    {
        try
        {
            var valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr) ?? throw new ArgumentNullException();
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }
        catch (Exception ex)
        {
            //Logger.Error($"An error occured while converting the SecureString to string: {ex.GetBaseException()}");
            return "";
        }
    }

    public static Task<string> GenerateToken()
    {
        var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        return Task.FromResult(token);
    }

    public static Task<string> GenerateCode()
    {
        const int length = 6;
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return Task.FromResult(new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray()));
    }

    public static Task<int> GenerateId()
    {
        return Task.FromResult(RandomNumberGenerator.GetInt32(10000, 99999));
    }
}