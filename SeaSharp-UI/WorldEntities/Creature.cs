using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

using SeaSharp_UI.Helpers;

namespace SeaSharp_UI.Entities
{
    struct Velocity
    {
        public double dx;
        public double dy;
       
        public Velocity(double dx, double dy)
        {
            this.dx = dx;
            this.dy = dy;
        }
    }

    enum CreatureState
    {
        MOVING_WITH_NO_PURPOSE
    }

    class Creature : AbstractCreature
    {

        private Thread creatureThread = null;
        
        private bool paused = false;

        private Image creatureImage = null;

        private Velocity velocity = new Velocity();
        
        private CreatureState creatureState = CreatureState.MOVING_WITH_NO_PURPOSE;

        private readonly int creatureSize = 128;

        private double timeElapsed = 0.0;
        private double timeThreshold = 1.0;

        private int threadTickTime = 100;

        private Random random = new Random();

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
            creatureImage = new Image();
            creatureImage.Source = ImageHelper.LoadBitmapImage($"{Name}.png", 256);
            creatureImage.Width = creatureSize;

            mainCanvas.Children.Add(creatureImage);
            
            int midWidth = (int)mainCanvas.ActualWidth / 2;
            int midHeight = (int)mainCanvas.ActualHeight / 2;

            UpdateLocation(midWidth - (creatureSize/2), midHeight - (creatureSize/2));

            creatureThread.Start();
        }

        protected override void UpdateLocation(double newX, double newY)
        {
            base.UpdateLocation(newX, newY);

            Canvas.SetLeft(creatureImage, x);
            Canvas.SetTop(creatureImage, y);
        }

        private void CreatureLogic()
        {
            double canvasWidth = 0;
            double canvasHeight = 0;

            bool updateDirection = false;

            timeElapsed += threadTickTime / 1000.0;
            if (timeElapsed >= timeThreshold)
            {
                updateDirection = true;
                timeElapsed = 0;
            }


            if (creatureState == CreatureState.MOVING_WITH_NO_PURPOSE)
            {
                dispatcher.Invoke(() =>
                {
                    canvasWidth = mainCanvas.ActualWidth;
                    canvasHeight = mainCanvas.ActualHeight;
                });


                double oldDx = velocity.dx;

                if (updateDirection)
                {
                    velocity = GenerateRandomVelocity();
                }

                CanvasBoundsCheck(canvasWidth, canvasHeight);

                double newDx = velocity.dx;

                DirectionFlipUpdate(oldDx, newDx);

                dispatcher.Invoke(() =>
                {
                    UpdateLocation(x + velocity.dx, y + velocity.dy);
                });
            }


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

                Thread.Sleep(threadTickTime);
            }
            
        }

        private void DirectionFlipUpdate(double oldDx, double newDx)
        {
            double velocityDiff = oldDx - newDx;


            if (oldDx != newDx && newDx != 0)
            {

                dispatcher.Invoke(() =>
                {
                    BitmapSource bitmapSource = null;

                    if (velocityDiff < 1)
                    {
                        bitmapSource = ImageHelper.LoadBitmapImage($"{Name}.png", 256, false);
                    }
                    else
                    {
                        bitmapSource = ImageHelper.LoadBitmapImage($"{Name}.png", 256, true);
                    }

                    creatureImage.Source = bitmapSource;
                });
            }

        }

        private void CanvasBoundsCheck(double canvasWidth, double canvasHeight)
        {
            bool bottomBoundReached = y + velocity.dy > canvasHeight - creatureSize;
            bool topBoundReached = y + velocity.dy < 0;
            if (topBoundReached || bottomBoundReached)
            {
                velocity.dy *= -1;
            }

            bool rightBoundReached = x + velocity.dx > canvasWidth - creatureSize;
            bool leftBoundReached = x + velocity.dx < 0;
            if (leftBoundReached || rightBoundReached)
            {
                velocity.dx *= -1;
            }
        }

        private Velocity GenerateRandomVelocity()
        {
            Velocity[] directions = new Velocity[]
                       {
                        new Velocity(10,0),

                        new Velocity(-10,0),

                        new Velocity(0,10),

                        new Velocity(0,-10),

                        new Velocity(0,0),
                       };

            Velocity selectedVelocity = directions[random.Next(directions.Length)];

            return selectedVelocity;
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
