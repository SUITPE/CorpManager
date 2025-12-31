// Exceptions/SalarioInvalidoException.cs
namespace CorpManager_Completo.Exceptions;

public class SalarioInvalidoException : Exception
{
    public SalarioInvalidoException(string mensaje) : base(mensaje) { }
}