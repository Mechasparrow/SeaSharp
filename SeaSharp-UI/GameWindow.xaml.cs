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
using System.Windows.Shapes;

namespace SeaSharp_UI
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private string selectedCreature = null;

        public GameWindow(string selectedCreature)
        {
            InitializeComponent();

            this.selectedCreature = selectedCreature;
            this.LogSelectedCreature();
        }

        void LogSelectedCreature()
        {
            System.Console.WriteLine("The selected creature is ");
            System.Console.WriteLine(this.selectedCreature);
        }

    }
}
