using System.ComponentModel.DataAnnotations;

namespace mini_project_csharp.Models
{
    public class Client 
    {
        [Key]
        public int IdClient { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public required string Nome { get; set; }
        public string? Apelido { get; set; }
        public string? Endereco { get; set; }
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O este endereço de email não é válido.")]
        public required string Email { get; set; }
        public string? Nif { get; set; }

        [Required(ErrorMessage = "A palavra-passe é obrigatória.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A palavra-passe deve ter pelo menos 8 caracteres.")]
        public required string Password { get; set; }

        public int IdCodPostal { get; set; }
    }
}