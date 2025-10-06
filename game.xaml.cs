using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public List<List<bool>> shown = new List<List<bool>>();
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
                shown.Add(new List<bool>());
                for (int y = 0; y < height; y++)
                {
                    Button label = new Button();
                    values[x].Add(0);
                    shown[x].Add(false);
                    gameGrid.Children.Add(label);
                    Grid.SetRow(label, y);
                    Grid.SetColumn(label, x);
                    buttons[x].Add(label);
                    label.Click += GenerateBombs;
                }
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
            showField(clickedX, clickedY);
        }
        private void showField(int x, int y)
        {
            if (values[x][y] == -1)      //Game over
                return;
            if(values[x][y] == 0 && !shown[x][y])
            {
                shown[x][y] = true;
                buttons[x][y].Background = new SolidColorBrush(Colors.Green);
                buttons[x][y].Content = "0";
                for(int i = x - 1; i <= x + 1; i++)
                {
                    if (i == -1 || i == width)
                        continue;
                    for(int j = y - 1; j <= y + 1; j++)
                    {
                        if (j == -1 || j == height)
                            continue;
                        showField(i, j);
                    }
                }
            }
            if(values[x][y] > 0 && !shown[x][y])
            {
                shown[x][y] = true;
                buttons[x][y].Background = new SolidColorBrush(Colors.Yellow);
                buttons[x][y].Content = values[x][y].ToString();
            }

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
                    divident = 4;
                    break;
                case 2:
                    divident = 3;
                    break;
                case 3:
                    divident = 2;
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
                }
            }
            //showValues();
            showField(clickedX, clickedY);
        }

        private void StartNewGame(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
