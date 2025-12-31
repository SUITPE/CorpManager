namespace CorpManager_Completo.Models;

public abstract class Persona
{
    protected Persona(string nombre, int edad)
    {
        Nombre = nombre;
        Edad = edad;
    }

    public string Nombre { get; protected set; } = string.Empty;
    public int Edad { get; protected set; }

    public virtual string ObtenerRol() => "Persona genérica";
    public virtual decimal CalcularSalarioAnual() => 0m;

    public override string ToString() => $"[{ObtenerRol()}] {Nombre} | {Edad} años";
}