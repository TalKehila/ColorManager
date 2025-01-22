using System.Data.SqlClient;
using System.Configuration;
using System.Data;


namespace ColorManager.DAL
{
    public class ColorDataAccess
    {
        private string connectionString =
            ConfigurationManager.ConnectionStrings["ColorsDBConnectionString"].ConnectionString;

        public DataTable GetAllColors()
        {
            using (SqlConnection connect = new SqlConnection(connectionString))
            {                
                string query = "SELECT ID, color_name, price, display_order, is_in_stock " +
               "FROM colors ORDER BY display_order";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connect);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        public void InsertColor(string colorName, decimal price, int displayOrder, bool isInStock)
        {
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO colors (color_name, price, display_order, is_in_stock) 
                                 VALUES (@ColorName, @Price, @DisplayOrder, @IsInStock)";

                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@ColorName", colorName);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@DisplayOrder", displayOrder);
                    cmd.Parameters.AddWithValue("@IsInStock", isInStock);
                    connect.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteColor(int id)
        {
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM colors WHERE ID = @ID";
                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    connect.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateDisplayOrder(int id, int displayOrder)
        {
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                string query = "UPDATE colors SET display_order = @DisplayOrder WHERE ID = @ID";
                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@DisplayOrder", displayOrder);
                    cmd.Parameters.AddWithValue("@ID", id);
                    connect.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool IsColorExists(string colorName)
        {
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM colors WHERE color_name = @ColorName";
                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@ColorName", colorName);
                    connect.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public void UpdateColor(int id, string colorName, decimal price, int displayOrder)
        {
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                string query = @"UPDATE colors
                                 SET color_name = @ColorName,                     
                                     price      = @Price,
                                     display_order = @DisplayOrder
                                 WHERE ID = @ID";
                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@ColorName", colorName);                  
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@DisplayOrder", displayOrder);
                    cmd.Parameters.AddWithValue("@ID", id);
                    connect.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool IsDisplayOrderExists(int displayOrder, int colorId)
        {
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM colors WHERE display_order = @DisplayOrder AND ID != @ID";
                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@DisplayOrder", displayOrder);
                    cmd.Parameters.AddWithValue("@ID", colorId);
                    connect.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }


        public bool IsDisplayOrderExists(int displayOrder)
        {
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM colors WHERE display_order = @DisplayOrder";
                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@DisplayOrder", displayOrder);
                    connect.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }


        public void UpdateStockStatus(int id, bool isInStock)
        {
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                string query = "UPDATE colors SET is_in_stock = @IsInStock WHERE ID = @ID";
                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@IsInStock", isInStock);
                    cmd.Parameters.AddWithValue("@ID", id);
                    connect.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataTable GetColorById(int id)
        {
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM colors WHERE ID = @ID";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connect);
                adapter.SelectCommand.Parameters.AddWithValue("@ID", id);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }


    }
}
