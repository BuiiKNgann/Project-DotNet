using QLKS.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QLKS.Controller
{
    internal class CustomerController : IController
    {
        readonly string connectionString = "server=NGANBUI2003; Initial Catalog=Hotel_MANAGER1; Integrated Security=True; TrustServerCertificate=True;";
        List<IModel> customers = new List<IModel>();

        public List<IModel> Items => customers;

        public bool Create(IModel model)
        {
            if (model is CustomerModel customer)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO customer (cname, mobile, nationality, gender, dob, idproof, address, checkin, checkout, roomid) VALUES (@cname, @mobile, @nationality, @gender, @dob, @idproof, @address, @checkin,@checkout, @roomid)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@cname", customer.cname);
                        command.Parameters.AddWithValue("@mobile", customer.mobile);
                        command.Parameters.AddWithValue("@nationality", customer.nationality);
                        command.Parameters.AddWithValue("@gender", customer.gender);
                        command.Parameters.AddWithValue("@dob", customer.dob);
                        command.Parameters.AddWithValue("@idproof", customer.idproof);
                        command.Parameters.AddWithValue("@address", customer.address);
                        command.Parameters.AddWithValue("@checkin", customer.checkin);
                        command.Parameters.AddWithValue("@checkout", (object)customer.checkout ?? DBNull.Value);
                        command.Parameters.AddWithValue("@roomid", customer.roomid);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            return false;
        }

        public bool Delete(IModel id)
        {
            return true; 
        }

        public bool IsExist(object model)
        {
            if (model is CustomerModel customer)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM customer WHERE cid = @cid";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@cid", customer.cid);
                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            return true;
        }

        public bool Load()
        {
            customers.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM customer ORDER BY cid ASC";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CustomerModel customer = new CustomerModel
                            {
                                cid = reader.GetInt32(0),
                                cname = reader.GetString(1),
                                mobile = reader.GetInt64(2),
                                nationality = reader.GetString(3),
                                gender = reader.GetString(4),
                                dob = reader.GetDateTime(5),
                                idproof = reader.GetString(6),
                                address = reader.GetString(7),
                                checkin = reader.GetDateTime(8),
                                checkout = reader.IsDBNull(9) ? (DateTime?)null : reader.GetDateTime(9),
                                roomid = reader.GetInt32(10),
                                checkoutStatus = reader.GetString(11)
                            };
                            customers.Add(customer);
                        }
                    }
                }
            }
            return customers.Count > 0;
        }

        public bool Load(object id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM customer WHERE cid = @cid";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@cid", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            CustomerModel customer = new CustomerModel
                            {
                                cid = reader.GetInt32(0),
                                cname = reader.GetString(1),
                                mobile = reader.IsDBNull(2) ? 0 : reader.GetInt64(2),
                                nationality = reader.GetString(3),
                                gender = reader.GetString(4),
                                dob = reader.GetDateTime(5),
                                idproof = reader.GetString(6),
                                address = reader.GetString(7),
                                checkin = reader.GetDateTime(8),
                                checkout = reader.IsDBNull(9) ? (DateTime?)null : reader.GetDateTime(9),
                                roomid = reader.GetInt32(10),
                                checkoutStatus = reader.GetString(11)
                            };
                            customers.Clear();
                            customers.Add(customer);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public IModel Read(IModel id)
        {
            if (id is CustomerModel customer)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM customer WHERE cid = @cid";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@cid", customer.cid);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new CustomerModel
                                {
                                    cid = reader.GetInt32(0),
                                    cname = reader.GetString(1),
                                    mobile = reader.IsDBNull(2) ? 0 : reader.GetInt64(2),
                                    nationality = reader.GetString(3),
                                    gender = reader.GetString(4),
                                    dob = reader.GetDateTime(5),
                                    idproof = reader.GetString(6),
                                    address = reader.GetString(7),
                                    checkin = reader.GetDateTime(8),
                                    checkout = reader.IsDBNull(9) ? (DateTime?)null : reader.GetDateTime(9),
                                    roomid = reader.GetInt32(10),
                                    checkoutStatus = reader.GetString(11)
                                };
                            }
                        }
                    }
                }
            }
            return null;
        }

        public bool Update(IModel model)
        {
            return true;  
        }

        public List<dynamic> GetCustomerRoomDetails()
        {
            List<dynamic> customerRoomDetails = new List<dynamic>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT c.cid, c.cname, c.mobile, c.nationality, c.gender, c.dob, c.idproof, c.address, 
                           c.checkin, c.checkout, c.roomid, r.roomType, r.bed, r.price
                    FROM customer AS c
                    JOIN rooms AS r ON c.roomid = r.roomid
                    ORDER BY c.cid ASC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var customerRoom = new
                            {
                                cid = reader.GetInt32(0),
                                cname = reader.GetString(1),
                                mobile = reader.GetInt64(2),
                                nationality = reader.GetString(3),
                                gender = reader.GetString(4),
                                dob = reader.GetDateTime(5),
                                idproof = reader.GetString(6),
                                address = reader.GetString(7),
                                checkin = reader.GetDateTime(8),
                                checkout = reader.IsDBNull(9) ? (DateTime?)null : reader.GetDateTime(9),
                                roomid = reader.GetInt32(10),
                                roomType = reader.GetString(11),
                                bed = reader.GetString(12),
                                price = reader.GetInt64(13),
                            };
                            customerRoomDetails.Add(customerRoom);
                        }
                    }
                }
            }
            return customerRoomDetails;
        }

        public bool SaveToAllCustomerDetails(CustomerModel customer)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Kiểm tra xem cid đã tồn tại hay chưa
                string checkQuery = "SELECT COUNT(*) FROM AllCustomerDetails WHERE cid = @cid";
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@cid", customer.cid);
                    int count = (int)checkCommand.ExecuteScalar();
                    if (count > 0)
                    {
                        throw new InvalidOperationException("CID đã tồn tại.");
                    }
                }

                // Nếu không tồn tại, thực hiện chèn
                string query = "INSERT INTO AllCustomerDetails ( cname, mobile, nationality, gender, dob, idproof, address, checkin, checkout, roomid, checkoutStatus) " +
                               "VALUES ( @cname, @mobile, @nationality, @gender, @dob, @idproof, @address, @checkin, @checkout, @roomid, 'YES')";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@cname", customer.cname);
                    command.Parameters.AddWithValue("@mobile", customer.mobile);
                    command.Parameters.AddWithValue("@nationality", customer.nationality);
                    command.Parameters.AddWithValue("@gender", customer.gender);
                    command.Parameters.AddWithValue("@dob", customer.dob);
                    command.Parameters.AddWithValue("@idproof", customer.idproof);
                    command.Parameters.AddWithValue("@address", customer.address);
                    command.Parameters.AddWithValue("@checkin", customer.checkin);
                    command.Parameters.AddWithValue("@checkout", (object)customer.checkout ?? DBNull.Value);
                    command.Parameters.AddWithValue("@roomid", customer.roomid);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
        public List<CustomerModel> GetAllCustomerDetails()
        {
            List<CustomerModel> customers = new List<CustomerModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM AllCustomerDetails";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        customers.Add(new CustomerModel
                        {
                            cid = (int)reader["cid"],
                            cname = reader["cname"].ToString(),
                            mobile = (long)reader["mobile"],
                            nationality = reader["nationality"].ToString(),
                            gender = reader["gender"].ToString(),
                            dob = (DateTime)reader["dob"],
                            idproof = reader["idproof"].ToString(),
                            address = reader["address"].ToString(),
                            checkin = (DateTime)reader["checkin"],
                            checkout = reader["checkout"] != DBNull.Value ? (DateTime?)reader["checkout"] : null,
                            roomid = (int)reader["roomid"],
                            checkoutStatus = reader["checkoutStatus"].ToString(),
                        });
                    }
                }
            }

            return customers;
        }
        public bool DeleteCustomerFromCurrentTable(int customerId)
        {
            try
            {
                using (var connection = new SqlConnection("server=NGANBUI2003; Initial Catalog=Hotel_MANAGER1; Integrated Security=True; TrustServerCertificate=True;"))
                {
                    connection.Open();
                    string query = "DELETE FROM customer WHERE cid = @customerId";  

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@customerId", customerId);
                        int result = command.ExecuteNonQuery();

                        // Trả về true nếu xóa thành công
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa dữ liệu từ cơ sở dữ liệu: " + ex.Message);
                return false;
            }
        }
        public List<CustomerModel> GetCurrentCustomers()
        {
            List<CustomerModel> currentCustomers = new List<CustomerModel>();

            string query = "SELECT * FROM customer WHERE checkout IS NULL";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    currentCustomers.Add(new CustomerModel
                    {
                        cid = Convert.ToInt32(reader["cid"]),
                        cname = reader["cname"].ToString(),
                        mobile = Convert.ToInt64(reader["mobile"]),
                        nationality = reader["nationality"].ToString(),
                        gender = reader["gender"].ToString(),
                        dob = Convert.ToDateTime(reader["dob"]),
                        idproof = reader["idproof"].ToString(),
                        address = reader["address"].ToString(),
                        checkin = Convert.ToDateTime(reader["checkin"]),
                        checkout = reader["checkout"] as DateTime?,
                        roomid = Convert.ToInt32(reader["roomid"]),
                        checkoutStatus = reader["checkoutStatus"].ToString()
                    });
                }
                conn.Close();
            }
            return currentCustomers;
        }
    }
}
