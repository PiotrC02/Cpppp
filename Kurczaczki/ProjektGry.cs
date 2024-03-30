using System; //zawiera wszystkie podstawowe typy i funkcje udostępniane przez platformę .NET
using System.Collections.Generic; //pozwala kodowi używać klas w przestrzeni nazw bez konieczności pełnego kwalifikowania nazw klas
using System.Drawing; //przestrzeń nazw zawierająca klasy dla podstawowych funkcji graficznych, takich jak tworzenie i manipulowanie obrazami, rysowanie kształtów i tekstu oraz praca z kolorami
using System.Windows.Forms; //przestrzeń nazw zawierająca klasy do tworzenia aplikacji opartych na systemie Windows, które w pełni wykorzystują bogate funkcje interfejsu użytkownika dostępne w systemie operacyjnym Microsoft Windows
using System.Media; //przestrzeń nazw, która umożliwia odtwarzanie dźwięków i plików audio w programie C#

namespace Kurczaczki //przestrzeń nazw
{
    public partial class ProjektGry : Form //częściowo publiczna klasa
    {
        //włączamy soundtracka
        SoundPlayer _soundPlayer = new SoundPlayer(soundLocation: @"C:\Users\MASTER\Desktop\Studia\Semestr 3\Notatki i zadania\Programowanie aplikacyjne\Projekt\Kurczaczki\Soundtrack.wav");
        SoundPlayer _soundPlayer2 = new SoundPlayer(soundLocation: @"C:\Users\MASTER\Desktop\Studia\Semestr 3\Notatki i zadania\Programowanie aplikacyjne\Projekt\Kurczaczki\Koniec.wav");
        SoundPlayer _soundPlayer3 = new SoundPlayer(soundLocation: @"C:\Users\MASTER\Desktop\Studia\Semestr 3\Notatki i zadania\Programowanie aplikacyjne\Projekt\Kurczaczki\Ups.wav");

        //przypisanie zmiennych różnych atrybutów
        int kurczakSpeed = 10, leftMostKurczak = 0, count = 0, dt = 1, zycia = 3, wynik = 0;
        int bossLives = 5;

        //stworzenie elementów i listy elementów
        Elementy _bigEgg;
        Elementy _rakieta;

        List<Elementy> _pociski = new List<Elementy>();

        //kurczak
        Bitmap _mainKurczakImage = Properties.Resources.chickenGreen;
        List<Bitmap> _kurczakObiekt = new List<Bitmap>();
        Elementy[,] _kurczak = new Elementy[4, 8];
        int[] topKurczak = new int[4];

        //serce
        List<Elementy> _zyjeSerce = new List<Elementy>();

        //jajka
        Bitmap _mainZlamaneJajko = Properties.Resources.eggBreak;
        List<Bitmap> _zlamaneJajkoObiekt = new List<Bitmap>();
        List<Elementy> _jajko = new List<Elementy>();

        //randomowo wybiera liczbe
        Random rand = new Random();

        public ProjektGry() //publiczna klasa, która odpala wszystko po kolei, każdy timer, każdy obiekt, itp.
        {
            InitializeComponent();
            Inicjalizacja();
            _soundPlayer.Play();
            _soundPlayer.PlayLooping(); //muzyka się zapętla
            this.FormClosing += ProjektGry_FormClosing; //powoduje, że gra się wyłącza i nie działa w tle

            _collisionTimer = new System.Windows.Forms.Timer(); //włączanie timerów na kolizje
            _collisionTimer.Interval = COLLISION_DELAY;
            _collisionTimer.Tick += OnCollisionTimerTick;
            bossLivesLabel.Visible = false; //życia bossa nie są jeszcze włączane na starcie
        }

        private void Inicjalizacja() //prywatna klasa, która tworzy rakiete, serca, kurczaki, itp.
        {
            _rakieta = new Elementy(50, 50); //utworzenie elementu rakieta o wymiarach 50x50
            _rakieta.Left = Width / 2 - _rakieta.Width / 2; //regulacja szerokości
            _rakieta.Top = Height - _rakieta.Height - 50; //regulacja wysokości
            _rakieta.Image = Properties.Resources.ship; //dodanie obrazka rakiety
            Controls.Add(_rakieta); //dodanie kontrolek do rakiety

            divideImageIntoFrames(_mainKurczakImage, _kurczakObiekt, 10); //tu chodzi o odpowiednią ilość klatek w gifie

            createKurczak(); //tworzy kurczaka

            createSerce(); //tworzy serce

            divideImageIntoFrames(_mainZlamaneJajko, _zlamaneJajkoObiekt, 8); //obrazek ma kilka klatek
        }

        private void ProjektGry_FormClosing(object sender, FormClosingEventArgs e) //zatrzymuje muzyke jak się wyłączy okno
        {
            _soundPlayer.Stop();
            _soundPlayer2.Stop();
            _soundPlayer3.Stop();
        }

        private void createBoss() //tworzy bossa, więc zatrzymuje timery dotyczące kurczaków i spawnowanie się jajek
        {
            jajkoTimer.Stop();
            kurczakTimer.Stop();
            for (int i = _jajko.Count - 1; i >= 0; i--) //usuwa jajka po tym jak się pojawi boss
            {
                _jajko[i].Dispose();
                _jajko.RemoveAt(i);
            }
            for (int i = _pociski.Count - 1; i >= 0; i--) //usuwa pociski po tym jak się pojawi boss
            {
                _pociski[i].Dispose();
                _pociski.RemoveAt(i);
            }
            _bigEgg = new Elementy(100, 100); //tworzy bossa
            _bigEgg.Left = Width / 2 - _bigEgg.Width / 2; //regulacja szerokości
            _bigEgg.Top = Height / 2 - _bigEgg.Height / 2 - 200; //regulacja wysokości
            _bigEgg.Image = Properties.Resources.bigEgg; //obrazek bossa
            Controls.Add(_bigEgg); //dodaje mu kontrolki
            checkCollision(); //sprawdzanie kolizji
            startBossTimer(); //timer bossa włączony
        }

        private void checkForBossSpawn() //czeka, aż po wybiciu kurczaków boss się zespawni
        {
            if (wynik == 320)
            {
                createBoss();
            }
        }

        private void updateScore() //sprawdza, aby wynik był dobrze zaktualizowany
        {
            wynik += 10;
            checkForBossSpawn();
        }

        private System.Windows.Forms.Timer _collisionTimer; //timer do kolizji
        private const int COLLISION_DELAY = 1000; // 1 sekunda

        private void checkCollision() //sprawdzanie kolizji
        {
            // Rectangle jest strukturą danych zawierającą informacje o kształcie geometrycznym z czterema bokami i czterema kątami prostymi
            Rectangle rakietaRect = new Rectangle(_rakieta.Left, _rakieta.Top, _rakieta.Width, _rakieta.Height);
            Rectangle bigEggRect = new Rectangle(_bigEgg.Left, _bigEgg.Top, _bigEgg.Width, _bigEgg.Height);

            for (int i = 0; i < _pociski.Count; i++) //jeśli pocisk uderzy w bossa to mu życie spada
            {
                if (_pociski[i].Bounds.IntersectsWith(_bigEgg.Bounds))
                {
                    bossLives--;
                    _pociski.RemoveAt(i);
                    i--;
                    wynik += 100; //za bossa więcej punktów otrzymujemy
                    lblScore.Text = "Punkty: " + wynik.ToString(); //licznik punktów
                }
            }

            if (_bigEgg.Bounds.IntersectsWith(_rakieta.Bounds)) //kolizja rakiety z bossem
            {
                if (!_collisionTimer.Enabled) //spada życie statku
                {
                    _collisionTimer.Start();
                    zycia--;
                    _zyjeSerce[zycia].Image = Properties.Resources.d_heart; //tracimy serce
                }
                if (zycia < 1) //jeśli stracimy życia to koniec gry
                {
                    _bigEggTimer.Stop();
                    _bigEgg.Dispose();
                    Controls.Remove(_bigEgg);
                    koniecGry(Properties.Resources.lose);
                    _soundPlayer3.Play();
                }
            }
        }

        private void OnCollisionTimerTick(object sender, EventArgs e) //timer podczas kolizji
        {
            _collisionTimer.Stop();
        }

        System.Windows.Forms.Timer _bigEggTimer = new System.Windows.Forms.Timer(); //timer bossa

        private void startBossTimer() //timer bossa
        {
            _bigEggTimer.Interval = 20;
            _bigEggTimer.Tick += new EventHandler(MoveBoss); //movement bossa
            _bigEggTimer.Start();
        }

        private bool _moveRight = true;
        private bool _moveDown = true;

        private void MoveBoss(object sender, EventArgs e) //jak sie porusza boss
        {
            int bigEggWidth = 150; //szerokość
            int bigEggHeight = 150; //wysokość
            _bigEgg.Width = bigEggWidth;
            _bigEgg.Height = bigEggHeight;

            //ogólnie to co się stanie jak boss pójdzie w prawo i napotka barierę, to wtedy idzie w przeciwnym kierunku, itp.
            if (_moveRight)
            {
                _bigEgg.Left += 10;
                if (_bigEgg.Right >= this.Width - 10)
                    _moveRight = false;
            }
            else
            {
                _bigEgg.Left -= 10;
                if (_bigEgg.Left <= 0)
                    _moveRight = true;
            }
            if (_moveDown)
            {
                _bigEgg.Top += 1;
                if (_bigEgg.Bottom >= this.Height - 1)
                    _moveDown = false;
            }
            else
            {
                _bigEgg.Top -= 1;
                if (_bigEgg.Top <= 0)
                    _moveDown = true;
            }

            checkCollision(); //sprawdzanie kolizji

            bossLivesLabel.Visible = true; //widoczne życia bossa
            bossLivesLabel.Text = "Boss Życia: " + bossLives.ToString(); //tekst z ilością życia bossa

            if (bossLives <= 0)
            {
                _bigEggTimer.Stop();
                _bigEgg.Dispose();
            }
            if (bossLives == 0) //boss przegrywa
            {
                _bigEggTimer.Stop();
                Controls.Remove(_bigEgg);
                koniecGry(Properties.Resources.win);
                _soundPlayer2.Play();
            }
        }

        private void createSerce() //tworzy 3 serca z postacią bitmapy
        {
            Bitmap serce = Properties.Resources.heart;
            for (int i = 0; i < 3; i++)
            {
                _zyjeSerce.Add(new Elementy(50, 50));
                _zyjeSerce[i].Image = serce;
                _zyjeSerce[i].Left = Width - (3 - i) * 70;
                Controls.Add(_zyjeSerce[i]);
            }
        }

        private void createKurczak() //tworzy kurczaki w 4 rzędach i 8 kolumnach z postacią bitmapy
        {
            Bitmap img = _kurczakObiekt[0];
            for (int i = 0; i < 4; i++)
            {
                topKurczak[i] = i * 100 + img.Height;
                for (int j = 0; j < 8; j++)
                {
                    Elementy kurczak = new Elementy(50, 50);
                    kurczak.Image = img;
                    kurczak.Left = j * 100;
                    kurczak.Top = i * 100 + img.Height;
                    _kurczak[i, j] = kurczak;
                    Controls.Add(kurczak);
                }
            }
        }

        private void divideImageIntoFrames(Bitmap original, List<Bitmap> res, int n) //obrazki mają kilka klatek
        {
            int w = original.Width / n, h = original.Height;
            for (int i = 0; i < n; i++)
            {
                int s = i * w;
                Bitmap elementy = new Bitmap(w, h);
                for (int j = 0; j < h; j++)
                    for (int k = 0; k < w; k++)
                        elementy.SetPixel(k, j, original.GetPixel(k + s, j));
                res.Add(elementy);
            }
        }

        private void jajkoTimer_Tick(object sender, EventArgs e) //timer na jajka, które spadają losowo
        {
            if (rand.Next(30) == 10)
                wystrzelLosowoJajko();
            // Pozwala jajkom spadać w dół
            for (int i = 0; i < _jajko.Count; i++)
            {
                _jajko[i].Top += _jajko[i].jajkoSpadaSpeed + 2;
                // _jajko[i] zderzenie z rakietą
                if (_rakieta.Bounds.IntersectsWith(_jajko[i].Bounds))
                {
                    Controls.Remove(_jajko[i]);
                    _jajko.RemoveAt(i);
                    spadaSerce();
                    break;
                }
                if (_jajko[i].Top >= Height - (_jajko[i].Height + 80))
                {
                    _jajko[i].jajkoSpadaSpeed = 0;
                    if (_jajko[i].jajkoSpadaCount / 2 < _zlamaneJajkoObiekt.Count)
                    {
                        _jajko[i].Image = _zlamaneJajkoObiekt[_jajko[i].jajkoSpadaCount / 2];
                        _jajko[i].jajkoSpadaCount += 1;
                    }
                    else
                    {
                        Controls.Remove(_jajko[i]);
                        _jajko.RemoveAt(i);
                    }
                }
            }
        }

        private void spadaSerce() //tracimy serca
        {
            zycia -= 1;
            _zyjeSerce[zycia].Image = Properties.Resources.d_heart;
            if (zycia < 1)
                koniecGry(Properties.Resources.lose);
        }

        private void koniecGry(Bitmap img) //koniec gry, czyli wyczyszczenie planszy, itp.
        {
            jajkoTimer.Stop();
            pociskiTimer.Stop();
            kurczakTimer.Stop();
            Controls.Clear();
            Elementy obraz = new Elementy(700, 400);
            obraz.Click += wymazanie;
            obraz.Image = img;
            obraz.Left = Width / 2 - obraz.Width / 2;
            obraz.Top = Height / 2 - obraz.Height / 2;
            Controls.Add(obraz);
            _soundPlayer.Stop();
            if (zycia < 1)
                _soundPlayer3.Play();
        }
        private void wymazanie(object sender, EventArgs e) //czyści plansze
        {
            Close();
        }
        private void wystrzelLosowoJajko() //strzela losowo jajkami
        {
            List<Elementy> dostepnyKurczak = new List<Elementy>();
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 8; j++)
                    if (_kurczak[i, j] != null)
                        dostepnyKurczak.Add(_kurczak[i, j]);
            // Wybiera losowo pozycje, gdzie znajduje się kurczak, aby zrzucił jajko z niej
            Elementy kurczak = dostepnyKurczak[rand.Next() % dostepnyKurczak.Count];
            Elementy jajko = new Elementy(30, 30);
            jajko.Image = Properties.Resources.egg;
            jajko.Left = kurczak.Left + kurczak.Width / 2 - jajko.Width / 2;
            jajko.Top = kurczak.Top + kurczak.Height;
            _jajko.Add(jajko);
            Controls.Add(jajko);
        }

        private void ProjektGry_KeyDown(object sender, KeyEventArgs e) //tutaj zaprogramowane klawisze i ruch rakiety
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    _rakieta.Left -= 10;
                    if (_rakieta.Left < 0)
                    {
                        _rakieta.Left = 0;
                    }
                    break;
                case Keys.Right:
                    _rakieta.Left += 10;
                    if (_rakieta.Left > 1140)
                    {
                        _rakieta.Left = 1140;
                    }
                    break;
                case Keys.Up:
                    _rakieta.Top -= 10;
                    if (_rakieta.Top < 450)
                    {
                        _rakieta.Top = 450;
                    }
                    break;
                case Keys.Down:
                    _rakieta.Top += 10;
                    if (_rakieta.Top > 600)
                    {
                        _rakieta.Top = 600;
                    }
                    break;
            }
        }

        System.Windows.Forms.Timer shootingTimer = new System.Windows.Forms.Timer(); //timer na strzelanie

        private void shootingTimer_Tick(object sender, EventArgs e)
        {
            shootingTimer.Stop();
        }

        private void ProjektGry_KeyUp(object sender, KeyEventArgs e) //spacja jako strzały
        {
            if (e.KeyCode == Keys.Space)
            {
                if (!shootingTimer.Enabled)
                {
                    wystrzelPociski();
                    shootingTimer.Interval = 3; //1 sekund opóźnienia przy strzale
                    shootingTimer.Tick += shootingTimer_Tick;
                    shootingTimer.Start();
                }
            }
        }

        private void wystrzelPociski() //strzela pociskami, co ileś punktów zmienia strzały na podwójne, potrójne, itd.
        {
            Elementy pociski = new Elementy(10, 10);
            pociski.Left = _rakieta.Left + _rakieta.Width / 2 - pociski.Width / 2;
            pociski.Top = _rakieta.Top - pociski.Height;
            if (wynik >= 0)
                pociski.Image = Properties.Resources.b1;
            _pociski.Add(pociski);
            Controls.Add(pociski);
            if (wynik > 100)
                pociski.Image = Properties.Resources.b2;
            if (wynik > 200)
                pociski.Image = Properties.Resources.b3;
            if (wynik > 320)
                pociski.Image = Properties.Resources.a3;
        }

        private void kurczakTimer_Tick(object sender, EventArgs e) //timer kurczaków, ich prędkość, itp.
        {
            if (leftMostKurczak + 770 > Width || leftMostKurczak < 0)
                kurczakSpeed = -kurczakSpeed;
            leftMostKurczak += kurczakSpeed;
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (_kurczak[i, j] == null)
                        continue;
                    _kurczak[i, j].Image = _kurczakObiekt[count];
                    _kurczak[i, j].Left += kurczakSpeed;
                }
            count = count + dt;
            dt = count == 9 ? -1 : (count == 0 ? 1 : dt); //dotyczy animacji skrzydeł kurczaków
        }

        private void pociskiTimer_Tick(object sender, EventArgs e) //timer na pociski
        {
            for (int i = 0; i < _pociski.Count; i++)
                _pociski[i].Top -= 30;
            kolizja();
            if (wynik == 320)
                updateScore();
        }

        private void kolizja() //tutaj jest zdefiniowana kolizja z obiektami, np. pocisk jak trafi w kurczaka, itp.
        {
            for (int i = 0; i < topKurczak.Length; i++)
            {
                // Pierwsze wystąpienie wyszukiwania binarnego w pociskach
                int lo = 0, hi = _pociski.Count - 1, md, ans = -1;
                while (lo <= hi)
                {
                    md = lo + (hi - lo) / 2;
                    if (_pociski[md].Top >= topKurczak[i])
                    {
                        hi = md - 1;
                        ans = md;
                    }
                    else
                        lo = md + 1;
                }
                if (ans != -1 && _pociski[ans].Top >= topKurczak[i] && _pociski[ans].Top <= topKurczak[i] + _kurczakObiekt[0].Height)
                {
                    int j = (_pociski[ans].Left + 9 - leftMostKurczak) / 100;
                    if (j >= 0 && j < 8 && _kurczak[i, j] != null && _pociski[ans].Bounds.IntersectsWith(_kurczak[i, j].Bounds))
                    {
                        Controls.Remove(_pociski[ans]);
                        _pociski.RemoveAt(ans);
                        Controls.Remove(_kurczak[i, j]);
                        _kurczak[i, j] = null;
                        wynik += 10;
                        lblScore.Text = "Punkty: " + wynik.ToString(); //zapisuje punkty w formie tekstu na etykiecie
                    }
                    checkForBossSpawn();
                }
            }
        }
    }
}