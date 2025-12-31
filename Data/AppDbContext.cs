// Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;

namespace CorpManager_Completo.Data;

/// <summary>
/// Contexto de Entity Framework Core (Sesión 13).
/// Actúa como puente entre las clases del proyecto y las tablas de la base de datos.
/// </summary>
public class AppDbContext : DbContext
{
    public DbSet<PersonaEntity> Personas { get; set; } = null!;

    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                "Server=localhost;Database=CorpManagerDB;User Id=sa;Password=YourPassword123;TrustServerCertificate=True;"
            );
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PersonaEntity>(entity =>
        {
            entity.ToTable("Personas");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Cargo)
                .HasMaxLength(50);

            entity.Property(e => e.Departamento)
                .HasMaxLength(50);

            entity.Property(e => e.SalarioMensual)
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.BonoAnual)
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.Tipo)
                .IsRequired()
                .HasMaxLength(20);
        });

        base.OnModelCreating(modelBuilder);
    }
}

/// <summary>
/// Entidad para mapear a la tabla Personas en la base de datos.
/// Entity Framework Core requiere propiedades con get/set públicos.
/// </summary>
public class PersonaEntity
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Edad { get; set; }
    public string Tipo { get; set; } = "Empleado"; // "Empleado" o "GERENTE"
    public string? Cargo { get; set; }
    public decimal SalarioMensual { get; set; }
    public string? Departamento { get; set; }
    public decimal? BonoAnual { get; set; }
}
