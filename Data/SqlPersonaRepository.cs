// Data/SqlPersonaRepository.cs
using CorpManager_Completo.Interfaces;
using CorpManager_Completo.Models;
using System.Data;
using Microsoft.Data.SqlClient;

namespace CorpManager_Completo.Data;

/// <summary>
/// Implementación del repositorio usando SQL Server con ADO.NET.
/// Demuestra: Interfaces (Sesión 9), Conexión BD (Sesión 12),
/// Procedimientos Almacenados (Sesión 14) y Excepciones (Sesión 11).
/// </summary>
public class SqlPersonaRepository : IPersonaRepository
{
    private readonly string _connectionString;
    private List<Persona> _cache = new();

    public SqlPersonaRepository(string? connectionString = null)
    {
        _connectionString = connectionString ??
            "Server=localhost;" +
            "Database=CorpManagerDB;" +
            "User Id=sa;" +
            "Password=YourPassword123;" +
            "TrustServerCertificate=True;";
    }

    public int Insertar(Persona persona)
    {
        try
        {
            using SqlConnection conn = new(_connectionString);
            conn.Open();

            SqlCommand cmd = new("SP_InsertarPersona", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Nombre", persona.Nombre);
            cmd.Parameters.AddWithValue("@Edad", persona.Edad);
            cmd.Parameters.AddWithValue("@Tipo", persona.ObtenerRol());

            if (persona is Gerente ger)
            {
                cmd.Parameters.AddWithValue("@Cargo", "Gerente");
                cmd.Parameters.AddWithValue("@SalarioMensual", ger.SalarioMensual);
                cmd.Parameters.AddWithValue("@Departamento", ger.Departamento);
                cmd.Parameters.AddWithValue("@BonoAnual", ger.BonoAnual);
            }
            else if (persona is Empleado emp)
            {
                cmd.Parameters.AddWithValue("@Cargo", emp.Cargo);
                cmd.Parameters.AddWithValue("@SalarioMensual", emp.SalarioMensual);
                cmd.Parameters.AddWithValue("@Departamento", DBNull.Value);
                cmd.Parameters.AddWithValue("@BonoAnual", DBNull.Value);
            }

            return Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Error de SQL al insertar: {ex.Message}");
            throw;
        }
    }

    public List<Persona> Cargar()
    {
        try
        {
            _cache.Clear();

            using SqlConnection conn = new(_connectionString);
            conn.Open();

            SqlCommand cmd = new("SP_ListarPersonas", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string tipo = reader["Tipo"].ToString()!;

                Persona persona = tipo switch
                {
                    "GERENTE" => new Gerente(
                        reader["Nombre"].ToString()!,
                        reader["Departamento"].ToString()!,
                        Convert.ToInt32(reader["Edad"]),
                        Convert.ToDecimal(reader["SalarioMensual"]),
                        Convert.ToDecimal(reader["BonoAnual"])
                    ),
                    _ => new Empleado(
                        reader["Nombre"].ToString()!,
                        reader["Cargo"].ToString()!,
                        Convert.ToInt32(reader["Edad"]),
                        Convert.ToDecimal(reader["SalarioMensual"])
                    )
                };

                _cache.Add(persona);
            }

            return _cache;
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Error de SQL al cargar: {ex.Message}");
            return new List<Persona>();
        }
    }

    public void Guardar(List<Persona> personas)
    {
        try
        {
            foreach (var persona in personas)
            {
                Insertar(persona);
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Error de SQL al guardar: {ex.Message}");
            throw;
        }
    }

    public Persona? ObtenerPorId(int id)
    {
        try
        {
            using SqlConnection conn = new(_connectionString);
            conn.Open();

            SqlCommand cmd = new("SP_ObtenerPersonaPorId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);

            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                string tipo = reader["Tipo"].ToString()!;

                return tipo switch
                {
                    "GERENTE" => new Gerente(
                        reader["Nombre"].ToString()!,
                        reader["Departamento"].ToString()!,
                        Convert.ToInt32(reader["Edad"]),
                        Convert.ToDecimal(reader["SalarioMensual"]),
                        Convert.ToDecimal(reader["BonoAnual"])
                    ),
                    _ => new Empleado(
                        reader["Nombre"].ToString()!,
                        reader["Cargo"].ToString()!,
                        Convert.ToInt32(reader["Edad"]),
                        Convert.ToDecimal(reader["SalarioMensual"])
                    )
                };
            }

            return null;
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Error de SQL al obtener por ID: {ex.Message}");
            return null;
        }
    }

    public bool Eliminar(int id)
    {
        try
        {
            using SqlConnection conn = new(_connectionString);
            conn.Open();

            SqlCommand cmd = new("SP_EliminarPersona", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);

            int filasAfectadas = cmd.ExecuteNonQuery();
            return filasAfectadas > 0;
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Error de SQL al eliminar: {ex.Message}");
            return false;
        }
    }
}
