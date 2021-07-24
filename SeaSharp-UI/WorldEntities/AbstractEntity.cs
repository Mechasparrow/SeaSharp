using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SeaSharp_UI.WorldEntities
{
    abstract class AbstractEntity : INotifyPropertyChanged
    {
        protected Canvas mainCanvas;

        protected string name;

        protected double x;
        protected double y;

        protected double entitySize;

        public double EntitySize
        {
            get
            {
                return entitySize;
            }
        }

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
                return name;
            }
            set
            {
                name = value;
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

        public virtual void HandleWorldUpdate(object sender, WorldEventArgs worldEventArgs)
        {

        }

        public virtual void Destroy()
        {

        }
    }
}
