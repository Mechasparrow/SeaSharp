using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaSharp_UI
{
    public class MoneyManager
    {
        private int money;

        public event EventHandler MoneyUpdated;

        public int Money
        {
            set
            {
                this.money = value;
                InvokeMoneyUpdate();
            }
            get
            {
                return this.money;
            }
        }

        public void InvokeMoneyUpdate()
        {
            EventHandler moneyUpdatedHandler = MoneyUpdated;

            if (moneyUpdatedHandler != null)
            {
                moneyUpdatedHandler.Invoke(this, null);
            }
        }

        public MoneyManager() : this(0)
        {
        }

        public MoneyManager(int money)
        {
            this.money = money;
        }
    }
}
