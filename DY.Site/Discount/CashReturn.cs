using System;
using System.Collections.Generic;
using System.Text;

namespace DY.Site
{
    public class CashReturn : CashSuper
    {
        private decimal moneyCondition = 0.0M;
        private decimal moneyReturn = 0.0M;
        
        public CashReturn(string moneyCondition,string moneyReturn)
        {
            this.moneyCondition = decimal.Parse(moneyCondition);
            this.moneyReturn = decimal.Parse(moneyReturn);
        }

        public override decimal acceptCash(decimal money)
        {
            decimal result = money;
            if (money >= moneyCondition)
                result=money- Math.Floor(money / moneyCondition) * moneyReturn;
                
            return result;
        } 
    }
}
