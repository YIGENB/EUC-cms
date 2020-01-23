using System;
using System.Collections.Generic;
using System.Text;

namespace DY.Site
{
    public class CashRebate : CashSuper
    {
        private decimal moneyRebate = 1M;
        public CashRebate(string moneyRebate)
        {
            this.moneyRebate = decimal.Parse(moneyRebate);
        }

        public override decimal acceptCash(decimal money)
        {
            return money * moneyRebate;
        } 
    }
}
