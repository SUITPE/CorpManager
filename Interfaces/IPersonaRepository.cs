// Interfaces/IPersonaRepository.cs
using CorpManager_Completo.Models;

namespace CorpManager_Completo.Interfaces;

/// <summary>
/// Interfaz que define el contrato para repositorios de personas.
/// Permite intercambiar entre diferentes implementaciones (JSON, SQL, EF Core).
/// </summary>
public interface IPersonaRepository
{
    void Guardar(List<Persona> personas);
    List<Persona> Cargar();
    int Insertar(Persona persona);
    Persona? ObtenerPorId(int id);
    bool Eliminar(int id);
}
