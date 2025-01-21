using System.ComponentModel.DataAnnotations;

namespace mini_project_csharp.Models {
  public class CodPostal {
    [Key]
    public int IdCodPostal { get; set; }
    public string? Localidade { get; set; }
    public string? CodPostalValue { get; set; }
  }
}