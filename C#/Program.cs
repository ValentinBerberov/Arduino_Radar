using System.Globalization;
using System.IO.Ports;

namespace ArduinoInterfacing;

static class Program
{
    // [System.Runtime.InteropServices.DllImport("kernel32.dll")]
    // private static extern bool AllocConsole();

    
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.

        // AllocConsole();

        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());

    }
}