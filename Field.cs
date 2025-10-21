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
            bitmap.UriSource = new Uri("pack://application:,,,/Images/grass.png");
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
        public void SetImage(string name)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("pack://application:,,,/Images/" + name);
            bitmap.DecodePixelWidth = 200;
            bitmap.EndInit();

            ShownImage.Source = bitmap;
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
            state = 1;
            switch (value)
            {
                case 0:
                    SetImage("empty.png");
                    break;
                case 1:
                    SetImage("one.png");
                    break;
                case 2:
                    SetImage("two.png");
                    break;
                case 3:
                    SetImage("three.png");
                    break;
                case 4:
                    SetImage("four.png");
                    break;
                case 5:
                    SetImage("five.png");
                    break;
                case 6:
                    SetImage("six.png");
                    break;
                case 7:
                    SetImage("seven.png");
                    break;
                case 8:
                    SetImage("eight.png");
                    break;
                case -1:
                    SetImage("bomb.png");
                    break;

            }

        }
        public int ToggleFlag()
        {
            if (state == 0)
            {
                state = 2;
                //pokaż flage
                SetImage("flag.png");
                return -1;
            }
            else if (state == 2)
            {
                state = 0;
                //ukryj flage

                SetImage("grass.png");
                return 1;
            }
            return 0;
        }
        
    }

}
