namespace CorpManager_Completo.Models;

/// <summary>
/// Clase Gerente que hereda de Empleado.
/// Demuestra: Herencia multinivel (Sesión 8) y polimorfismo.
/// </summary>
public class Gerente : Empleado
{
    private const decimal SALARIO_MINIMO_GERENTE = 5000m;
    private const decimal BONO_MINIMO = 1000m;

    public Gerente(string nombre, string departamento, int edad, decimal salarioMensual, decimal bonoAnual)
        : base(nombre, "Gerente", edad, salarioMensual)
    {
        if (salarioMensual < SALARIO_MINIMO_GERENTE)
            throw new Exceptions.SalarioInvalidoException(
                $"El salario de gerente debe ser al menos ${SALARIO_MINIMO_GERENTE:N2}.",
                salarioMensual,
                SALARIO_MINIMO_GERENTE
            );

        if (bonoAnual < BONO_MINIMO)
            throw new ArgumentException($"El bono anual debe ser al menos ${BONO_MINIMO:N2}.");

        Departamento = departamento;
        BonoAnual = bonoAnual;
    }

    public string Departamento { get; init; } = string.Empty;
    public decimal BonoAnual { get; init; }

    public override string ObtenerRol() => "GERENTE";

    public override decimal CalcularSalarioAnual() => base.CalcularSalarioAnual() + BonoAnual;

    public override string ToString() => base.ToString() + $" | Depto: {Departamento} | +Bono ${BonoAnual:N2}";
}
