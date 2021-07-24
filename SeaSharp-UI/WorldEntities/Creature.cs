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

using SeaSharp_UI.Helpers;

namespace SeaSharp_UI.Entities
{
    class Creature : AbstractCreature
    {

        private Thread creatureThread = null;
        private bool paused = false;
        private Image creatureImage = null;

        public Creature(Dispatcher dispatcher, Canvas mainCanvas) : this("", dispatcher, mainCanvas) { }

        public Creature(string creatureName, Dispatcher dispatcher, Canvas mainCanvas)
        {
            this.creatureName = creatureName;
            this.dispatcher = dispatcher;
            this.mainCanvas = mainCanvas;

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

            Canvas.SetLeft(creatureImage, x);
            Canvas.SetTop(creatureImage, y);
        }

        private void CreatureLogic()
        {
            Console.WriteLine("creature running");

            dispatcher.Invoke(() =>
            {
                UpdateLocation(x + 5, y);
            });
        }

        private void CreatureLoop()
        {
            while (true)
            {
                if (paused)
                {
                    continue;
                }

                CreatureLogic();

                Thread.Sleep(100);
            }
            
        }

        public void Shutdown()
        {
            if (creatureThread != null)
            {

                if (creatureThread.ThreadState != ThreadState.Stopped)
                {
                    creatureThread.Abort();
                }

                creatureThread = null;
            }
        }
    }
}
