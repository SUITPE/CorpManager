// UI/MenuPrincipal.cs
using CorpManager_Completo.Exceptions;
using CorpManager_Completo.Models;
using CorpManager_Completo.Services;

namespace CorpManager_Completo.UI;

/// <summary>
/// Menú principal de consola interactiva.
/// Demuestra manejo de excepciones en la UI (Sesión 11).
/// </summary>
public static class MenuPrincipal
{
    public static void Mostrar(PersonaService servicio)
    {
        servicio.CargarDesdeArchivo();

        bool salir = false;
        while (!salir)
        {
            Console.Clear();
            MostrarCabecera();

            Console.WriteLine("1. Registrar empleado");
            Console.WriteLine("2. Registrar gerente");
            Console.WriteLine("3. Listar personas");
            Console.WriteLine("4. Ver estadisticas");
            Console.WriteLine("5. Salir");
            Console.Write("\nSelecciona una opcion: ");

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
                    salir = true;
                    Console.WriteLine("\nGracias por usar CorpManager!");
                    break;
                default:
                    Console.WriteLine("Opcion invalida.");
                    Pausa();
                    break;
            }
        }
    }

    private static void MostrarCabecera()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=========================================");
        Console.WriteLine("   CORPMANAGER v2.0 - SESIONES 1-14");
        Console.WriteLine("   Sistema de Gestion Corporativa");
        Console.WriteLine("=========================================\n");
        Console.ResetColor();
    }

    /// <summary>
    /// Registra un empleado con manejo de excepciones (Sesión 11).
    /// </summary>
    private static void RegistrarEmpleado(PersonaService servicio)
    {
        Console.Clear();
        Console.WriteLine("REGISTRO DE EMPLEADO\n");

        try
        {
            Console.Write("Nombre: ");
            string nombre = Console.ReadLine()?.Trim() ?? "Sin nombre";

            Console.Write("Cargo: ");
            string cargo = Console.ReadLine()?.Trim() ?? "Sin cargo";

            int edad = PedirEntero("Edad", 18, 100);
            decimal salario = PedirDecimal("Salario mensual", 1000m);

            var empleado = servicio.CrearEmpleado(nombre, cargo, edad, salario);
            servicio.Agregar(empleado);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n{nombre} registrado como {cargo}!");
            Console.ResetColor();
        }
        catch (SalarioInvalidoException ex)
        {
            MostrarError($"Salario invalido: {ex.Message}");
        }
        catch (EdadInvalidaException ex)
        {
            MostrarError($"Edad invalida: {ex.Message}");
        }
        catch (Exception ex)
        {
            MostrarError($"Error inesperado: {ex.Message}");
        }
    }

    /// <summary>
    /// Registra un gerente con manejo de excepciones (Sesión 11).
    /// </summary>
    private static void RegistrarGerente(PersonaService servicio)
    {
        Console.Clear();
        Console.WriteLine("REGISTRO DE GERENTE\n");

        try
        {
            Console.Write("Nombre: ");
            string nombre = Console.ReadLine()?.Trim() ?? "Sin nombre";

            Console.Write("Departamento: ");
            string depto = Console.ReadLine()?.Trim() ?? "General";

            int edad = PedirEntero("Edad", 18, 100);
            decimal salario = PedirDecimal("Salario mensual", 5000m);
            decimal bono = PedirDecimal("Bono anual", 1000m);

            var gerente = servicio.CrearGerente(nombre, depto, edad, salario, bono);
            servicio.Agregar(gerente);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n{nombre} registrado como Gerente de {depto} con bono ${bono:N2}!");
            Console.ResetColor();
        }
        catch (SalarioInvalidoException ex)
        {
            MostrarError($"Salario invalido: {ex.Message}");
        }
        catch (EdadInvalidaException ex)
        {
            MostrarError($"Edad invalida: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            MostrarError($"Dato invalido: {ex.Message}");
        }
        catch (Exception ex)
        {
            MostrarError($"Error inesperado: {ex.Message}");
        }
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
        Console.WriteLine("ESTADISTICAS\n");

        var (total, promSal, promEdad, masa) = servicio.CalcularEstadisticas();

        if (total == 0)
        {
            Console.WriteLine("No hay datos.");
            return;
        }

        Console.WriteLine($"Total personas: {total}");
        Console.WriteLine($"Salario promedio: ${promSal:N2}/mes");
        Console.WriteLine($"Edad promedio: {promEdad:F1} anos");
        Console.WriteLine($"Masa salarial: ${masa:N2}/mes");
    }

    private static int PedirEntero(string campo, int min, int max)
    {
        Console.Write($"{campo} ({min}-{max}): ");
        if (int.TryParse(Console.ReadLine(), out int valor) && valor >= min && valor <= max)
            return valor;
        Console.WriteLine($"Valor invalido. Usando {min}.");
        return min;
    }

    private static decimal PedirDecimal(string campo, decimal min)
    {
        Console.Write($"{campo} (>= {min}): ");
        if (decimal.TryParse(Console.ReadLine(), out decimal valor) && valor >= min)
            return valor;
        Console.WriteLine($"Valor invalido. Usando {min}.");
        return min;
    }

    private static void MostrarError(string mensaje)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\nERROR: {mensaje}");
        Console.ResetColor();
    }

    private static void Pausa()
    {
        Console.WriteLine("\nPresiona cualquier tecla...");
        Console.ReadKey();
    }
}
