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
        //��d�N�����`�F�b�N����
        if (1 < Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length)
        {
            //���łɋN�����Ă���Ɣ��f���ďI��
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