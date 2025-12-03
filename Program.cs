using CorpManager.Services;
using CorpManager.UI;

namespace CorpManager;

class Program
{
    static void Main(string[] args)
    {
        var servicioEmpleados = new EmpleadoService();
        MenuPrincipal.Mostrar(servicioEmpleados);
    }
}