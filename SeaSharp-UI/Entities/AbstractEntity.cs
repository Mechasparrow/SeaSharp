using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SeaSharp_UI.Entities
{
    abstract class AbstractEntity : INotifyPropertyChanged
    {
        protected Canvas mainCanvas;
        protected Dispatcher dispatcher;

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

        public static double EntityDistance(AbstractEntity entity, AbstractEntity otherEntity)
        {
            double dx = entity.x - otherEntity.x;
            double dy = entity.y - otherEntity.y;

            return Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
        }

        public virtual void HandleWorldUpdate(object sender, WorldEventArgs worldEventArgs)
        {

        }

        public virtual void Destroy()
        {

        }
    }
}
