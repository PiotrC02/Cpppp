using System;
using System.Windows.Forms;

namespace Kurczaczki
{
    public partial class MenuGry : Form
    {
        public MenuGry()
        {
            InitializeComponent();
        }

        private void LoadGame(object sender, EventArgs e)
        {
            ProjektGry projektGry = new ProjektGry();

            projektGry.Show();
        }
    }
}
