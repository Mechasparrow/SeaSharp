using SeaSharp_UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SeaSharp_UI.Entities
{
    class Drink : ConsumableEntity
    {
        public Drink(Dispatcher dispatcher, Canvas mainCanvas) : base(dispatcher, mainCanvas)
        {    
            this.entitySize = 56;
            consumableName = "drink";
        }

    }
}
