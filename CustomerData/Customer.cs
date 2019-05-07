using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerData
{
    public class Customer
    {
        // Class Variables
        public int AccountNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerType { get; set; }
        public decimal ChargeAmount { get; set; }

        // Class Constructors
        public Customer(int AccountNo, string CustomerName, string CustomerType, decimal ChargeAmount)
        {
            this.AccountNo = AccountNo;
            this.CustomerName = CustomerName;
            this.CustomerType = CustomerType;
            this.ChargeAmount = ChargeAmount;
        }

        public Customer()
        {
        }

        // Class Methods
        public string displayToString()
        {
            string Account = Convert.ToString(AccountNo);
            string Name = CustomerName;
            string Type = CustomerType;
            string Charge = Convert.ToString(ChargeAmount);
            return Account + "," + Name + "," + Type + "," + Charge;
        }

        public decimal calculateCharge(decimal usage, decimal offPeakUsage)
        {
            if(CustomerType == "R")
            {
                decimal baseCharge = 6.00m;
                decimal baseRate = 0.052m;
                decimal answer = (usage * baseRate) + baseCharge;
                return answer;
            }

            else if(CustomerType == "C")
            {
                decimal baseCharge = 60.00m;
                decimal baseRate = 0.045m;
                decimal overBaseCharge = ((usage - 1000.00m) * baseRate);

                if (usage <= 1000)
                {
                    return baseCharge;
                }
                else
                {
                    return (baseCharge + overBaseCharge);
                }
            }

            else 
            {
                decimal peakCharge = 76.00m;
                decimal peakRate = 0.065m;
                decimal offPeakCharge = 40.00m;
                decimal offPeakRate = 0.028m;
                decimal peakOverCharge = (usage - 1000.00m) * (peakRate);
                decimal offPeakOverCharge = (offPeakUsage - 1000.00m) * (offPeakRate);

                if ((usage <= 1000) && (offPeakUsage <= 1000))
                {
                    return (peakCharge + offPeakCharge);
                }
                else if ((usage > 1000) && (offPeakUsage < 1000))
                {
                    return ((peakCharge + offPeakCharge) + (peakOverCharge));
                }
                else if ((offPeakUsage > 1000) && (usage < 1000))
                {
                    return ((peakCharge + offPeakCharge) + (offPeakOverCharge));
                }
                else
                {
                    return ((peakCharge + offPeakCharge) + (peakOverCharge) + (offPeakOverCharge));
                }
            }
        }
    }
}
