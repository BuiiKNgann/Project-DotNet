using Guna.UI2.AnimatorNS;
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
    public partial class Employee : Form, IView
    {
        private EmployeeController controller;
        private EmployeeModel employee;
        private BindingList<EmployeeModel> employeeList; // Thêm BindingList
        public Employee()
        {
            InitializeComponent();
            controller = new EmployeeController();
            employee = new EmployeeModel();
            employeeList = new BindingList<EmployeeModel>();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Employee_Load(object sender, EventArgs e)
        {
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            try
            {
                if (controller.Load())
                {
                    var employeeData = controller.Items.Cast<EmployeeModel>().Select(employee => new
                    {
                        eid = employee.eid,
                        ename = employee.ename,
                        mobile = employee.mobile,
                        gender = employee.gender, 
                        emailid = employee.emailid,
                        role = employee.role,
                        pass = employee.pass,
                    }).ToList();

                    guna2DataGridView1.DataSource = employeeData;  

                     
                    guna2DataGridView1.Columns["eid"].HeaderText = "Mã nhân viên";
                    guna2DataGridView1.Columns["ename"].HeaderText = "Tên nhân viên";
                    guna2DataGridView1.Columns["mobile"].HeaderText = "Số điện thoại";
                    guna2DataGridView1.Columns["gender"].HeaderText = "Giới tính";  
                    guna2DataGridView1.Columns["emailid"].HeaderText = "Email";
                    guna2DataGridView1.Columns["role"].HeaderText = "role";
                    guna2DataGridView1.Columns["pass"].HeaderText = "Password";

                   
                    cmbGender.Items.Clear();
                    cmbGender.Items.Add("Nam");
                    cmbGender.Items.Add("Nu");
                     

                    
                    if (employeeData.Any())
                    {
                        cmbGender.SelectedItem = employeeData.First().gender;  
                    }
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu để hiển thị.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi tải dữ liệu: {ex.Message}");
            }

        }

        public void SetDataToText()
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                
                DataGridViewRow selectedRow = guna2DataGridView1.SelectedRows[0];

               
                employee = new EmployeeModel
                {
                    eid = Convert.ToInt32(selectedRow.Cells["eid"].Value),
                    ename = selectedRow.Cells["ename"].Value.ToString(),
                    mobile = Convert.ToInt64(selectedRow.Cells["mobile"].Value),
                    gender = selectedRow.Cells["gender"].Value.ToString(),
                    emailid = selectedRow.Cells["emailid"].Value.ToString(),
                    role = selectedRow.Cells["role"].Value.ToString(),
                    pass = selectedRow.Cells["pass"].Value.ToString()
                };

            };

         
            txtName.Text = employee.ename;
            txtPhoneNo.Text = employee.mobile.ToString();
            cmbGender.SelectedItem = employee.gender;  
            txtNationnality.Text = employee.emailid;
            txtRole.Text = employee.role;
            txtPassword.Text = employee.pass;

        }

        public void GetDataFromText()
        {
            employee.ename = txtName.Text.Trim();
            employee.mobile = Convert.ToInt64(txtPhoneNo.Text.Trim());
            employee.gender = cmbGender.SelectedItem?.ToString() ?? string.Empty;  
            employee.emailid = txtNationnality.Text.Trim();                
            employee.role = txtRole.Text.Trim();                 
            employee.pass = txtPassword.Text.Trim();                     

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                SetDataToText(); 
            }
        }
        private void guna2DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                SetDataToText();  
            }

        }
        public void UpdateDataGridView()
        {
            employeeList.Clear();  

            foreach (var employee in controller.Items.Cast<EmployeeModel>())
            {
                employeeList.Add(employee);  
            }

            guna2DataGridView1.DataSource = employeeList; 
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            GetDataFromText();             
            if (employee.IsValidate())  
            {
                try
                {
                    
                    if (controller.Create(employee))
                    {
                        MessageBox.Show("Chi nhánh đã được thêm thành công!");
                       ClearForm();
                        LoadEmployees();  
                    }
                    else
                    {
                        MessageBox.Show("Có lỗi xảy ra khi thêm chi nhánh.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Có lỗi xảy ra: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Dữ liệu không hợp lệ. Vui lòng kiểm tra lại.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            GetDataFromText();

            if (employee.IsValidate())  
            {
                 
                var confirmResult = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa khách hàng này không?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

               
                if (confirmResult == DialogResult.Yes)
                {
                    try
                    {
                       
                        if (controller.Delete(employee))
                        {
                            MessageBox.Show("Khách hàng đã được xóa thành công!");
                             ClearForm();  
                            LoadEmployees();  
                        }
                        else
                        {
                            MessageBox.Show("Có lỗi xảy ra khi xóa khách hàng.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Có lỗi xảy ra: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Dữ liệu không hợp lệ. Vui lòng kiểm tra lại.");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            GetDataFromText();

            if (employee.IsValidate())  
            {
                try
                {
                    if (controller.Update(employee)) 
                    {
                        MessageBox.Show("Nhân viên đã được cập nhật thành công!");
                        ClearForm();
                        LoadEmployees(); 
                    }
                    else
                    {
                        MessageBox.Show("Có lỗi xảy ra khi cập nhật nhân viên.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Có lỗi xảy ra: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Dữ liệu không hợp lệ. Vui lòng kiểm tra lại.");
            }
        }
        private void ClearForm()
        {
            txtName.Text = string.Empty;
            txtPhoneNo.Text = string.Empty;
            cmbGender.SelectedIndex = -1;
            txtNationnality.Text = string.Empty;
            txtRole.Text = string.Empty;
            txtPassword.Text = string.Empty;
        }

        private void cmbGender_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}