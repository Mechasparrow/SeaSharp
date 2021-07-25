using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

using SeaSharp_UI.Helpers;
using SeaSharp_UI.Entities;

namespace SeaSharp_UI.Entities
{
    class Food : ConsumableEntity
    {
        public Food(Dispatcher dispatcher, Canvas mainCanvas) : base(dispatcher, mainCanvas)
        {
            entitySize = 56;
            consumableName = "food";
        }

    }
}
