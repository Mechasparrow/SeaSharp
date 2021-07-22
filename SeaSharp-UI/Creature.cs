using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SeaSharp_UI
{
    //https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=net-5.0
    class Creature : INotifyPropertyChanged
    {
        private string creatureName;
        private Dispatcher dispatcher;
        private Canvas mainCanvas;

        public event PropertyChangedEventHandler PropertyChanged;
        
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

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

        public Creature(Dispatcher dispatcher, Canvas mainCanvas) : this("", dispatcher, mainCanvas)
        {
        }

        public Creature(string creatureName, Dispatcher dispatcher, Canvas mainCanvas)
        {
            this.creatureName = creatureName;
            this.dispatcher = dispatcher;
            this.mainCanvas = mainCanvas;
        }
    }
}
