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
using SeaSharp_UI.Entities;

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

        public override string ToString()
        {
            return $"X: {this.dx} Y: {this.dy}";
        }
    }

    enum CreatureState
    {
        MOVING_WITH_NO_PURPOSE,
        HEADING_TOWARDS_CONSUMABLE
    }

    public class Creature : AbstractCreature
    {

        private Thread creatureThread = null;
        
        private bool paused = false;

        private Velocity velocity = new Velocity();

        public AbstractEntity currentTarget = null;

        private CreatureState creatureState = CreatureState.MOVING_WITH_NO_PURPOSE;

        private Image creatureImage = null;

        private readonly int creatureSize = 128;
        
        private int threadTickTime = 100;

        private int daysPassed = 0;

        private double hunger = 100.0;
        private double thirst = 100.0;
        private double play = 100.0;

        private Random random = new Random();

        public AbstractEntity TargetingEntity
        {
            get
            {
                return currentTarget;
            }
        }

        public int DaysPassed
        {
            set
            {
                daysPassed = value;
            }
            get
            {
                return daysPassed;
            }
        }

        public double Thirst
        {
            set
            {
                thirst = value;
            }
            get
            {
                return thirst;
            }
        }

        public double Hunger
        {
            set
            {
                hunger = value;
            }
            get
            {
                return hunger;
            }
        }

        public double Play
        {
            set
            {
                play = value;
            }
            get
            {
                return play;
            }
        }

        private double canvasWidth = 0;
        private double canvasHeight = 0;

        bool updateDirection = false;

        Timer movementTimer;
        Timer consumableTimer;
        Timer dayTimer;

        public Creature(Dispatcher dispatcher, Canvas mainCanvas) : this("", dispatcher, mainCanvas) { }

        public Creature(string creatureName, Dispatcher dispatcher, Canvas mainCanvas)
        {
            this.name = creatureName;
            this.dispatcher = dispatcher;
            this.mainCanvas = mainCanvas;

            this.entitySize = creatureSize;


            creatureThread = new Thread(CreatureLoop);
        }

        public void PropertyRefresh()
        {
            dispatcher.BeginInvoke(new Action(() =>
            {
                NotifyPropertyChanged("Day");

                NotifyPropertyChanged("Hunger");
                NotifyPropertyChanged("Thirst");
                NotifyPropertyChanged("Play");
            }));
        }

        private void UpdateMovementTick(object state)
        {
            updateDirection = true;
        }

        public void UpdateDayTick(object state)
        {
            dispatcher.BeginInvoke(new Action(() =>
            {
                daysPassed += 1;
                NotifyPropertyChanged("Day");
            }));
        }

        public void UpdateConsumables(object state)
        {
            dispatcher.BeginInvoke(new Action(() =>
            {
                hunger = Math.Max(hunger - 5.0, 0);
                thirst = Math.Max(thirst - 5.0, 0);
                play = Math.Max(play - 5.0, 0);

                NotifyPropertyChanged("Hunger");
                NotifyPropertyChanged("Thirst");
                NotifyPropertyChanged("Play");

                if (hunger <= 0 || thirst <= 0 || play <= 0)
                {
                    NotifyPropertyChanged("Death");
                }
            }));
        }

        public void Start()
        {
            creatureImage = new Image();
            creatureImage.Source = ImageHelper.LoadBitmapImage($"{Name}.png", 256);
            creatureImage.Width = creatureSize;

            mainCanvas.Children.Add(creatureImage);

            canvasWidth = mainCanvas.ActualWidth;
            canvasHeight = mainCanvas.ActualHeight;

            int midWidth = (int)canvasWidth / 2;
            int midHeight = (int)canvasHeight / 2;

            UpdateLocation(midWidth - (creatureSize/2), midHeight - (creatureSize/2));


            creatureThread.Start();
        }

        protected override void UpdateLocation(double newX, double newY)
        {
            base.UpdateLocation(newX, newY);

            dispatcher.BeginInvoke(new Action(() =>
            {
                Canvas.SetLeft(creatureImage, x);
                Canvas.SetTop(creatureImage, y);

            }));
        }

        public Creature duplicateCreature(Dispatcher dispatcher, Canvas mainCanvas)
        {
            Creature duplicatedCreature = new Creature(dispatcher, mainCanvas);

            duplicatedCreature.Name = this.Name;

            duplicatedCreature.DaysPassed = this.DaysPassed;
            duplicatedCreature.Hunger = this.Hunger;
            duplicatedCreature.Thirst = this.Thirst;
            duplicatedCreature.Play = this.Play;

            return duplicatedCreature;
        }

        private void CreatureLogic()
        {
            if (creatureState == CreatureState.MOVING_WITH_NO_PURPOSE)
            {

                double oldDx = velocity.dx;

                if (updateDirection)
                {
                    velocity = GenerateRandomVelocity();
                    updateDirection = false;
                }

                CanvasBoundsCheck(canvasWidth, canvasHeight);

                double newDx = velocity.dx;

                DirectionFlipUpdate(oldDx, newDx);

                
                UpdateLocation(x + velocity.dx, y + velocity.dy);

            }
            else if (creatureState == CreatureState.HEADING_TOWARDS_CONSUMABLE)
            {

                CanvasBoundsCheck(canvasWidth, canvasHeight);

                UpdateLocation(x + velocity.dx, y + velocity.dy);
            }

        }

        private void CreatureLoop()
        {

            movementTimer = new Timer(UpdateMovementTick, null, 0, 1000);
            consumableTimer = new Timer(UpdateConsumables, null, 1000, 2000);
            dayTimer = new Timer(UpdateDayTick, null, 5000, 5000);

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

                dispatcher.InvokeAsync(() =>
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

        private Velocity velocityTowardsTarget(double targetX, double targetY, double magnitude)
        {
            double distanceX = targetX - x;
            double distanceY = targetY - y;

            double targetDistance = Math.Sqrt(Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2));

            double normalizedX = distanceX / targetDistance;
            double normalizedY = distanceY / targetDistance;

            return new Velocity(normalizedX * magnitude, normalizedY * magnitude);
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

        public override void HandleWorldUpdate(object sender, WorldEventArgs worldEventArgs)
        {
            base.HandleWorldUpdate(sender, worldEventArgs);

            World world = worldEventArgs.OccuringWorld;


            List<ConsumableEntity> consumableEntities;

            switch (worldEventArgs.WorldEventType)
            {
                case WorldEventType.ENTITY_CHANGE:
                    consumableEntities = world.FindConsumableEntities();


                    if (consumableEntities.Count > 0)
                    {
                        creatureState = CreatureState.HEADING_TOWARDS_CONSUMABLE;

                        if (currentTarget == null)
                        {
                            AbstractEntity consumableEntity = consumableEntities.OrderBy((entity) => EntityDistance(entity, this)).First();
                            currentTarget = consumableEntity;

                            velocity = velocityTowardsTarget(currentTarget.X, currentTarget.Y, 10);
                        }

                    }
                    break;
                case WorldEventType.ENTITY_COLLISION:
                    List<AbstractEntity> affectedEntities = worldEventArgs.affectedEntites;
                    
                    if (affectedEntities.Contains(this) && affectedEntities.Contains(currentTarget))
                    {
                        AbstractEntity targetingEntity = affectedEntities.First(entity => entity == currentTarget);

                        string updatingConsumableProperty = string.Empty;
                        Type consumableType = targetingEntity.GetType();

                        if (consumableType == typeof(Food))
                        {
                            updatingConsumableProperty = "Hunger";
                        }
                        else if (consumableType == typeof(Drink))
                        {
                            updatingConsumableProperty = "Thirst";
                        }else if (consumableType == typeof(Ball))
                        {
                            updatingConsumableProperty = "Play";
                        }

                        world.RemoveEntity(targetingEntity);
                        currentTarget = null;

                        dispatcher.BeginInvoke(new Action(() =>
                        {
                            switch (updatingConsumableProperty)
                            {
                                case "Hunger":
                                    hunger = Math.Min(hunger + 10,100);
                                    break;
                                case "Thirst":
                                    thirst = Math.Min(thirst + 10, 100);
                                    break;
                                case "Play":
                                    play = Math.Min(play + 10, 100);
                                    break;
                            }

                            NotifyPropertyChanged(updatingConsumableProperty);
                        }));

                        consumableEntities = world.FindConsumableEntities();
                        if (consumableEntities.Count > 0)
                        {
                            if (currentTarget == null)
                            {
                                currentTarget = consumableEntities.OrderBy((entity) => EntityDistance(entity, this)).First();

                                velocity = velocityTowardsTarget(currentTarget.X, currentTarget.Y, 10);
                            }

                        }
                        else
                        {
                            creatureState = CreatureState.MOVING_WITH_NO_PURPOSE;
                        }
                    }

                    break;
            }

            
        }

        public override void Destroy()
        {
            mainCanvas.Children.Remove(creatureImage);

            movementTimer.Dispose();
            dayTimer.Dispose();
            consumableTimer.Dispose();

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
