namespace CorpManager.Models;

public class Empleado
{
    public string Nombre { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public int Edad { get; set; }
    public decimal Salario { get; set; }

    public override string ToString()
    {
        return $"{Nombre} | {Cargo} | {Edad} años | ${Salario:N2}";
    }
}