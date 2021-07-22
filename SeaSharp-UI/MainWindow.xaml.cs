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
        public MainWindow()
        {
            InitializeComponent();
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


            string creature = null;

            switch (control.Name)
            {
                case "CrabBtn":
                    creature = "crab";
                    break;
                case "DolphinBtn":
                    creature = "dolphin";
                    break;
                case "SeahorseBtn":
                    creature = "seahorse";
                    break;
                default:
                    creature = "crab";
                    break;
            }

            SwitchToGameWindow(creature);
        }

    }
}
