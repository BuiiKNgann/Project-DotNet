using QLKS.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace QLKS
{
    internal class UserAuthentication
    {
        readonly string connectionString = "server=NGANBUI2003; Initial Catalog=Hotel_MANAGER1; Integrated Security=True; TrustServerCertificate=True;";

        public EmployeeModel AuthenticateUser(string email, string password)
        {
            EmployeeModel employee = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM employee WHERE emailid = @Email AND pass = @Password";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            employee = new EmployeeModel
                            {
                                eid = (int)reader["eid"],
                                ename = reader["ename"].ToString(),
                                mobile = (long)reader["mobile"],
                                gender = reader["gender"].ToString(),
                                emailid = reader["emailid"].ToString(),
                                role = reader["role"].ToString(),
                                pass = reader["pass"].ToString()
                            };
                        }
                    }
                }
            }
            return employee;
        }
    }
}
