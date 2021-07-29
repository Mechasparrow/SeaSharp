using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SeaSharp_UI.Entities
{
    class Ball : ConsumableEntity
    {
        public Ball(Dispatcher dispatcher, Canvas mainCanvas) : base(dispatcher, mainCanvas)
        {
            this.entitySize = 56;
            consumableName = "beachball";
        }
    }
}
