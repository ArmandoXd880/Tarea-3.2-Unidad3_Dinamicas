using System;
using System.Windows.Forms;

namespace Unidad3.Dinamicas.Interfaz;

internal static class Program
{
    [STAThread] // Requerido por cuadros de diálogo y componentes de Windows.
    private static void Main()
    {
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new FormPrincipal());
    }
}
