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
        public game(int width, int height) {
            InitializeComponent();
            for (int x = 0; x < width; x++)
            {
                
            }
        }
    }
}
