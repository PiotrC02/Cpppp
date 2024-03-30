using System;
using System.Drawing;
using System.Windows.Forms;

namespace Kurczaczki
{
    class Elementy : PictureBox
    {
        public int jajkoSpadaCount = 0, jajkoSpadaSpeed = 2;
        public Elementy(int szerokosc, int wysokosc)
            {
            Width = szerokosc;
            Height = wysokosc;
            SizeMode = PictureBoxSizeMode.StretchImage;
            BackColor = Color.Transparent;
            }
    }
}
