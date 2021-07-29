using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

using SeaSharp_UI.Helpers;
using SeaSharp_UI.Entities;

namespace SeaSharp_UI
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private World world = null;
        private Creature creature = null;
        private Random rnd = new Random();

        private MoneyManager moneyManager;

        private void InitGameState(Creature creature, MoneyManager moneyManager)
        {
            this.creature = new Creature(Application.Current.Dispatcher, MainCanvas);
            
            this.creature.Name = creature.Name;

            this.creature.DaysPassed = creature.DaysPassed;
            this.creature.Hunger = creature.Hunger;
            this.creature.Thirst = creature.Thirst;
            this.creature.Play = creature.Play;

            this.creature.PropertyChanged += HandleCreatureUpdate;
            this.creature.PropertyRefresh();
            
            this.moneyManager = moneyManager;
            this.moneyManager.MoneyUpdated += HandleMoneyUpdate;
            this.moneyManager.InvokeMoneyUpdate();

            world = new World();

            world.AddEntity(this.creature);

        }

        public GameWindow(Creature creature, MoneyManager moneyManager)
        {
            InitializeComponent();

            this.InitGameState(creature, moneyManager);
        }

        public GameWindow(string selectedCreature)
        {
            InitializeComponent();

            MoneyManager moneyManager = new MoneyManager();
            creature = new Creature(Application.Current.Dispatcher, MainCanvas);
            creature.Name = selectedCreature;

            this.InitGameState(creature, moneyManager);
        }

        public void RenderSea()
        {
            double canvasWidth = MainCanvas.ActualWidth;
            double canvasHeight = MainCanvas.ActualHeight;

            List<Image> imageTiles = new List<Image>();
            
            string[] oceanTileNames =
            {
                "ocean00",
                "ocean01"
            };

            double[] rotations =
            {
                0,
                90,
                180,
                270
            };

            int tilingSize = 64;

            int tilingX = (int) Math.Ceiling(canvasWidth / tilingSize);
            int tilingY = (int)Math.Ceiling(canvasHeight / tilingSize);


            for (int y = 0; y < tilingY; y++)
            {
                double tileYOffset = y * (tilingSize-1);

                for (int x = 0; x < tilingX; x++)
                {
                    double tileXOffset = x * tilingSize;

                    int occeanIdx = rnd.Next(oceanTileNames.Length);
                    string oceanName = oceanTileNames[occeanIdx];


                    int rotationIdx = rnd.Next(rotations.Length);
                    double rotation = rotations[rotationIdx];

                    Image seaTile = new Image();
                    seaTile.Source = ImageHelper.LoadBitmapImage($"{oceanName}.png", tilingSize, rotation);
                    seaTile.Width = tilingSize;

                    MainCanvas.Children.Add(seaTile);

                    Canvas.SetLeft(seaTile, tileXOffset);
                    Canvas.SetTop(seaTile, tileYOffset);
                }
            }
        }

        public void HandleMoneyUpdate(object sender, EventArgs e)
        {
            MoneyText.Text = $"Money: {moneyManager.Money}";
        }

        public void HandleCreatureUpdate(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Name":
                    renderCreatureName(creature.Name);
                    break;
                case "Hunger":
                    renderFoodbar(creature.Hunger);
                    break;
                case "Thirst":
                    renderDrinkbar(creature.Thirst);
                    break;
                case "Death":
                    renderDeathDialog(creature.DaysPassed);
                    break;
                case "Day":
                    renderDaysPassed(creature.DaysPassed);
                    break;
            }
        }

        private void renderDeathDialog(int days)
        {
            creature.Destroy();
            MessageBox.Show($"Your pet has passed out after {days} Days", "Pet fainted", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            GoBackToCharacterSelect(null, null);
        }

        private void renderDaysPassed(int days)
        {
            TimeText.Text = $"Days {creature.DaysPassed}";
        }


        private void renderDrinkbar(double thirst)
        {
            CreatureThirstBar.Value = thirst;
        }

        private void renderFoodbar(double hunger)
        {
            CreatureHungerBar.Value = hunger;
        }

        private void renderCreatureName(string creatureName)
        {
            GameTitle.Text = $"Playing with {creatureName}";
        }

        private void GoBackToCharacterSelect(object sender, RoutedEventArgs e)
        {
            if (creature != null)
            {
                creature.Destroy();
            }

            Window mainWindow = new MainWindow();

            Window currentWindow = App.Current.MainWindow;
            var oldLeftX = currentWindow.Left;
            var oldTopY = currentWindow.Top;

            mainWindow.WindowStartupLocation = WindowStartupLocation.Manual;

            mainWindow.Left = oldLeftX;
            mainWindow.Top = oldTopY;

            App.Current.MainWindow = mainWindow;

            Close();
            mainWindow.Show();
        }

        private void GoToWorkWindow(object sender, RoutedEventArgs e)
        {
            if (creature != null)
            {
                creature.Destroy();
            }

            //TODO save creature state 

            Window mainWindow = new GoToWorkWindow(this.creature, this.moneyManager);

            Window currentWindow = App.Current.MainWindow;
            var oldLeftX = currentWindow.Left;
            var oldTopY = currentWindow.Top;

            mainWindow.WindowStartupLocation = WindowStartupLocation.Manual;

            mainWindow.Left = oldLeftX;
            mainWindow.Top = oldTopY;

            App.Current.MainWindow = mainWindow;

            Close();
            mainWindow.Show();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (creature != null)
            {
                creature.Destroy();
            }
        }

        private void MainCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            RenderSea();

            creature.Start();

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void FeedCreatureButtonClick(object sender, RoutedEventArgs e)
        {
            Food food = new Food(Dispatcher, MainCanvas);
            food.SetRandomLocation();
            food.Start();


            world.AddEntity(food);
        }

        private void GiveWaterCreatureButtonClick(object sender, RoutedEventArgs e)
        {
            Drink drink = new Drink(Dispatcher, MainCanvas);
            drink.SetRandomLocation();
            drink.Start();


            world.AddEntity(drink);
        }

        private void GoToWorkButton_Click(object sender, RoutedEventArgs e)
        {
            GoToWorkWindow(sender, e);
        }
    }
}
