using CorpManager_Completo.Models;
using CorpManager_Completo.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace CorpManager_Completo.Services;

public class PersonaService
{
    private readonly Dictionary<int, Persona> _personas = new();
    private int _nextId = 1;

    private readonly JsonPersonaRepository _jsonRepository = new();

    public Persona CrearEmpleado(string nombre, string cargo, int edad, decimal salario)
    {
        return new Empleado(nombre, cargo, edad, salario);
    }

    public Persona CrearGerente(string nombre, string depto, int edad, decimal salario, decimal bono)
    {
        return new Gerente(nombre, depto, edad, salario, bono);
    }

    public int Agregar(Persona persona)
    {
        int id = _nextId++;
        _personas[id] = persona;
        return id;
    }

    public IReadOnlyDictionary<int, Persona> ObtenerTodos() => _personas;

    // Métodos de persistencia JSON
    public void CargarDesdeArchivo()
    {
        var personasCargadas = _jsonRepository.Cargar();

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

    public void GuardarEnArchivo()
    {
        _jsonRepository.Guardar(_personas.Values.ToList());
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
} // ← Esta llave cierra la clase – ¡AHORA ESTÁ PERFECTA!