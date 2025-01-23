using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mini_project_csharp.Models {
  public class CodPostal {
    
    [Key]
    public int IdCodPostal { get; set; }
    public string? Localidade { get; set; }
    public string? Codpostal { get; set; }

    [NotMapped]
    public string CodPostalFormatado
    {
      get
      {
        if (Codpostal?.Length == 7)
        {
          return Codpostal.Insert(4, "-") + " " + Localidade;
        }
        return Codpostal + " " + Localidade;
      }
    }
  }
}