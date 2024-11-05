using QLKS.Controller;
using QLKS.Model;
using QLKS.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLKS
{
    public partial class CustomerDetails : Form, IView
    {

        public CustomerDetails()
        {
            InitializeComponent();
        }

        public void GetDataFromText()
        {
            throw new NotImplementedException();
        }

        public void SetDataToText()
        {
            throw new NotImplementedException();
        }

        private void CustomerDetails_Load(object sender, EventArgs e)
        {
            LoadCustomerDetails();
        }

        private void LoadCustomerDetails()
        {
            var customerController = new CustomerController();
            var allCustomerDetails = customerController.GetAllCustomerDetails();

            if (allCustomerDetails == null || allCustomerDetails.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu khách hàng để hiển thị.");
                return;
            }

            guna2DataGridView1.DataSource = new BindingList<CustomerModel>(allCustomerDetails);

            // Đặt HeaderText cho các cột
            guna2DataGridView1.Columns["cid"].HeaderText = "Mã khách hàng";
            guna2DataGridView1.Columns["cname"].HeaderText = "Tên khách hàng";
            guna2DataGridView1.Columns["mobile"].HeaderText = "Số điện thoại";
            guna2DataGridView1.Columns["nationality"].HeaderText = "Quốc tịch";
            guna2DataGridView1.Columns["gender"].HeaderText = "Giới tính";
            guna2DataGridView1.Columns["dob"].HeaderText = "Ngày sinh";
            guna2DataGridView1.Columns["idproof"].HeaderText = "Giấy tờ tùy thân";
            guna2DataGridView1.Columns["address"].HeaderText = "Địa chỉ";
            guna2DataGridView1.Columns["checkin"].HeaderText = "Ngày nhận phòng";
            guna2DataGridView1.Columns["checkout"].HeaderText = "Ngày trả phòng";
            guna2DataGridView1.Columns["roomid"].HeaderText = "Mã phòng";
            guna2DataGridView1.Columns["checkoutStatus"].HeaderText = "Trạng thái trả phòng";
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
