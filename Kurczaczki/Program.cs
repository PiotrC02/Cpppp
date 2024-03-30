using System;
using System.Windows.Forms;
using static Kurczaczki.Elementy;

namespace Kurczaczki
{
    internal static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MenuGry());
        }
    }
}