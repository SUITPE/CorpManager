using CorpManager.Models;
using System;
using System.Collections.Generic;

namespace CorpManager.Services;

public class EmpleadoService
{
    private readonly List<Empleado> _empleados = new();

    public void Agregar(Empleado empleado)
    {
        _empleados.Add(empleado);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n¡{empleado.Nombre} registrado con éxito!");
        Console.ResetColor();
    }

    public IReadOnlyList<Empleado> ObtenerTodos() => _empleados.AsReadOnly();

    public (int total, decimal salarioPromedio, double edadPromedio, decimal masaSalarial) ObtenerEstadisticas()
    {
        if (_empleados.Count == 0)
            return (0, 0, 0, 0);

        decimal sumaSalarios = 0;
        int sumaEdades = 0;

        foreach (var e in _empleados)
        {
            sumaSalarios += e.Salario;
            sumaEdades += e.Edad;
        }

        return (
            _empleados.Count,
            sumaSalarios / _empleados.Count,
            (double)sumaEdades / _empleados.Count,
            sumaSalarios
        );
    }
}