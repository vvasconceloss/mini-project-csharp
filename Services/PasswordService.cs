using System.Security.Cryptography;
using System.Text;

namespace mini_project_csharp.Services {
  // Esta classe serve para proteger e verificar palavras-passe usando hashing
  public class PasswordService
  {
    // Uma chave secreta usada para "assinar" as palavras-passe com mais segurança
    // Esta chave deve ser protegida e não deve estar diretamente no código
    string SecretKey = "x8&@l0YJf$9TqW1!NvKp^6*z3dQrZAc";

    // Este método cria um hash de uma palavra-passe fornecida
    public string HashPassword(string password)
    {
      // Cria um objeto para gerar o hash, usando o algoritmo HMAC-SHA256 e a chave secreta
      using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(SecretKey));
      
      // Gera o hash da palavra-passe (em formato de bytes)
      var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
      
      // Converte o hash para uma string em Base64 (um formato que podemos guardar como texto)
      var base64Hash = Convert.ToBase64String(hash);
      
      // Se o hash for muito longo, corta-o para no máximo 45 caracteres
      return base64Hash.Length > 45 ? base64Hash[..45] : base64Hash;
    }

    // Este método verifica se uma palavra-passe fornecida corresponde ao hash armazenado
    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        // Cria um hash para a palavra-passe fornecida
        var hashedProvidedPassword = HashPassword(providedPassword);
        
        // Compara o hash gerado com o hash guardado e devolve verdadeiro se forem iguais
        return hashedPassword == hashedProvidedPassword;
    }
  }
}
