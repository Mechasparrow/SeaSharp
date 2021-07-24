using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

using SeaSharp_UI.Helpers;
using SeaSharp_UI.WorldEntities;

namespace SeaSharp_UI.Entities
{
    class Food : AbstractEntity
    {
        private Dispatcher dispatcher;
        private Image foodImage;

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

        public Food(Dispatcher dispatcher, Canvas mainCanvas)
        {
            this.dispatcher = dispatcher;
            this.mainCanvas = mainCanvas;

            this.entitySize = 56;
        }

        public void Start()
        {
            foodImage = new Image();
            string foodName = "food";
            foodImage.Source = ImageHelper.LoadBitmapImage($"{foodName}.png", 256);
            foodImage.Width = this.entitySize;

            mainCanvas.Children.Add(foodImage);

        }

        public override void Destroy()
        {
            mainCanvas.Children.Remove(foodImage);
            base.Destroy();
        }
    }
}
