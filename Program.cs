using Com.Nakasendo.Gakupetit.Properties;

namespace Com.Nakasendo.Gakupetit;

internal static class Program
{
    // Hold the mutex for the lifetime of the application so the instance remains unique
    private static Mutex? s_singleInstanceMutex;

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        const string mutexName = "Gakupetit_Singleton_Mutex_v1";
        bool createdNew;
        try
        {
            // Try to create a named mutex. If it already exists, another instance is running.
            s_singleInstanceMutex = new Mutex(true, mutexName, out createdNew);
        }
        catch
        {
            // If mutex creation fails for any reason, fall back to allowing startup to avoid blocking the user.
            createdNew = true;
        }

        if (!createdNew)
        {
            // Ç∑Ç≈Ç…ãNìÆÇµÇƒÇ¢ÇÈÇ∆îªífÇµÇƒèIóπ
            MessageBox.Show(Resources.MainSingletonMessage, Resources.Gakupetit,
                MessageBoxButtons.OK, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1, 0);
            return;
        }

        try
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
        finally
        {
            // Release and dispose the mutex when the application exits
            if (s_singleInstanceMutex != null)
            {
                try { s_singleInstanceMutex.ReleaseMutex(); } catch { }
                s_singleInstanceMutex.Dispose();
                s_singleInstanceMutex = null;
            }
        }
    }
}
