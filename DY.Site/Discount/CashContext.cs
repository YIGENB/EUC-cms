using System;
using System.Collections.Generic;
using System.Text;

namespace DY.Site
{
    public class CashContext
    {
        private CashSuper cs;

        public void setBehavior(CashSuper csuper)
        {
            this.cs = csuper;
        }

        public decimal GetResult(decimal money)
        {
            return cs.acceptCash(money);
        }
    }
}
