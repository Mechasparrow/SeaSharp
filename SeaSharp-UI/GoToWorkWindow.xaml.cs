using SeaSharp_UI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SeaSharp_UI
{
    /// <summary>
    /// Interaction logic for GoToWorkWindow.xaml
    /// </summary>
    public partial class GoToWorkWindow : Window
    {
        private List<Button> oceanButtons;

        private int depth = 0;
        private bool flipped = false;

        private Random random = new Random();

        private MoneyManager moneyManager;
        private Creature creature;

        private ImageSource currentDefaultImageSource;

        private readonly ImageSource regularImageSource;
        private readonly ImageSource flippedImageSource;
        private readonly ImageSource discoveryImageSource;

        public event EventHandler MoneyUpdated;

        enum OceanState
        {
            OCEAN_DISCOVERY,
            OCEAN_NORMAL
        }

        public GoToWorkWindow(Creature creature): this(creature, new MoneyManager())
        {

        }

        public GoToWorkWindow(Creature creature, MoneyManager moneyManager)
        {
            InitializeComponent();

            this.creature = creature;
            this.moneyManager = moneyManager;
            this.moneyManager.MoneyUpdated += HandleMoneyUpdate;
            this.moneyManager.InvokeMoneyUpdate();

            oceanButtons = OceanGrid.Children.OfType<Button>().ToList();

            regularImageSource = Helpers.ImageHelper.LoadBitmapImage("ocean-base.png", 128);
            flippedImageSource = Helpers.ImageHelper.LoadBitmapImage("ocean-base.png", 128, true);
            discoveryImageSource = Helpers.ImageHelper.LoadBitmapImage("ocean-discovery-01.png", 128);

            regularImageSource.Freeze();
            flippedImageSource.Freeze();
            discoveryImageSource.Freeze();

            currentDefaultImageSource = regularImageSource;

            ClearSeaButtons();
        }

        private void InvokeMoneyUpdate()
        {
            EventHandler moneyHandler = MoneyUpdated;

            if (moneyHandler != null)
            {
                moneyHandler.Invoke(this, new EventArgs());
            }
        }

        private void HandleMoneyUpdate(object source, EventArgs e)
        {
            MoneyText.Text = $"Money: {moneyManager.Money}";
        }

        private void HandleDepthUpdate(int depth)
        {
            DepthTextBlock.Text = $"Depth: {depth}";
        }

        private void SeaButtonClick(object sender, RoutedEventArgs e)
        {
            Button selectedButton = sender as Button;
            OceanState? selectedButtonState = selectedButton.DataContext as OceanState?;

            if (selectedButtonState.HasValue)
            {
                switch (selectedButtonState.Value)
                {
                    case OceanState.OCEAN_DISCOVERY:
                        moneyManager.Money += 10;
                        ClearSeaButtons();
                        break;
                }
            }
        }

        private void ClearSeaButtons()
        {

            flipped = !flipped;

            currentDefaultImageSource = flipped ? flippedImageSource : regularImageSource;

            foreach (Button btn in oceanButtons)
            {
                (btn.Content as Image).Source = currentDefaultImageSource;
                btn.DataContext = OceanState.OCEAN_NORMAL;
            }
        }

        private void PopulateSeaButtons()
        {
            ClearSeaButtons();

            Button[] buttons = oceanButtons.ToArray();
            Button button = buttons[random.Next(0, buttons.Length)];
            (button.Content as Image).Source = discoveryImageSource;
            button.DataContext = OceanState.OCEAN_DISCOVERY;
        }

        private void DiveDeeper(object sender, RoutedEventArgs e)
        {
            depth += 10;
            HandleDepthUpdate(depth);
            PopulateSeaButtons();
        }

        private void StopWorking(object sender, RoutedEventArgs e)
        {
 

            GameWindow mainWindow = new GameWindow(this.creature, this.moneyManager);

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
    }
}
