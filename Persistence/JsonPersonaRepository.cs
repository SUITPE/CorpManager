// Persistence/JsonPersonaRepository.cs
using CorpManager_Completo.Interfaces;
using CorpManager_Completo.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CorpManager_Completo.Persistence;

/// <summary>
/// Implementación del repositorio usando archivos JSON.
/// Demuestra el uso de interfaces (Sesión 9) y manejo de archivos (Sesión 10).
/// </summary>
public class JsonPersonaRepository : IPersonaRepository
{
    private const string RutaArchivo = "Data/personas.json";
    private List<Persona> _personasEnMemoria = new();

    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new PersonaJsonConverter() }
    };

    public JsonPersonaRepository()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(RutaArchivo)!);
    }

    public void Guardar(List<Persona> personas)
    {
        _personasEnMemoria = personas;
        var json = JsonSerializer.Serialize(personas, _options);
        File.WriteAllText(RutaArchivo, json);
    }

    public List<Persona> Cargar()
    {
        if (!File.Exists(RutaArchivo))
            return new List<Persona>();

        var json = File.ReadAllText(RutaArchivo);
        _personasEnMemoria = JsonSerializer.Deserialize<List<Persona>>(json, _options) ?? new List<Persona>();
        return _personasEnMemoria;
    }

    public int Insertar(Persona persona)
    {
        _personasEnMemoria.Add(persona);
        Guardar(_personasEnMemoria);
        return _personasEnMemoria.Count;
    }

    public Persona? ObtenerPorId(int id)
    {
        if (id > 0 && id <= _personasEnMemoria.Count)
            return _personasEnMemoria[id - 1];
        return null;
    }

    public bool Eliminar(int id)
    {
        if (id > 0 && id <= _personasEnMemoria.Count)
        {
            _personasEnMemoria.RemoveAt(id - 1);
            Guardar(_personasEnMemoria);
            return true;
        }
        return false;
    }
}

/// <summary>
/// Converter personalizado para serializar/deserializar la jerarquía de Persona.
/// Necesario porque System.Text.Json no maneja polimorfismo automáticamente.
/// </summary>
public class PersonaJsonConverter : JsonConverter<Persona>
{
    public override Persona? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using JsonDocument doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        string tipo = root.GetProperty("tipo").GetString() ?? "Empleado";
        string nombre = root.GetProperty("nombre").GetString() ?? "";
        int edad = root.GetProperty("edad").GetInt32();

        if (tipo == "GERENTE")
        {
            string depto = root.GetProperty("departamento").GetString() ?? "";
            decimal salario = root.GetProperty("salarioMensual").GetDecimal();
            decimal bono = root.GetProperty("bonoAnual").GetDecimal();
            return new Gerente(nombre, depto, edad, salario, bono);
        }
        else
        {
            string cargo = root.GetProperty("cargo").GetString() ?? "";
            decimal salario = root.GetProperty("salarioMensual").GetDecimal();
            return new Empleado(nombre, cargo, edad, salario);
        }
    }

    public override void Write(Utf8JsonWriter writer, Persona value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("tipo", value.ObtenerRol());
        writer.WriteString("nombre", value.Nombre);
        writer.WriteNumber("edad", value.Edad);

        if (value is Gerente gerente)
        {
            writer.WriteString("cargo", "Gerente");
            writer.WriteNumber("salarioMensual", gerente.SalarioMensual);
            writer.WriteString("departamento", gerente.Departamento);
            writer.WriteNumber("bonoAnual", gerente.BonoAnual);
        }
        else if (value is Empleado empleado)
        {
            writer.WriteString("cargo", empleado.Cargo);
            writer.WriteNumber("salarioMensual", empleado.SalarioMensual);
        }

        writer.WriteEndObject();
    }
}
