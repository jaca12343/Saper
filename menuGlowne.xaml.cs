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
    /// Logika interakcji dla klasy menuGlowne.xaml
    /// </summary>
    public partial class menuGlowne : Page
    {
        public menuGlowne()
        {
            InitializeComponent();
        }
        private void Slider_ValueChangedX(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TextBox textBox = txtboxWidth;
            Slider slider = (Slider)sender;
            int sliderValue = (int)Math.Round(slider.Value);

            textBox.Text = "szerokość: " + sliderValue.ToString();
        }
        private void Slider_ValueChangedY(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TextBox textBox = txtboxHeight;

            Slider slider = (Slider)sender;
            int sliderValue = (int)Math.Round(slider.Value);

            textBox.Text = "wysokość: " + sliderValue.ToString();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new game());
        }
    }
}
