using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SeaSharp_UI
{
    class Creature : AbstractCreature
    {

        private Thread creatureThread = null;
        private bool paused = false;
        private bool creatureDestroyed = false;

        private Image creatureImage = null;

        public Creature(Dispatcher dispatcher, Canvas mainCanvas) : this("", dispatcher, mainCanvas) { }

        public Creature(string creatureName, Dispatcher dispatcher, Canvas mainCanvas)
        {
            this.creatureName = creatureName;
            this.dispatcher = dispatcher;
            this.mainCanvas = mainCanvas;
            this.x = 50;
            this.y = 50;

            creatureThread = new Thread(CreatureLoop);

        }

        public void Start()
        {
            int creatureSize = 128;
            creatureImage = new Image();
            creatureImage.Source = ImageHelper.LoadBitmapImage($"{Name}.png", 256);
            creatureImage.Width = creatureSize;

            mainCanvas.Children.Add(creatureImage);

            int midWidth = (int)mainCanvas.ActualWidth / 2;
            int midHeight = (int)(mainCanvas.ActualHeight) / 2;

            UpdateLocation(midWidth - (creatureSize/2), midHeight - (creatureSize/2));

            creatureThread.Start();
        }

        protected override void UpdateLocation(int newX, int newY)
        {
            base.UpdateLocation(newX, newY);

            Canvas.SetLeft(creatureImage, this.x);
            Canvas.SetTop(creatureImage, this.y);
        }

        private void CreatureLoop()
        {
            while (!creatureDestroyed)
            {
                if (paused)
                {
                    continue;
                }

                Console.WriteLine("creature running");

                Thread.Sleep(1000);
            }
            
        }

        public void Shutdown()
        {
            if (creatureThread != null)
            {
                creatureDestroyed = true;

                if (creatureThread.ThreadState != ThreadState.Stopped)
                {
                    creatureThread.Abort();
                }

                creatureThread = null;
            }
        }
    }
}
