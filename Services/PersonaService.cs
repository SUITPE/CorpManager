// Services/PersonaService.cs
using CorpManager_Completo.Exceptions;
using CorpManager_Completo.Interfaces;
using CorpManager_Completo.Models;
using CorpManager_Completo.Persistence;

namespace CorpManager_Completo.Services;

/// <summary>
/// Servicio de lógica de negocio para gestionar personas.
/// Demuestra: Inyección de dependencias mediante interfaz (Sesión 9),
/// Factory Pattern, y manejo de excepciones (Sesión 11).
/// </summary>
public class PersonaService
{
    private readonly Dictionary<int, Persona> _personas = new();
    private int _nextId = 1;
    private readonly IPersonaRepository _repository;

    /// <summary>
    /// Constructor que permite inyectar cualquier implementación de IPersonaRepository.
    /// Por defecto usa JsonPersonaRepository.
    /// </summary>
    public PersonaService(IPersonaRepository? repository = null)
    {
        _repository = repository ?? new JsonPersonaRepository();
    }

    /// <summary>
    /// Factory method para crear empleados con validación.
    /// Usa try/catch para manejar excepciones de validación (Sesión 11).
    /// </summary>
    public Persona CrearEmpleado(string nombre, string cargo, int edad, decimal salario)
    {
        try
        {
            return new Empleado(nombre, cargo, edad, salario);
        }
        catch (SalarioInvalidoException ex)
        {
            Console.WriteLine($"Error de validación: {ex.Message}");
            throw;
        }
        catch (EdadInvalidaException ex)
        {
            Console.WriteLine($"Error de validación: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Factory method para crear gerentes con validación.
    /// Usa try/catch para manejar excepciones de validación (Sesión 11).
    /// </summary>
    public Persona CrearGerente(string nombre, string depto, int edad, decimal salario, decimal bono)
    {
        try
        {
            return new Gerente(nombre, depto, edad, salario, bono);
        }
        catch (SalarioInvalidoException ex)
        {
            Console.WriteLine($"Error de validación: {ex.Message}");
            throw;
        }
        catch (EdadInvalidaException ex)
        {
            Console.WriteLine($"Error de validación: {ex.Message}");
            throw;
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error de validación: {ex.Message}");
            throw;
        }
    }

    public int Agregar(Persona persona)
    {
        int id = _nextId++;
        _personas[id] = persona;
        return id;
    }

    public IReadOnlyDictionary<int, Persona> ObtenerTodos() => _personas;

    /// <summary>
    /// Carga personas desde el repositorio.
    /// Usa try/catch/finally para manejo robusto de errores (Sesión 11).
    /// </summary>
    public void CargarDesdeArchivo()
    {
        try
        {
            var personasCargadas = _repository.Cargar();

            _personas.Clear();

            if (personasCargadas.Any())
            {
                _nextId = personasCargadas.Count + 1;

                int idActual = 1;
                foreach (var persona in personasCargadas)
                {
                    _personas[idActual] = persona;
                    idActual++;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar datos: {ex.Message}");
        }
        finally
        {
            Console.WriteLine($"Proceso de carga finalizado. Total: {_personas.Count} personas.");
        }
    }

    /// <summary>
    /// Guarda personas en el repositorio.
    /// Usa try/catch/finally para manejo robusto de errores (Sesión 11).
    /// </summary>
    public void GuardarEnArchivo()
    {
        try
        {
            _repository.Guardar(_personas.Values.ToList());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al guardar datos: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Proceso de guardado finalizado.");
        }
    }

    public (int total, decimal promSalario, double promEdad, decimal masaSalarial) CalcularEstadisticas()
    {
        if (!_personas.Any())
            return (0, 0m, 0.0, 0m);

        var empleados = _personas.Values.OfType<Empleado>().ToList();

        if (!empleados.Any())
            return (0, 0m, 0.0, 0m);

        return (
            empleados.Count,
            empleados.Average(e => e.SalarioMensual),
            empleados.Average(e => e.Edad),
            empleados.Sum(e => e.SalarioMensual)
        );
    }
}
