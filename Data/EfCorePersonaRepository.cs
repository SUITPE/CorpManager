// Data/EfCorePersonaRepository.cs
using CorpManager_Completo.Interfaces;
using CorpManager_Completo.Models;
using Microsoft.EntityFrameworkCore;

namespace CorpManager_Completo.Data;

/// <summary>
/// Implementación del repositorio usando Entity Framework Core (Sesión 13).
/// Demuestra el uso de DbContext, DbSet y operaciones CRUD sin SQL manual.
/// </summary>
public class EfCorePersonaRepository : IPersonaRepository
{
    private readonly AppDbContext _context;

    public EfCorePersonaRepository(AppDbContext? context = null)
    {
        _context = context ?? new AppDbContext();
    }

    public void Guardar(List<Persona> personas)
    {
        try
        {
            foreach (var persona in personas)
            {
                var entity = ConvertirAEntidad(persona);
                _context.Personas.Add(entity);
            }
            _context.SaveChanges();
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error de EF Core al guardar: {ex.Message}");
            throw;
        }
    }

    public List<Persona> Cargar()
    {
        try
        {
            var entidades = _context.Personas.ToList();
            return entidades.Select(ConvertirAModelo).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error de EF Core al cargar: {ex.Message}");
            return new List<Persona>();
        }
    }

    public int Insertar(Persona persona)
    {
        try
        {
            var entity = ConvertirAEntidad(persona);
            _context.Personas.Add(entity);
            _context.SaveChanges();
            return entity.Id;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error de EF Core al insertar: {ex.Message}");
            throw;
        }
    }

    public Persona? ObtenerPorId(int id)
    {
        try
        {
            var entity = _context.Personas.Find(id);
            return entity != null ? ConvertirAModelo(entity) : null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error de EF Core al obtener por ID: {ex.Message}");
            return null;
        }
    }

    public bool Eliminar(int id)
    {
        try
        {
            var entity = _context.Personas.Find(id);
            if (entity != null)
            {
                _context.Personas.Remove(entity);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error de EF Core al eliminar: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Convierte un modelo de dominio a entidad de EF Core.
    /// </summary>
    private static PersonaEntity ConvertirAEntidad(Persona persona)
    {
        var entity = new PersonaEntity
        {
            Nombre = persona.Nombre,
            Edad = persona.Edad,
            Tipo = persona.ObtenerRol()
        };

        if (persona is Gerente gerente)
        {
            entity.Cargo = "Gerente";
            entity.SalarioMensual = gerente.SalarioMensual;
            entity.Departamento = gerente.Departamento;
            entity.BonoAnual = gerente.BonoAnual;
        }
        else if (persona is Empleado empleado)
        {
            entity.Cargo = empleado.Cargo;
            entity.SalarioMensual = empleado.SalarioMensual;
        }

        return entity;
    }

    /// <summary>
    /// Convierte una entidad de EF Core a modelo de dominio.
    /// </summary>
    private static Persona ConvertirAModelo(PersonaEntity entity)
    {
        if (entity.Tipo == "GERENTE")
        {
            return new Gerente(
                entity.Nombre,
                entity.Departamento ?? "General",
                entity.Edad,
                entity.SalarioMensual,
                entity.BonoAnual ?? 0
            );
        }

        return new Empleado(
            entity.Nombre,
            entity.Cargo ?? "Sin cargo",
            entity.Edad,
            entity.SalarioMensual
        );
    }
}
