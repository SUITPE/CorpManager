// Data/PersonaRepository.cs
using CorpManager_Completo.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;


namespace CorpManager_Completo.Data;

public class SqlPersonaRepository
{
    private readonly string _connectionString = 
        "Server=tuserver;" +
        "Database=DB_TSQL_CURSO;" +
        "User Id=aburgatherealone;" +
        "Password=noseansapos;" +
        "TrustServerCertificate=True;";
    public int Insertar(Persona persona)
    {
        using SqlConnection conn = new(_connectionString);
        conn.Open();

        SqlCommand cmd = new("SP_InsertarPersona", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@Nombre", persona.Nombre);
        cmd.Parameters.AddWithValue("@Edad", persona.Edad);
        cmd.Parameters.AddWithValue("@Tipo", persona.ObtenerRol());

        if (persona is Empleado emp)
        {
            cmd.Parameters.AddWithValue("@Cargo", emp.Cargo);
            cmd.Parameters.AddWithValue("@SalarioMensual", emp.SalarioMensual);
        }
        else if (persona is Gerente ger)
        {
            cmd.Parameters.AddWithValue("@Cargo", "Gerente");
            cmd.Parameters.AddWithValue("@SalarioMensual", ger.SalarioMensual);
            cmd.Parameters.AddWithValue("@Departamento", ger.Departamento);
            cmd.Parameters.AddWithValue("@BonoAnual", ger.BonoAnual);
        }

        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public List<Persona> ObtenerTodas()
    {
        List<Persona> lista = new();

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
                "Empleado" => new Empleado(
                    reader["Nombre"].ToString()!,
                    reader["Cargo"].ToString()!,
                    Convert.ToInt32(reader["Edad"]),
                    Convert.ToDecimal(reader["SalarioMensual"])
                ),
                "GERENTE" => new Gerente(
                    reader["Nombre"].ToString()!,
                    reader["Departamento"].ToString()!,
                    Convert.ToInt32(reader["Edad"]),
                    Convert.ToDecimal(reader["SalarioMensual"]),
                    Convert.ToDecimal(reader["BonoAnual"])
                ),
                _ => new Empleado(reader["Nombre"].ToString()!, "Desconocido", 0, 0m)
            };

            lista.Add(persona);
        }

        return lista;
    }
}