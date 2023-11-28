using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PansitBilao
{
    public partial class AddMenuForm : Form
    {
        private string connectionString = "Data Source=LAPTOP-TV64G129\\SQLEXPRESS;Initial Catalog=pansitBilao;Integrated Security=True";
        public AddMenuForm()
        {
            InitializeComponent();
            LoadMenu();
        }

        public void LoadMenu()
        {
            ItemTable.Rows.Clear();
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Item", connection))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while(reader.Read())
                    {
                        ItemTable.Rows.Add(reader[0].ToString(), reader[1].ToString(), reader[2].ToString());
                    }
                }
                connection.Close();
            }
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Item(itemName, price) VALUES (@itemName, @price)", connection))
                {
                    cmd.Parameters.AddWithValue("@itemName", itemName.Text);
                    cmd.Parameters.AddWithValue("@price", Price.Text);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data inserted successfully!");
                        LoadMenu();
                    }
                    else
                    {
                        MessageBox.Show("Failed to insert data.");
                    }
                }

            }
        }

        private void ItemTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ItemTable_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Use parameterized query to prevent SQL injection
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Item WHERE itemNo = @itemNo", connection))
                    {
                        cmd.Parameters.AddWithValue("@itemNo", textBox1.Text);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                textBox2.Text = reader.GetString(1);
                                textBox3.Text = reader.GetInt32(2).ToString();
                            }
                            else
                            {
                                // Handle the case where no records are found
                                MessageBox.Show("Item not found");
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (e.g., log the error, display an error message)
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE Item SET itemName = @itemName, price = @price WHERE itemNo = @itemNo", connection))
                {
                    cmd.Parameters.AddWithValue("@itemNo", textBox1.Text);
                    cmd.Parameters.AddWithValue("@itemName", textBox2.Text);
                    cmd.Parameters.AddWithValue("@price", textBox3.Text);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Update Successfully!");
                }
                connection.Close();
            }
            LoadMenu();
        }

        private void removeBtn_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Item WHERE itemNo = @itemNo", connection))
                {
                    cmd.Parameters.AddWithValue("@itemNo", textBox1.Text);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Delete Successfully!");
                }
                connection.Close();
            }
            LoadMenu();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
