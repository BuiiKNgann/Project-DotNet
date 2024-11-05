using QLKS.Controller;
using QLKS.Model;
using QLKS.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

namespace QLKS
{
    public partial class CheckOut : Form, IView
    {
        private CustomerController customerController;
        private BindingList<CustomerModel> customerRoomDetails;

        public CheckOut()
        {
            InitializeComponent();
            customerController = new CustomerController();
            loadCheckOut();

             
            txtCheckOut.CustomFormat = "dd/MM/yyyy";
            txtCheckOut.Format = System.Windows.Forms.DateTimePickerFormat.Custom;

             
            txtCheckOut.ValueChanged += TxtCheckOut_ValueChanged;

             
            guna2DataGridView1.SelectionChanged += guna2DataGridView1_SelectionChanged;
            guna2DataGridView1.CellFormatting += guna2DataGridView1_CellFormatting;
        }

        private void TxtCheckOut_ValueChanged(object sender, EventArgs e)
        {
            // Xử lý logic nếu cần khi ngày tháng thay đổi
        }

        private void guna2DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = guna2DataGridView1.SelectedRows[0];

                txtCName.Text = selectedRow.Cells["cname"].Value?.ToString() ?? string.Empty;
                txtRoom.Text = selectedRow.Cells["roomid"].Value?.ToString() ?? string.Empty;

                 
                if (selectedRow.Cells["checkout"].Value != null && selectedRow.Cells["checkout"].Value != DBNull.Value)
                {
                    DateTime dateOpened;
                    
                    if (selectedRow.Cells["checkout"].Value is DateTime date && date > DateTime.MinValue)
                    {
                        txtCheckOut.Value = date;
                    }
                    else
                    {
                      
                        string dateStr = selectedRow.Cells["checkout"].Value.ToString();
                        if (DateTime.TryParseExact(dateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOpened) && dateOpened > DateTime.MinValue)
                        {
                            txtCheckOut.Value = dateOpened;
                        }
                        else
                        {
                            MessageBox.Show("Định dạng ngày tháng không hợp lệ hoặc ngày checkout không hợp lệ. Hãy kiểm tra lại.");
                            txtCheckOut.Value = DateTime.Now;
                        }
                    }
                }
                else
                {
                    txtCheckOut.Value = DateTime.Now;
                }
            }
        }

        private void loadCheckOut()
        {
            var customerRoomDetailsList = customerController.GetCustomerRoomDetails();

            if (customerRoomDetailsList == null || customerRoomDetailsList.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu nào để hiển thị.");
                return;
            }

            List<CustomerModel> customerModels = new List<CustomerModel>();
            foreach (var item in customerRoomDetailsList)
            {
                customerModels.Add(new CustomerModel
                {
                    cid = item.cid,
                    cname = item.cname,
                    mobile = item.mobile,
                    nationality = item.nationality,
                    gender = item.gender,
                    dob = item.dob,
                    idproof = item.idproof,
                    address = item.address,
                    checkin = item.checkin,
                    checkout = item.checkout,
                    roomid = item.roomid,
                });
            }

            customerRoomDetails = new BindingList<CustomerModel>(customerModels);
            guna2DataGridView1.DataSource = customerRoomDetails;

            
            guna2DataGridView1.Columns["checkout"].DefaultCellStyle.Format = "dd/MM/yyyy";
            guna2DataGridView1.Columns["checkin"].DefaultCellStyle.Format = "dd/MM/yyyy";
            guna2DataGridView1.Columns["dob"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        public void SetDataToText()
        {
            throw new NotImplementedException();
        }

        public void GetDataFromText()
        {
            throw new NotImplementedException();
        }
         
        private void btn_CheckOut_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = guna2DataGridView1.SelectedRows[0];

                int cid = Convert.ToInt32(selectedRow.Cells["cid"].Value);
                string cname = selectedRow.Cells["cname"].Value.ToString();
                long mobile = Convert.ToInt64(selectedRow.Cells["mobile"].Value);
                string nationality = selectedRow.Cells["nationality"].Value.ToString();
                string gender = selectedRow.Cells["gender"].Value.ToString();
                DateTime dob = Convert.ToDateTime(selectedRow.Cells["dob"].Value);
                string idproof = selectedRow.Cells["idproof"].Value.ToString();
                string address = selectedRow.Cells["address"].Value.ToString();
                DateTime checkin = Convert.ToDateTime(selectedRow.Cells["checkin"].Value);
                DateTime? checkout = selectedRow.Cells["checkout"].Value as DateTime?;
                int roomid = Convert.ToInt32(selectedRow.Cells["roomid"].Value);

                if (txtCheckOut.Value.Date < DateTime.Now.Date)
                {
                    MessageBox.Show("Ngày checkout không hợp lệ. Vui lòng chọn ngày sau hoặc bằng ngày hiện tại.");
                    return;
                }

                try
                {
                    bool saveSuccess = customerController.SaveToAllCustomerDetails(new CustomerModel
                    {
                        cid = cid,
                        cname = cname,
                        mobile = mobile,
                        nationality = nationality,
                        gender = gender,
                        dob = dob,
                        idproof = idproof,
                        address = address,
                        checkin = checkin,
                        checkout = txtCheckOut.Value.Date,
                        roomid = roomid
                    });

                    if (saveSuccess)
                    {
                        bool deleteSuccess = customerController.DeleteCustomerFromCurrentTable(cid);

                        if (deleteSuccess)
                        {
                            MessageBox.Show("Checkout thành công và đã xóa dữ liệu khỏi cơ sở dữ liệu!");
                            customerRoomDetails.RemoveAt(selectedRow.Index);  
                        }
                        else
                        {
                            MessageBox.Show("Checkout thành công nhưng xóa dữ liệu trong cơ sở dữ liệu không thành công.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Checkout không thành công.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
                }
            }
        }


        private void guna2DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "checkout" && e.Value != null)
            {
                if (e.Value is DateTime dateValue)
                {
                    e.Value = dateValue.ToString("dd/MM/yyyy");
                    e.FormattingApplied = true;
                }
            }
            else if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "checkin" && e.Value != null)
            {
                if (e.Value is DateTime dateValue)
                {
                    e.Value = dateValue.ToString("dd/MM/yyyy");
                    e.FormattingApplied = true;
                }
            }
            else if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "dob" && e.Value != null)
            {
                if (e.Value is DateTime dateValue)
                {
                    e.Value = dateValue.ToString("dd/MM/yyyy");
                    e.FormattingApplied = true;
                }
            }
        }

        private void CheckOut_Load(object sender, EventArgs e)
        {
            // Xử lý khi form Load nếu cần thiết
        }
    }
}
