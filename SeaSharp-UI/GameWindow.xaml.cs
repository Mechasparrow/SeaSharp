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


        public GameWindow(string selectedCreature)
        {
            InitializeComponent();

            world = new World();

            creature = new Creature(Application.Current.Dispatcher, MainCanvas);
            creature.PropertyChanged += HandleCreatureUpdate;
            creature.Name = selectedCreature;

            world.AddEntity(creature);

            renderDaysPassed(0);
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
                case "Day":
                    renderDaysPassed(creature.DaysPassed);
                    break;
            }
        }

        private void renderDaysPassed(int days)
        {
            TimeText.Text = $"Days {creature.DaysPassed}";
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
    }
}
