using System.ComponentModel.DataAnnotations;

namespace mini_project_csharp.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Um email precisa ser definido.")]
        [EmailAddress]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Uma password precisa ser definida.")]
        public required string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}