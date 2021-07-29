using SeaSharp_UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SeaSharp_UI.Entities
{
    public abstract class ConsumableEntity : AbstractEntity
    {

        protected int nutritionalValue;

        protected Image consumableImage;

        protected double canvasWidth;
        protected double canvasHeight;

        private Random random = new Random();

        protected string consumableName;

        protected ConsumableEntity(Dispatcher dispatcher, Canvas mainCanvas)
        {
            this.dispatcher = dispatcher;
            this.mainCanvas = mainCanvas;

            canvasWidth = mainCanvas.ActualWidth;
            canvasHeight = mainCanvas.ActualHeight;
        }

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

        public void SetRandomLocation()
        {
            x = random.NextDouble() * (canvasWidth - entitySize);
            y = random.NextDouble() * (canvasHeight - entitySize);
        }

        public virtual void Start()
        {
            consumableImage = new Image();
            consumableImage.Source = ImageHelper.LoadBitmapImage($"{consumableName}.png", 256);
            consumableImage.Width = this.entitySize;

            mainCanvas.Children.Add(consumableImage);

            Canvas.SetTop(consumableImage, y);
            Canvas.SetLeft(consumableImage, x);
        }

        public override void Destroy()
        {
            dispatcher.BeginInvoke(new Action(() =>
            {

                mainCanvas.Children.Remove(consumableImage);
            }));
            base.Destroy();
        }
    }
}
