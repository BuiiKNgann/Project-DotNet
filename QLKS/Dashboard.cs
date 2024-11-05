using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLKS.Model;
using QLKS.View;
namespace QLKS
{
    internal partial class Dashboard : Form
    {
        private EmployeeModel currentEmployee;

        // Constructor nhận đối tượng EmployeeModel
        public Dashboard(EmployeeModel employee)
        {
            InitializeComponent();
            currentEmployee = employee;
          SetupPermissions();
        }

       

        private void SetupPermissions()
        {
            // Kiểm tra vai trò của người dùng
            if (currentEmployee.role != "admin")
            {
                 
                btnAddRoom.Visible = false;  
                btnEmployee.Visible = false;  
            }
        }
        private void btnAddRoom_Click(object sender, EventArgs e)
        {
            AddRoom addRoom = new AddRoom();
            addRoom.ShowDialog();
        }

        private void btnCustomerRes_Click(object sender, EventArgs e)
        {
            CustomerRes customerRes = new CustomerRes();
            customerRes.ShowDialog();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            CheckOut checkOut = new CheckOut();
            checkOut.ShowDialog();
        }

        private void btnCustomerDetail_Click(object sender, EventArgs e)
        {
            //CustomerDetails customerDetails = new CustomerDetails();
            //customerDetails.ShowDialog();
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            Employee employee = new Employee();
            employee.ShowDialog();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {

        }

        private void customerInHotelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentCustomers CC = new CurrentCustomers();
            CC.ShowDialog();
        }

        private void khachĐaCheckoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomerDetails customerDetails = new CustomerDetails();
            customerDetails.ShowDialog();
        }
    }
}
