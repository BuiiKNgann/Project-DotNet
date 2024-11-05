using QLKS.Controller;
using QLKS.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLKS.View
{
    public partial class CurrentCustomers : Form, IView
    {
        private CustomerController customerController;
        public CurrentCustomers()
        {
            InitializeComponent();
            customerController = new CustomerController();
            LoadCurrentCustomers();
        }

        public void GetDataFromText()
        {
            throw new NotImplementedException();
        }

        public void SetDataToText()
        {
            throw new NotImplementedException();
        }
        private void LoadCurrentCustomers()
        {
             
            List<CustomerModel> currentCustomers = customerController.GetCurrentCustomers();

            
            guna2DataGridView1.DataSource = currentCustomers;
 
            guna2DataGridView1.Columns["cid"].HeaderText = "Customer ID";
            guna2DataGridView1.Columns["cname"].HeaderText = "Customer Name";
            guna2DataGridView1.Columns["mobile"].HeaderText = "Mobile";
            guna2DataGridView1.Columns["nationality"].HeaderText = "Nationality";
            guna2DataGridView1.Columns["gender"].HeaderText = "Gender";
            guna2DataGridView1.Columns["dob"].HeaderText = "Date of Birth";
            guna2DataGridView1.Columns["idproof"].HeaderText = "ID Proof";
            guna2DataGridView1.Columns["address"].HeaderText = "Address";
            guna2DataGridView1.Columns["checkin"].HeaderText = "Check-In Date";
            guna2DataGridView1.Columns["checkout"].HeaderText = "Check-Out Date";
            guna2DataGridView1.Columns["roomid"].HeaderText = "Room ID";
            guna2DataGridView1.Columns["checkoutStatus"].HeaderText = "Checkout Status";

             
            guna2DataGridView1.Columns["checkin"].DefaultCellStyle.Format = "dd/MM/yyyy";
            guna2DataGridView1.Columns["dob"].DefaultCellStyle.Format = "dd/MM/yyyy";
            guna2DataGridView1.Columns["checkout"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }
        private void CurrentCustomers_Load(object sender, EventArgs e)
        {

        }
    }
}
