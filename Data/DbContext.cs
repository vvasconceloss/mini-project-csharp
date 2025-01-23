using mini_project_csharp.Models;
using Microsoft.EntityFrameworkCore;

namespace mini_project_csharp.Data
{
  public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
  {
    public required DbSet<Client> Clientes { get; set; }
    public required DbSet<CodPostal> CodPostals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Client>().ToTable("Clientes");
      modelBuilder.Entity<CodPostal>().ToTable("CodPostal");
    }
  }
}