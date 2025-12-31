using CorpManager_Completo.Models;

namespace CorpManager_Completo.Models;

public class Empleado : Persona
{
    public Empleado(string nombre, string cargo, int edad, decimal salarioMensual)
        : base(nombre, edad)
    {
        Cargo = cargo;
        SalarioMensual = salarioMensual;
    }

    public string Cargo { get; init; } = string.Empty;
    public decimal SalarioMensual { get; private set; }

    public override string ObtenerRol() => "Empleado";

    public override decimal CalcularSalarioAnual() => SalarioMensual * 12m;

    public void AplicarAumento(decimal porcentaje)
    {
        if (porcentaje > 0)
            SalarioMensual += SalarioMensual * (porcentaje / 100m);
    }

    public override string ToString() => base.ToString() + $" | {Cargo} | ${SalarioMensual:N2}/mes → ${CalcularSalarioAnual():N2}/año";
}