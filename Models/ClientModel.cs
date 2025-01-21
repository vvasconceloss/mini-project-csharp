using System.ComponentModel.DataAnnotations;

namespace mini_project_csharp.Models
{
    public class Client 
    {
        [Key]
        public int IdClient { get; set; }
        public required string Nome { get; set; }
        public string? Apelido { get; set; }
        public string? Endereco { get; set; }
        public string? Telefone { get; set; }
        public required string Email { get; set; }
        public string? Nif { get; set; }
        public required string Password { get; set; }

        public int IdCodPostal { get; set; }
    }
}