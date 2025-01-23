    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace mini_project_csharp.Models
    {
        [Table("Clientes")]
        public class Client 
        {
            [Key]
            public int IdClientes { get; set; }

            [Required(ErrorMessage = "*Um nome precisa ser introduzido.")]
            public required string Nome { get; set; }
            public string? Apelido { get; set; }
            public string? Endereco { get; set; }
            public string? Telefone { get; set; }

            [Required(ErrorMessage = "*Um email precisa ser introduzido.")]
            [EmailAddress(ErrorMessage = "*O este endereço de email não é válido.")]
            public required string Email { get; set; }
            public string? Nif { get; set; }

            [Required(ErrorMessage = "*Uma password precisa ser definida.")]
            [MinLength(8, ErrorMessage = "*A password deve ter pelo menos 8 caracteres.")]
            [RegularExpression(
                @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).+$", 
                ErrorMessage = "*A password deve conter pelo menos uma letra minúscula, uma letra maiúscula, um número e um caractere especial.")]
            public required string Password { get; set; }

            [ForeignKey("IdCodPostal")]
            public int? IdCodPostal { get; set; }
        }
    }