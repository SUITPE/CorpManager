using CorpManager_Completo.Exceptions;

namespace CorpManager_Completo.Models;

/// <summary>
/// Clase Empleado que hereda de Persona.
/// Demuestra: Herencia (Sesión 8), Encapsulamiento (Sesión 7),
/// y Excepciones personalizadas (Sesión 11).
/// </summary>
public class Empleado : Persona
{
    private const decimal SALARIO_MINIMO = 1000m;
    private const int EDAD_MINIMA = 18;
    private const int EDAD_MAXIMA = 100;

    public Empleado(string nombre, string cargo, int edad, decimal salarioMensual)
        : base(nombre, edad)
    {
        ValidarEdad(edad);
        ValidarSalario(salarioMensual);

        Cargo = cargo;
        SalarioMensual = salarioMensual;
    }

    public string Cargo { get; init; } = string.Empty;
    public decimal SalarioMensual { get; private set; }

    public override string ObtenerRol() => "Empleado";

    public override decimal CalcularSalarioAnual() => SalarioMensual * 12m;

    /// <summary>
    /// Aplica un aumento porcentual al salario.
    /// Usa try/catch para manejo de errores (Sesión 11).
    /// </summary>
    public void AplicarAumento(decimal porcentaje)
    {
        try
        {
            if (porcentaje <= 0)
                throw new ArgumentException("El porcentaje debe ser mayor a cero.");

            if (porcentaje > 100)
                throw new ArgumentException("El porcentaje no puede ser mayor a 100%.");

            SalarioMensual += SalarioMensual * (porcentaje / 100m);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error al aplicar aumento: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Valida que el salario cumpla con el mínimo establecido.
    /// Lanza SalarioInvalidoException si no cumple (Sesión 11).
    /// </summary>
    private static void ValidarSalario(decimal salario)
    {
        if (salario < SALARIO_MINIMO)
        {
            throw new SalarioInvalidoException(
                $"El salario ${salario:N2} es menor al mínimo permitido (${SALARIO_MINIMO:N2}).",
                salario,
                SALARIO_MINIMO
            );
        }
    }

    /// <summary>
    /// Valida que la edad esté en el rango permitido.
    /// Lanza EdadInvalidaException si no cumple (Sesión 11).
    /// </summary>
    private static void ValidarEdad(int edad)
    {
        if (edad < EDAD_MINIMA || edad > EDAD_MAXIMA)
        {
            throw new EdadInvalidaException(
                $"La edad {edad} no está en el rango permitido ({EDAD_MINIMA}-{EDAD_MAXIMA}).",
                edad
            );
        }
    }

    public override string ToString() => base.ToString() + $" | {Cargo} | ${SalarioMensual:N2}/mes -> ${CalcularSalarioAnual():N2}/año";
}
