using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        // Residential Calculation Test
        // Method takes two parameters for industrial, residential & commercial only use one parameter
        [TestMethod]
        public void Customer_Residential_CalculateCharge_Test1()
        {
            CustomerData.Customer c = new CustomerData.Customer();
            c.CustomerType = "R";
            decimal usage = 50;
            decimal value = c.calculateCharge(usage, 0);
            Assert.AreEqual(8.600m, value);
        }

        [TestMethod]
        public void Customer_Residential_CalculateCharge_Test2()
        {
            CustomerData.Customer c = new CustomerData.Customer();
            c.CustomerType = "R";
            decimal usage = 100;
            decimal value = c.calculateCharge(usage, 0);
            Assert.AreEqual(11.200m, value);
        }

        [TestMethod]
        public void Customer_Commercial_CalculateCharge_Test1()
        {
            CustomerData.Customer c = new CustomerData.Customer();
            c.CustomerType = "C";
            decimal usage = 1350;
            decimal value = c.calculateCharge(usage, 0);
            Assert.AreEqual(75.75000m, value);
        }

        [TestMethod]
        public void Customer_Commercial_CalculateCharge_Test2()
        {
            CustomerData.Customer c = new CustomerData.Customer();
            c.CustomerType = "C";
            decimal usage = 100;
            decimal value = c.calculateCharge(usage, 0);
            Assert.AreEqual(60.00m, value);
        }

        [TestMethod]
        public void Customer_Industrial_CalculateCharge_Test1()
        {
            CustomerData.Customer c = new CustomerData.Customer();
            c.CustomerType = "I";
            decimal peakUsage = 100;
            decimal offPeakUsage = 1200;
            decimal value = c.calculateCharge(peakUsage, offPeakUsage);
            Assert.AreEqual(121.60000m, value);
        }

        [TestMethod]
        public void Customer_Industrial_CalculateCharge_Test2()
        {
            CustomerData.Customer c = new CustomerData.Customer();
            c.CustomerType = "I";
            decimal peakUsage = 1200;
            decimal offPeakUsage = 100;
            decimal value = c.calculateCharge(peakUsage, offPeakUsage);
            Assert.AreEqual(129.00000m, value);
        }
    }
}
