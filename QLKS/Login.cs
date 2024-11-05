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

namespace QLKS
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtUsername.Text;
            string password = txtPassword.Text;

            UserAuthentication auth = new UserAuthentication();
            EmployeeModel employee = auth.AuthenticateUser(email, password);

            if (employee != null)
            {
                Dashboard ds = new Dashboard(employee);
                ds.Show();
                this.Hide();
            }
            else
            {
                labelError.Text = "Email hoặc mật khẩu không đúng!";
                labelError.Visible = true;
            }
        }
    }
}
