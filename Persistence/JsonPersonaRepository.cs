// Persistence/JsonPersonaRepository.cs
using CorpManager_Completo.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace CorpManager_Completo.Persistence;

public class JsonPersonaRepository
{
    private const string RutaArchivo = "Data/personas.json";

    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public JsonPersonaRepository()
    {
        // Crea la carpeta Data si no existe
        Directory.CreateDirectory(Path.GetDirectoryName(RutaArchivo)!);
    }

    public void Guardar(List<Persona> personas)
    {
        var json = JsonSerializer.Serialize(personas, _options);
        File.WriteAllText(RutaArchivo, json);
    }

    public List<Persona> Cargar()
    {
        if (!File.Exists(RutaArchivo))
            return new List<Persona>();

        var json = File.ReadAllText(RutaArchivo);
        return JsonSerializer.Deserialize<List<Persona>>(json, _options) ?? new List<Persona>();
    }
}