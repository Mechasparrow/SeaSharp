using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SeaSharp_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<string> availableCreatures = new List<string>()
        {
            "crab",
            "dolphin",
            "seahorse"
        };

        public MainWindow()
        {
            InitializeComponent();

            List<Image> creatureImages = new List<Image>() { CreatureImage1, CreatureImage2, CreatureImage3 };
            string[] creatureNames = availableCreatures.ToArray();

            int i = 0;
            foreach (Image creatureImage in creatureImages)
            {
                creatureImage.Source = ImageHelper.LoadBitmapImage($"{creatureNames[i]}.png", 128);
                creatureImage.DataContext = creatureNames[i];
                i = (i + 1) % creatureImages.Count;
            }
        }

        private void SwitchToGameWindow(string creature)
        {
            Window gameWindow = new GameWindow(creature);

            Window currentWindow = App.Current.MainWindow;
            var oldLeftX = currentWindow.Left;
            var oldTopY = currentWindow.Top;

            gameWindow.WindowStartupLocation = WindowStartupLocation.Manual;

            gameWindow.Left = oldLeftX;
            gameWindow.Top = oldTopY;

            App.Current.MainWindow = gameWindow;

            this.Close();
            gameWindow.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Control control = sender as Control;

            string creatureImageName = control.Name.Replace("Button", "CreatureImage");
            Image creatureImage = FindName(creatureImageName) as Image;
            string creature = creatureImage.DataContext as string;

            SwitchToGameWindow(creature);
        }

    }
}
