using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Windows.Controls.Image;

namespace Saper
{
    internal class Field
    {
        Board parent;
        private Image ShownImage;
        private int value;
        private int state; // 0 - covered; 1 - uncovered; 2 - flagged
        public Field(Board par)
        {
            this.parent = par; 
            value = 0;
            ShownImage = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("pack://application:,,,/Images/trawa.png");
            bitmap.DecodePixelWidth = 200;
            bitmap.EndInit();

            ShownImage.Source = bitmap;
            state = 0;
        }
        public Image GetImage() {
            return ShownImage;
        }
        public int GetValue()
        {
            return value;
        }
        public int GetState()
        {
            return state;
        }
        public void SetImage(Image image)
        {
            ShownImage = image;
        }
        public void SetValue(int val)
        {
            value = val;
        }
        public void SetState(int st)
        {
            state = st;
        }
        public void Show()
        {

        }
        public int ToggleFlag()
        {
            if (state == 0)
            {
                state = 2;
                //pokaż flage
                //TxtBoxFlags.Text = "Flagi: " + numberOfFlags;
                return -1;
            }
            else if (state == 2)
            {
                state = 0;
                //pokaż flage

                //TxtBoxFlags.Text = "Flagi: " + numberOfFlags;
                return 1;
            }
            return 0;
        }
        
    }

}
