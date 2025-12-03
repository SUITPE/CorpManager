namespace CorpManager.Models;

public class Empleado
{
    public required string Nombre { get; set; }
    public required string Cargo { get; set; }
    public int Edad { get; set; } = 18;
    public decimal Salario { get; set; } = 0m;

    public override string ToString()
    {
        return $"{Nombre} | {Cargo} | {Edad} años | ${Salario:N2}";
    }
}