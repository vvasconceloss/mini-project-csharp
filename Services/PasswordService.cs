using System.Security.Cryptography;
using System.Text;

namespace mini_project_csharp.Services {
  public class PasswordService
  {
    string SecretKey = "x8&@l0YJf$9TqW1!NvKp^6*z3dQrZAc";

    public string HashPassword(string password)
    {
      using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(SecretKey));
      var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
      var base64Hash = Convert.ToBase64String(hash);
      return base64Hash.Length > 45 ? base64Hash[..45] : base64Hash;
    }

    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        var hashedProvidedPassword = HashPassword(providedPassword);
        return hashedPassword == hashedProvidedPassword;
    }
  }
}