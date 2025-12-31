using CorpManager_Completo.Services;
using CorpManager_Completo.UI;

namespace CorpManager_Completo;

class Program
{
    static void Main(string[] args)
    {
        var servicioPersonas = new PersonaService();
        MenuPrincipal.Mostrar(servicioPersonas);
    }
}