using Com.Nakasendo.Gakupetit.Properties;
using System.Diagnostics;

namespace Com.Nakasendo.Gakupetit;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        //二重起動をチェックする
        if (1 < Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length)
        {
            //すでに起動していると判断して終了
            MessageBox.Show(Resources.MainSingletonMessage, Resources.Gakupetit,
                MessageBoxButtons.OK, MessageBoxIcon.Warning,
                 MessageBoxDefaultButton.Button1, 0);
            return;
        }

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}