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

namespace SeaSharp_UI
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private Creature creature = null;

        public GameWindow(string selectedCreature)
        {
            InitializeComponent();

            creature = new Creature(Dispatcher, MainCanvas);
            creature.PropertyChanged += HandleCreatureUpdate;

            creature.Name = selectedCreature;

            creature.Start();
        }

        public void HandleCreatureUpdate(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Name":
                    renderCreatureName(creature.Name);
                    break;
            }
        }

        private void renderCreatureName(string creatureName)
        {
            GameTitle.Text = $"Playing with {creatureName}";
        }

        private void GoBackToCharacterSelect(object sender, RoutedEventArgs e)
        {
            if (creature != null)
            {
                creature.Shutdown();
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
                creature.Shutdown();
            }
        }
    }
}
