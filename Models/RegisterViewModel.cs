using System.ComponentModel.DataAnnotations;

namespace mini_project_csharp.Models
{
    public class RegisterViewModel
    {
        [Required]
        public required string Nome { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "A palavra-passe deve ter pelo menos 8 caracteres.")]
        public required string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "As palavras-passe não coincidem.")]
        public required string ConfirmPassword { get; set; }
    }
}