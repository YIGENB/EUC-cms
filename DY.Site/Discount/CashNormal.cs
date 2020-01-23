using System;
using System.Collections.Generic;
using System.Text;

namespace DY.Site
{
    public class CashNormal : CashSuper
    {
        public override decimal acceptCash(decimal money)
        {
            return money;
        } 
    }
}
