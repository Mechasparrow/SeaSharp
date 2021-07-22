using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace SeaSharp_UI
{

    //https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=net-5.0
    abstract class AbstractCreature : INotifyPropertyChanged
    {

        protected string creatureName;
        protected Dispatcher dispatcher;
        protected Canvas mainCanvas;

        public string Name
        {
            get
            {
                return this.creatureName;
            }
            set
            {
                this.creatureName = value;
                NotifyPropertyChanged("Name");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual BitmapImage LoadBitmapImage(string path)
        {
            string newPath = System.IO.Path.Combine(Environment.CurrentDirectory, path);

            BitmapImage myBitmapImage = new BitmapImage();
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(newPath);
            myBitmapImage.DecodePixelWidth = 128;
            myBitmapImage.EndInit();

            return myBitmapImage;
        }
    }
}
