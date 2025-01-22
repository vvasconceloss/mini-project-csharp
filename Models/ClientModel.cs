using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mini_project_csharp.Models
{
    [Table("Clientes")]
    public class Client 
    {
        [Key]
        public int IdClientes { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public required string Nome { get; set; }
        public string? Apelido { get; set; }
        public string? Endereco { get; set; }
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O este endereço de email não é válido.")]
        public required string Email { get; set; }
        public string? Nif { get; set; }

        [Required(ErrorMessage = "A password é obrigatória.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "A password deve ter pelo menos 8 caracteres.")]
        public required string Password { get; set; }

        public int IdCodPostal { get; set; } = 1;
    }
}