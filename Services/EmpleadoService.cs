using CorpManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CorpManager.Services;

public class EmpleadoService
{
    // Ahora usamos Dictionary para acceso rápido por ID
    private readonly Dictionary<int, Empleado> _empleados = new();
    private int _nextId = 1;

    public Empleado Agregar(Empleado empleado)
    {
        empleado = new Empleado
        {
            Nombre = empleado.Nombre.Trim(),
            Cargo = empleado.Cargo.Trim(),
            Edad = empleado.Edad,
            Salario = empleado.Salario
        };

        var nuevo = new EmpleadoConId(_nextId++, empleado);
        _empleados[nuevo.Id] = nuevo;

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n¡Empleado {nuevo.Nombre} registrado con éxito! (ID: {nuevo.Id})");
        Console.ResetColor();

        return nuevo;
    }

    public IReadOnlyDictionary<int, Empleado> ObtenerTodos() => _empleados.ToDictionary(k => k.Key, v => (Empleado)v.Value);

    public Empleado? ObtenerPorId(int id) => _empleados.GetValueOrDefault(id) as Empleado;

    public bool ExisteId(int id) => _empleados.ContainsKey(id);

    public (int total, decimal salarioPromedio, double edadPromedio, decimal masaSalarial) ObtenerEstadisticas()
    {
        if (_empleados.Count == 0)
            return (0, 0, 0, 0);

        var valores = _empleados.Values.Cast<EmpleadoConId>().ToList();
        decimal sumaSalarios = valores.Sum(e => e.Salario);
        int sumaEdades = valores.Sum(e => e.Edad);

        return (
            _empleados.Count,
            sumaSalarios / _empleados.Count,
            (double)sumaEdades / _empleados.Count,
            sumaSalarios
        );
    }
}

// Clase auxiliar para llevar el ID
internal class EmpleadoConId : Empleado
{
    public int Id { get; }

    public EmpleadoConId(int id, Empleado source)
    {
        Id = id;
        Nombre = source.Nombre;
        Cargo = source.Cargo;
        Edad = source.Edad;
        Salario = source.Salario;
    }

    public override string ToString() => $"[{Id}] {Nombre} | {Cargo} | {Edad} años | ${Salario:N2}";
}