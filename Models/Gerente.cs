using CorpManager_Completo.Models;

namespace CorpManager_Completo.Models;

public class Gerente : Empleado
{
    public Gerente(string nombre, string departamento, int edad, decimal salarioMensual, decimal bonoAnual)
        : base(nombre, "Gerente", edad, salarioMensual)
    {
        Departamento = departamento;
        BonoAnual = bonoAnual;
    }

    public string Departamento { get; init; } = string.Empty;
    public decimal BonoAnual { get; init; }

    public override string ObtenerRol() => "GERENTE";

    public override decimal CalcularSalarioAnual() => base.CalcularSalarioAnual() + BonoAnual;

    public override string ToString() => base.ToString() + $" | Depto: {Departamento} | +Bono ${BonoAnual:N2}";
}