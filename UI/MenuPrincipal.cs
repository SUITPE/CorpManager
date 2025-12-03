using CorpManager.Models;
using CorpManager.Services;
using System;

namespace CorpManager.UI;

public static class MenuPrincipal
{
    public static void Mostrar(EmpleadoService servicio)
    {
        bool salir = false;

        while (!salir)
        {
            Console.Clear();
            MostrarCabecera();

            Console.WriteLine("1. Registrar empleado");
            Console.WriteLine("2. Listar empleados");
            Console.WriteLine("3. Ver estadísticas");
            Console.WriteLine("4. Salir");
            Console.Write("\nSelecciona una opción: ");

            switch (Console.ReadLine())
            {
                case "1":
                    RegistrarEmpleado(servicio);
                    Pausa();
                    break;
                case "2":
                    ListarEmpleados(servicio);
                    Pausa();
                    break;
                case "3":
                    MostrarEstadisticas(servicio);
                    Pausa();
                    break;
                case "4":
                    salir = true;
                    Console.WriteLine("\n¡Gracias por usar CorpManager! Hasta pronto");
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
        Console.WriteLine("     Sistema de Gestión Corporativa");
        Console.WriteLine("=========================================\n");
        Console.ResetColor();
    }

    private static void RegistrarEmpleado(EmpleadoService servicio)
    {
        Console.Clear();
        Console.WriteLine("REGISTRO DE NUEVO EMPLEADO\n");

        var emp = new Empleado();

        Console.Write("Nombre: ");
        emp.Nombre = Console.ReadLine()?.Trim() ?? "Sin nombre";

        Console.Write("Cargo: ");
        emp.Cargo = Console.ReadLine()?.Trim() ?? "Sin cargo";

        emp.Edad = PedirEntero("Edad", 18, 100);
        emp.Salario = PedirDecimal("Salario mensual", 0);

        servicio.Agregar(emp);
    }

   private static void ListarEmpleados(EmpleadoService servicio)
    {
        Console.Clear();
        Console.WriteLine("LISTA DE EMPLEADOS\n");

        var lista = servicio.ObtenerTodos();
        if (!lista.Any())
        {
            Console.WriteLine("No hay empleados registrados.");
            return;
        }

        foreach (var kvp in lista.OrderBy(k => k.Key))
        {
            Console.WriteLine($"[{kvp.Key}] {kvp.Value}");
        }
    }

    private static void MostrarEstadisticas(EmpleadoService servicio)
    {
        Console.Clear();
        Console.WriteLine("ESTADÍSTICAS DE LA EMPRESA\n");

        var (total, salarioProm, edadProm, masa) = servicio.ObtenerEstadisticas();

        if (total == 0)
        {
            Console.WriteLine("No hay datos para mostrar.");
            return;
        }

        Console.WriteLine($"Total empleados     : {total}");
        Console.WriteLine($"Salario promedio    : ${salarioProm:N2}");
        Console.WriteLine($"Edad promedio       : {edadProm:F1} años");
        Console.WriteLine($"Masa salarial mensual: ${masa:N2}");
    }

    private static int PedirEntero(string campo, int valorPorDefecto, int max = int.MaxValue)
    {
        Console.Write($"{campo}: ");
        if (int.TryParse(Console.ReadLine(), out int valor) && valor > 0 && valor <= max)
            return valor;
        Console.WriteLine($"Valor inválido. Usando {valorPorDefecto}.");
        return valorPorDefecto;
    }

    private static decimal PedirDecimal(string campo, decimal valorPorDefecto)
    {
        Console.Write($"{campo}: ");
        if (decimal.TryParse(Console.ReadLine(), out decimal valor) && valor >= 0)
            return valor;
        Console.WriteLine($"Valor inválido. Usando {valorPorDefecto}.");
        return valorPorDefecto;
    }

    private static void Pausa()
    {
        Console.WriteLine("\nPresiona cualquier tecla para continuar...");
        Console.ReadKey();
    }
}