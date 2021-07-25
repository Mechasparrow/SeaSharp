
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
    abstract class AbstractCreature : AbstractEntity 
    {
        
        protected virtual void UpdateLocation(double newX, double newY)
        {
            x = newX;
            y = newY;

            NotifyPropertyChanged("location");
        }
    }
}
