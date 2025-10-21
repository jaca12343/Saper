using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Saper
{
    /// <summary>
    /// Logika interakcji dla klasy game.xaml
    /// </summary>
    public partial class game : Page
    {
        public int dificulty;
        public bool firstMove;
        public int height;
        public int width;
        internal Board myBoard;
        public game()
        {
            InitializeComponent();
        }
        public DispatcherTimer timer;
        public DateTime start;
        public game(int width, int height, int dif) {
            InitializeComponent();

            myBoard = new Board(width, height, gameGrid, dificulty, this);



            dificulty = dif;
            firstMove = true;
            this.height = height;
            this.width = width;


            
            
            //pokazywanie ilości flag
            int divident, numberOfBombs;
            switch (dificulty)
            {
                case 1:
                    divident = 5;
                    break;
                case 2:
                    divident = 4;
                    break;
                case 3:
                    divident = 3;
                    break;
                default:
                    divident = 5;
                    break;
            }
            numberOfBombs = (width * height) / divident;
            TxtBoxFlags.Text = "Flagi: " + numberOfBombs;
            myBoard.SetFlagNumber(numberOfBombs);


            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += TimerTick;
            
        }
        private void TimerTick(object sender, EventArgs e)
        {
            if ((DateTime.Now - start).TotalMinutes < 1)
            {
                TxtBoxTimer.Text = "Czas: " + (Math.Round((DateTime.Now - start).TotalSeconds)).ToString();
            }
            else
            {
                TxtBoxTimer.Text = "Czas: " + (Math.Round((DateTime.Now - start).TotalMinutes)).ToString() + ":" + (Math.Round((DateTime.Now - start).TotalSeconds % 60)).ToString();
            }
        }

        
        
        
        
        private void StartNewGame(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
