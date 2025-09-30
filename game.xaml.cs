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
        public game()
        {
            InitializeComponent();
        }
        public List<List<Label>> labels = new List<List<Label>>();
        public game(int width, int height, int dificulty) {
            InitializeComponent();
            gameGrid.Children.Clear();
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
                labels.Add(new List<Label>());
                for (int y = 0; y < height; y++)
                {
                    Label label = new Label();
                    label.Content = "r" + x + "c" + y;
                    gameGrid.Children.Add(label);
                    Grid.SetRow(label, y);
                    Grid.SetColumn(label, x);
                    labels[x].Add(label);
                }
            }
        }
        public int dificulty;

        private void startNewGame(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
