// Models/Empleado.cs
namespace CorpManager.Models;

public class Empleado
{
    // Campos privados (encapsulamiento)
    private string _nombre = string.Empty;
    private string _cargo = string.Empty;
    private int _edad;
    private decimal _salario;

    // Constructor principal
    public Empleado(string nombre, string cargo, int edad = 18, decimal salario = 0m)
    {
        Nombre = nombre;     // usa la propiedad para validar
        Cargo = cargo;
        Edad = edad;
        Salario = salario;
    }

    // Propiedades con validación (Sesión 6)
    public string Nombre
    {
        get => _nombre;
        set => _nombre = string.IsNullOrWhiteSpace(value) ? "Sin nombre" : value.Trim();
    }

    public string Cargo
    {
        get => _cargo;
        set => _cargo = string.IsNullOrWhiteSpace(value) ? "Sin cargo" : value.Trim();
    }

    public int Edad
    {
        get => _edad;
        set => _edad = value < 16 ? 18 : value > 100 ? 100 : value;
    }

    public decimal Salario
    {
        get => _salario;
        set => _salario = value < 0 ? 0m : value;
    }

    // Método de instancia (Sesión 6)
    public string ObtenerInfoCompleta()
    {
        return $"[ID] {Nombre.PadRight(20)} | {Cargo.PadRight(15)} | {Edad,3} años | ${Salario,12:N2}";
    }

    public void AplicarAumento(decimal porcentaje)
    {
        if (porcentaje <= 0) return;
        Salario += Salario * (porcentaje / 100m);
    }

    public override string ToString() => ObtenerInfoCompleta();
}