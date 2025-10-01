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
        public game(int width, int height, int dif) {
            InitializeComponent();
            gameGrid.Children.Clear();
            dificulty = dif;
            firstMove = true;
            this.height = height;
            this.width = width;
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
                for (int y = 0; y < height; y++)
                {
                    Button label = new Button();
                    values[x].Add(0);
                    gameGrid.Children.Add(label);
                    Grid.SetRow(label, y);
                    Grid.SetColumn(label, x);
                    buttons[x].Add(label);
                    label.Click += (s,e)=>GenerateBombs(s, e);
                }
            }
            showValues();
        }
        private void showValues()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    buttons[x][y].Content = "r" + x + "c" + y + "v" + values[x][y];
                }
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
            for(int x = 0; x < width; x++)
                for(int y = 0; y < height; y++)
                    if (x != clickedX && x != clickedX - 1 && x != clickedX + 1 && y != clickedY && y != clickedY - 1 && y != clickedY + 1)
                        freeFields.Add(new List<int> { x, y });
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
            numberOfBombs = width * height / 2;
            if (numberOfBombs > freeFields.Count)
                numberOfBombs = freeFields.Count;
            for(int i = 0;i < width; i++)
            {
                List<int> currentCoordinates = freeFields[rand.Next() % freeFields.Count];
                values[currentCoordinates[0]][currentCoordinates[1]] = -1;
            }
            showValues();
        }

        private void StartNewGame(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
