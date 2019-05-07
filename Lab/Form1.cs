// Energy Consumption Calculator
// CPRG 200 Lab 2 - Ariel Contreras
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Lab
{
    public partial class Form1 : Form
    {
        List<CustomerData.Customer> AllCustomers = new List<CustomerData.Customer>();
        string path = @"C:\Users\student\Desktop\ArielContreras_CPRG200_Lab2\Lab\Lab\bin\Debug\accountinfo.txt";

        // Form initialization with included hidden elements
        public Form1()
        {
            InitializeComponent();
            radioDashboard.Checked = true;
            offPeakGroup.Visible = false;
            panelDashboard.Visible = true;

            // Create a file to write to if file doesn't exist
            if (!File.Exists(path))
                using (StreamWriter sw = File.CreateText(path))
                {
                }

            // Open the file to read from & Check if file is empty
            FileInfo f = new FileInfo(path);
            if (f.Length > 0)
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    using (StreamReader r = new StreamReader(path))
                    {
                        string line;
                        while ((line = r.ReadLine()) != null)
                        {
                            string[] accountInfo = line.Split(',');
                            int account = Convert.ToInt32(accountInfo[0]);
                            decimal chargeAmount = Convert.ToDecimal(accountInfo[3]);

                            CustomerData.Customer customer = new CustomerData.Customer(account, accountInfo[1], accountInfo[2], chargeAmount);
                            listBox1.Items.Add(line);
                            AllCustomers.Add(customer);
                            Calculate_Customer_Statistics();
                        }
                    }
                }
            }
        }

        // Method for displaying customer statistics on home page
        private void Calculate_Customer_Statistics()
        {
            decimal totalResidential = AllCustomers.Where(item => item.CustomerType == "R").Sum(item => item.ChargeAmount);
            decimal totalCommercial = AllCustomers.Where(item => item.CustomerType == "C").Sum(item => item.ChargeAmount);
            decimal totalIndustrial = AllCustomers.Where(item => item.CustomerType == "I").Sum(item => item.ChargeAmount);
            decimal totalCharges = AllCustomers.Sum(item => item.ChargeAmount);

            customerCount.Text = listBox1.Items.Count.ToString();
            lblTotalCharges.Text = Convert.ToString(totalCharges);
            lblTotalResidential.Text = Convert.ToString(totalResidential);
            lblTotalCommercial.Text = Convert.ToString(totalCommercial);
            lblTotalIndustrial.Text = Convert.ToString(totalIndustrial);
        }

        //Contains all methods for performing math functions to generate power bill
        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            // Create Local Customer Object 
            CustomerData.Customer customer = new CustomerData.Customer();
            
            // Verify Customer Name Is Not Empty
            if(string.IsNullOrWhiteSpace(custNameInput.Text))
            {
                MessageBox.Show("Please Enter A Name");
                return;
            }    
            string name = custNameInput.Text;

            // Verify Account Number Is Valid
            if (!int.TryParse(accountInput.Text, out int parsedValue))
            {
                MessageBox.Show("Please Enter A Valid Account Number, Numbers Only");
                return;
            }
            int account = Convert.ToInt32(accountInput.Text);

            // Calculate Charge Based On Customer Type
            if (radioResidential.Checked == true)
            {
                customer.CustomerType = "R";
                var usage = Check_Number(usageInput.Text);
                if (usage > -1)
                {
                    lblAccountName.Text = custNameInput.Text;
                    lblAccountNumber.Text = accountInput.Text;
                    offPeakUsage.Text = " ";
                    owingOutput.Text = Convert.ToString(Math.Round(customer.calculateCharge(usage, 0), 2));
                }
                else
                {
                    return;
                }
            }

            else if (radioCommercial.Checked == true)
            {
                customer.CustomerType = "C";
                var usage = Check_Number(usageInput.Text);
                if (usage > -1)
                {
                    lblAccountName.Text = custNameInput.Text;
                    lblAccountNumber.Text = accountInput.Text;
                    offPeakUsage.Text = " ";
                    owingOutput.Text = Convert.ToString(Math.Round(customer.calculateCharge(usage, 0), 2));
                }
                else
                {
                    return;
                }
            }

            else if(radioIndustrial.Checked == true)
            {
                customer.CustomerType = "I";
                var peakUse = Check_Number(usageInput.Text);
                var offPeakUse = Check_Multiple_Numbers(offPeakUsage.Text);
                if (peakUse > -1 && offPeakUse > -1)
                {
                    lblAccountName.Text = custNameInput.Text;
                    lblAccountNumber.Text = accountInput.Text;
                    owingOutput.Text = Convert.ToString(Math.Round(customer.calculateCharge(peakUse, offPeakUse), 2));
                }
                else
                {
                    return;
                }
            }

            decimal charge = Math.Round(Convert.ToDecimal(owingOutput.Text), 2);
            customer = new CustomerData.Customer(account, name, customer.CustomerType, charge);
            AllCustomers.Add(customer);

            // Verify There Is Input Before Adding Customer To ListBox & Textfile
            if ((!String.IsNullOrEmpty(usageInput.Text)) && (!String.IsNullOrEmpty(offPeakUsage.Text)))
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    listBox1.Items.Add(customer.displayToString());
                    sw.WriteLine(customer.displayToString());
                    Calculate_Customer_Statistics();
                }
            }
        }

        // Verify number input for single input
        private decimal Check_Number(string input)
        {
            if (!int.TryParse(usageInput.Text, out int usage))
            {
                MessageBox.Show("Please Enter A Number");
                return -1;
            }
            else
            {
                if (usage <= 0)
                {
                    MessageBox.Show("Numbers Greater Then 0");
                    return -1;
                }
                else
                {
                    return Convert.ToDecimal(input);
                }
            }
        }
        // Verify number input for multiple inputs
        private decimal Check_Multiple_Numbers(string input)
        {
            if (!int.TryParse(usageInput.Text, out int usage))
            {
                return -1;
            }

            else if (!int.TryParse(offPeakUsage.Text, out usage))
            {
                MessageBox.Show("Numbers Greater Then 0 In Both Boxes");
                return -1;
            }
            else
            {
                if (usage <= 0)
                {
                    MessageBox.Show("Numbers Greater Then 0");
                    return -1;
                }
                else
                {
                    return Convert.ToDecimal(input);
                }
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            custNameInput.Text = "";
            accountInput.Text = "";
            usageInput.Text = "";
            offPeakUsage.Text = "";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void RadioResidential_CheckedChanged(object sender, EventArgs e)
        {
            panelDashboard.Visible = false;
            offPeakGroup.Visible = false;
            custTypeLabel.Text = "Residential Customer";
            lblTop.Text= "Residential Customer";
            lblKwh.Text = "Enter kWh Used :";
        }

        private void RadioCommercial_CheckedChanged(object sender, EventArgs e)
        {
            panelDashboard.Visible = false;
            offPeakGroup.Visible = false;
            custTypeLabel.Text = "Commercial Customer";
            lblTop.Text = "Commercial Customer";
            lblKwh.Text = "Enter kWh Used :";
        }

        private void RadioIndustrial_CheckedChanged_1(object sender, EventArgs e)
        {
            panelDashboard.Visible = false;
            offPeakGroup.Visible = true;
            custTypeLabel.Text = "Industrial Customer";
            lblTop.Text = "Industrial Customer";
            lblKwh.Text = "Enter Peak kWh Used :";
        }

        private void Panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void RadioDashboard_CheckedChanged(object sender, EventArgs e)
        {
            panelDashboard.Visible = true;
            lblTop.Text = "Dashboard";
        }

        private void BtnCustomerList_Click(object sender, EventArgs e)
        {
            if (listBox1.Visible == true)
                listBox1.Visible = false;
            else
                listBox1.Visible = true;
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
