using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SeaSharp_UI
{
    class Food
    {
        private Dispatcher dispatcher;
        private Canvas mainCanvas;
        private Image foodImage;

        private int x;
        private int y;
        private int nutritionalValue;


        public int NutritionalValue
        {
            get
            {
                return nutritionalValue;
            }
            set
            {
                nutritionalValue = value;
            }
        }

        public int X { 
            get
            {
                return x;
            } 
            set
            {
                x = value;        
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        public Food(Dispatcher dispatcher, Canvas mainCanvas)
        {
            this.dispatcher = dispatcher;
            this.mainCanvas = mainCanvas;
        }

        public void Start()
        {
            int foodSize = 56;
            foodImage = new Image();
            string foodName = "food";
            foodImage.Source = ImageHelper.LoadBitmapImage($"{foodName}.png", 256);
            foodImage.Width = foodSize;

            mainCanvas.Children.Add(foodImage);

        }
    }
}
