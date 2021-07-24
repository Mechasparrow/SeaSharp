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

namespace SeaSharp_UI.Entities
{

    //https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=net-5.0
    //^ This should help isolate the Model, View, and Controller
    abstract class AbstractCreature : INotifyPropertyChanged
    {

        protected string creatureName;
        protected Dispatcher dispatcher;
        protected Canvas mainCanvas;

        protected double x;
        protected double y;

        public double X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        public string Name
        {
            get
            {
                return creatureName;
            }
            set
            {
                creatureName = value;
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

        protected virtual void UpdateLocation(double newX, double newY)
        {
            x = newX;
            y = newY;
        }

    }
}
