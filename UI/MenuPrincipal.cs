using CorpManager_Completo.Models;
using CorpManager_Completo.Services;
using System;

namespace CorpManager_Completo.UI;

public static class MenuPrincipal
{
    public static void Mostrar(PersonaService servicio)
    {
        servicio.CargarDesdeArchivo();
        Console.WriteLine($"Cargadas {servicio.ObtenerTodos().Count} personas desde JSON.\n");

        bool salir = false;
        while (!salir)
        {
            Console.Clear();
            MostrarCabecera();

            Console.WriteLine("1. Registrar empleado");
            Console.WriteLine("2. Registrar gerente");
            Console.WriteLine("3. Listar personas");
            Console.WriteLine("4. Ver estadísticas");
            Console.WriteLine("5. Salir");
            Console.Write("\nSelecciona una opción: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1":
                    RegistrarEmpleado(servicio);
                    Pausa();
                    break;
                case "2":
                    RegistrarGerente(servicio);
                    Pausa();
                    break;
                case "3":
                    ListarPersonas(servicio);
                    Pausa();
                    break;
                case "4":
                    MostrarEstadisticas(servicio);
                    Pausa();
                    break;
                case "5":
                    servicio.GuardarEnArchivo();
                    Console.WriteLine("\nDatos guardados en personas.json");
                    salir = true;
                    Console.WriteLine("\n¡Gracias por usar CorpManager! 👋");
                    break;
                default:
                    Console.WriteLine("Opción inválida.");
                    Pausa();
                    break;
            }
        }
    }

    private static void MostrarCabecera()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=========================================");
        Console.WriteLine("     CORPMANAGER v1.0 - PROYECTO COMPLETO");
        Console.WriteLine("     Sistema de Gestión Corporativa");
        Console.WriteLine("=========================================\n");
        Console.ResetColor();
    }

    private static void RegistrarEmpleado(PersonaService servicio)
    {
        Console.Clear();
        Console.WriteLine("REGISTRO DE EMPLEADO\n");

        Console.Write("Nombre: ");
        string nombre = Console.ReadLine()?.Trim() ?? "Sin nombre";

        Console.Write("Cargo: ");
        string cargo = Console.ReadLine()?.Trim() ?? "Sin cargo";

        int edad = PedirEntero("Edad", 18, 100);
        decimal salario = PedirDecimal("Salario mensual", 1000m);

        var empleado = servicio.CrearEmpleado(nombre, cargo, edad, salario);
        servicio.Agregar(empleado);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n¡{nombre} registrado como {cargo}!");
        Console.ResetColor();
    }

    private static void RegistrarGerente(PersonaService servicio)
    {
        Console.Clear();
        Console.WriteLine("REGISTRO DE GERENTE\n");

        Console.Write("Nombre: ");
        string nombre = Console.ReadLine()?.Trim() ?? "Sin nombre";

        Console.Write("Departamento: ");
        string depto = Console.ReadLine()?.Trim() ?? "General";

        int edad = PedirEntero("Edad", 25, 100);
        decimal salario = PedirDecimal("Salario mensual", 5000m);
        decimal bono = PedirDecimal("Bono anual", 10000m);

        var gerente = servicio.CrearGerente(nombre, depto, edad, salario, bono);
        servicio.Agregar(gerente);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n¡{nombre} registrado como Gerente de {depto} con bono ${bono:N2}!");
        Console.ResetColor();
    }

    private static void ListarPersonas(PersonaService servicio)
    {
        Console.Clear();
        Console.WriteLine("LISTA DE PERSONAS\n");

        var lista = servicio.ObtenerTodos();
        if (!lista.Any())
        {
            Console.WriteLine("No hay personas registradas.");
            return;
        }

        foreach (var kvp in lista.OrderBy(k => k.Key))
        {
            Console.WriteLine($"ID {kvp.Key}: {kvp.Value}");
            Console.WriteLine($"   Salario anual: ${kvp.Value.CalcularSalarioAnual():N2}\n");
        }
    }

    private static void MostrarEstadisticas(PersonaService servicio)
    {
        Console.Clear();
        Console.WriteLine("ESTADÍSTICAS\n");

        var (total, promSal, promEdad, masa) = servicio.CalcularEstadisticas();

        if (total == 0)
        {
            Console.WriteLine("No hay datos.");
            return;
        }

        Console.WriteLine($"Total personas: {total}");
        Console.WriteLine($"Salario promedio: ${promSal:N2}/mes");
        Console.WriteLine($"Edad promedio: {promEdad:F1} años");
        Console.WriteLine($"Masa salarial: ${masa:N2}/mes");
    }

    private static int PedirEntero(string campo, int min, int max)
    {
        Console.Write($"{campo} ({min}-{max}): ");
        if (int.TryParse(Console.ReadLine(), out int valor) && valor >= min && valor <= max)
            return valor;
        return min;
    }

    private static decimal PedirDecimal(string campo, decimal min)
    {
        Console.Write($"{campo} (>= {min}): ");
        if (decimal.TryParse(Console.ReadLine(), out decimal valor) && valor >= min)
            return valor;
        return min;
    }

    private static void Pausa()
    {
        Console.WriteLine("\nPresiona cualquier tecla...");
        Console.ReadKey();
    }
}