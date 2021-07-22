using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SeaSharp_UI
{
    class Creature : AbstractCreature
    {

        private Thread creatureThread = null;
        private bool paused = false;
        private bool creatureDestroyed = false;

        public Creature(Dispatcher dispatcher, Canvas mainCanvas) : this("", dispatcher, mainCanvas) { }

        public Creature(string creatureName, Dispatcher dispatcher, Canvas mainCanvas)
        {
            this.creatureName = creatureName;
            this.dispatcher = dispatcher;
            this.mainCanvas = mainCanvas;

            creatureThread = new Thread(CreatureLoop);
        }

        public void Start()
        {
            creatureThread.Start();
        }

        public void CreatureLoop()
        {
            while (!creatureDestroyed)
            {
                if (paused)
                {
                    continue;
                }

                Console.WriteLine("creature running");

                Thread.Sleep(1000);
            }
            
        }

        public void Shutdown()
        {
            if (creatureThread != null)
            {
                creatureDestroyed = true;

                if (creatureThread.ThreadState != ThreadState.Stopped)
                {
                    creatureThread.Abort();
                }

                creatureThread = null;
            }
        }
    }
}
