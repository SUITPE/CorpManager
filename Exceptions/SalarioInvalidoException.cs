// Exceptions/SalarioInvalidoException.cs
namespace CorpManager_Completo.Exceptions;

/// <summary>
/// Excepción personalizada para validar salarios (Sesión 11).
/// Se lanza cuando el salario no cumple con las reglas de negocio.
/// </summary>
public class SalarioInvalidoException : Exception
{
    public decimal SalarioIngresado { get; }
    public decimal SalarioMinimo { get; }

    public SalarioInvalidoException(string mensaje)
        : base(mensaje)
    { }

    public SalarioInvalidoException(string mensaje, decimal salarioIngresado, decimal salarioMinimo)
        : base(mensaje)
    {
        SalarioIngresado = salarioIngresado;
        SalarioMinimo = salarioMinimo;
    }

    public SalarioInvalidoException(string mensaje, Exception innerException)
        : base(mensaje, innerException)
    { }
}

/// <summary>
/// Excepción personalizada para validar edades (Sesión 11).
/// </summary>
public class EdadInvalidaException : Exception
{
    public int EdadIngresada { get; }

    public EdadInvalidaException(string mensaje)
        : base(mensaje)
    { }

    public EdadInvalidaException(string mensaje, int edadIngresada)
        : base(mensaje)
    {
        EdadIngresada = edadIngresada;
    }
}
