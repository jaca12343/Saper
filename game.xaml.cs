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
        public game()
        {
            InitializeComponent();
        }
        public List<List<Button>> buttons = new List<List<Button>>();
        public List<List<int>> values = new List<List<int>>();
        public List<List<int>> state = new List<List<int>>();//0 - covered, 1 - uncovered, 2 - flagged
        DispatcherTimer timer;
        DateTime start;
        int numberOfFlags;
        public game(int width, int height, int dif) {
            InitializeComponent();

            gameGrid.Children.Clear();
            dificulty = dif;
            firstMove = true;
            this.height = height;
            this.width = width;
            if(gameGrid.Height / height > gameGrid.Width / width)
                gameGrid.Height = height * gameGrid.Width /width;
            else
                gameGrid.Width = width * gameGrid.Height / height;

            for (int x = 0; x < width; x++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(1, GridUnitType.Star);
                gameGrid.ColumnDefinitions.Add(col);
                
            }
            for (int y = 0; y < height; y++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(1, GridUnitType.Star);
                gameGrid.RowDefinitions.Add(row);
            }
            for (int x = 0; x < width; x++)
            {
                buttons.Add(new List<Button>());
                values.Add(new List<int>());
                state.Add(new List<int>());
                for (int y = 0; y < height; y++)
                {
                    Button label = new Button();
                    values[x].Add(0);
                    state[x].Add(0);
                    gameGrid.Children.Add(label);
                    Grid.SetRow(label, y);
                    Grid.SetColumn(label, x);
                    buttons[x].Add(label);
                    label.Click += GenerateBombs;
                }
            }
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
            numberOfFlags = numberOfBombs;


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
        private void showValues()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    buttons[x][y].Content = "r" + x + "c" + y + "v" + values[x][y];
                    if(values[x][y] == -1)
                    {
                        buttons[x][y].Background = new SolidColorBrush(Colors.Red);
                    }
                    else if(values[x][y] == 0)
                    {
                        buttons[x][y].Background = new SolidColorBrush(Colors.Green);
                    }
                    else
                    {
                        buttons[x][y].Background = new SolidColorBrush(Colors.Yellow);
                    }
                }
            }
        }
        private void showValues(object sender, RoutedEventArgs e)
        {
            int clickedX = -1, clickedY = -1;
            Random rand = new Random();
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    if (buttons[x][y] == sender)
                    {
                        clickedX = x;
                        clickedY = y;
                    }
            showField(clickedX, clickedY, true);
        }
        private void showField(int x, int y, bool primary)
        {
            if (values[x][y] == -1 && state[x][y] == 0)
            {  //Game over
                buttons[x][y].Background = new SolidColorBrush(Colors.Red);
                buttons[x][y].Content = "boom";
            }
            if (values[x][y] == 0 && state[x][y] == 0)
            {
                state[x][y] = 1;
                buttons[x][y].Background = new SolidColorBrush(Colors.Green);
                buttons[x][y].Content = "0";
                for(int i = x - 1; i <= x + 1; i++)
                    for(int j = y - 1; j <= y + 1; j++)
                        if (i != -1 && i != width && j != -1 && j != height)
                            showField(i, j, false);
            }
            if(values[x][y] > 0 && state[x][y] == 0)
            {
                state[x][y] = 1;
                buttons[x][y].Background = new SolidColorBrush(Colors.Yellow);
                buttons[x][y].Content = values[x][y].ToString();
            }
            if (values[x][y] > 0 && state[x][y] == 1 && primary && CheckNOFlags(x,y))
            {
                for (int i = x - 1; i <= x + 1; i++)
                    for (int j = y - 1; j <= y + 1; j++)
                        if (i != -1 && i != width && j != -1 && j != height && state[i][j] != 2)
                            showField(i, j, false);
            }

        }
        private bool CheckNOFlags(int x, int y)
        {
            int NOFlags = 0;
            for (int i = x - 1; i <= x + 1; i++)
                for (int j = y - 1; j <= y + 1; j++)
                    if(i != -1 && i != width && j != -1 && j != height)
                        if (state[i][j] == 2)
                            NOFlags++;
            return NOFlags == values[x][y];
        }
        private void GenerateBombs(object sender, RoutedEventArgs e)
        {
            int clickedX = -1, clickedY = -1, numberOfBombs, divident;
            Random rand = new Random();
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    if (buttons[x][y] == sender) {
                        clickedX = x;
                        clickedY = y;   
                    }
            if (clickedX < 0 || clickedY < 0) {
                return;
            }
            List<List<int>> freeFields = new List<List<int>>();
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    if (!((x == clickedX || x == clickedX - 1 || x == clickedX + 1) && (y == clickedY || y == clickedY - 1 || y == clickedY + 1)))
                        freeFields.Add(new List<int> { x, y });
                    values[x][y] = 0;
                }
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
            if (numberOfBombs > freeFields.Count)
                numberOfBombs = freeFields.Count;
            //tworzenie bomb
            for(int i = 0;i < numberOfBombs; i++)
            {
                int currentField = rand.Next() % freeFields.Count;
                List<int> currentCoordinates = freeFields[currentField];
                freeFields.RemoveAt(currentField);
                values[currentCoordinates[0]][currentCoordinates[1]] = -1;
            }
            //ustawianie liczby bomb w sasiedztwie
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if(values[x][y] == -1)
                        continue;
                    values[x][y] = 0;
                    for(int a = x - 1; a <= x + 1; a++)
                    {
                        if(a < 0 || a == width)
                            continue;
                        for (int b = y - 1; b <= y + 1; b++)
                        {
                            if (b < 0 || b == height)
                                continue;
                            if(values[a][b] == -1)
                            {
                                values[x][y] += 1;
                            }
                        }
                    }
                }
            }
            //ustawianie innej funkcji
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    buttons[x][y].Click -= GenerateBombs;
                    buttons[x][y].Click += showValues;
                    buttons[x][y].MouseRightButtonDown += PlaceFlag;
                }
            }
            //showValues();
            showField(clickedX, clickedY, false);
            start = DateTime.Now;
            timer.Start();
        }
        private void PlaceFlag(object sender, RoutedEventArgs e)
        {
            int clickedX = 0, clickedY = 0;
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    if (buttons[x][y] == sender)
                    {
                        clickedX = x;
                        clickedY = y;
                    }
            if (state[clickedX][clickedY] == 0)
            {
                state[clickedX][clickedY] = 2;
                buttons[clickedX][clickedY].Background = new SolidColorBrush(Colors.Orange);
                buttons[clickedX][clickedY].Content = "F";
                numberOfFlags--;
                TxtBoxFlags.Text = "Flagi: " + numberOfFlags;
            }
            else if (state[clickedX][clickedY] == 2)
            {
                state[clickedX][clickedY] = 0;
                buttons[clickedX][clickedY].Background = new SolidColorBrush(Colors.LightGray);
                buttons[clickedX][clickedY].Content = "";
                numberOfFlags--;
                TxtBoxFlags.Text = "Flagi: " + numberOfFlags;
            }
        }
        private void StartNewGame(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
