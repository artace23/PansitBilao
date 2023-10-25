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
    public partial class AdminForm : Form
    {
        private string connectionString = "Data Source=LAPTOP-TV64G129\\SQLEXPRESS;Initial Catalog=pansitBilao;Integrated Security=True";
        public AdminForm()
        {
            InitializeComponent();
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("Select * from OrderTable Where item_no = @itemNo", connection))
                {
                    command.Parameters.AddWithValue("@itemNo", searchTextBox.Text);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dataGridView1.Rows.Clear();
                        dataGridView1.Columns.Add("itemNo", "Item No.");
                        dataGridView1.Columns.Add("date", "Date");

                        while (reader.Read())
                        {
                            dataGridView1.Rows.Add(reader["item_no"], reader["Date"]);
                        }   
                    }
                }
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                    // Check the column name used for 'itemNo', it should match the actual column name.
                    string itemNo = selectedRow.Cells["itemNo"].Value.ToString();

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("SELECT * FROM OrderTable WHERE item_no = @itemNo", connection))
                        {
                            command.Parameters.AddWithValue("@itemNo", itemNo);
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                itemNoLabel.Text = reader["item_no"].ToString();
                                dateLabel.Text = reader["date"].ToString();

                                // Clear existing rows in dataGridView2 before populating.
                                dataGridView2.Rows.Clear();
                                dataGridView2.Rows.Add(reader["quantity"].ToString(), reader["itemName"].ToString(), reader["price"].ToString(), reader["amount"].ToString());

                                int total = 0;

                                foreach (DataGridViewRow row in dataGridView2.Rows)
                                {
                                    if (int.TryParse(row.Cells["total"].Value.ToString(), out int value))
                                    {
                                        total += value;
                                    }
                                }

                                totalLabel.Text = total.ToString();
                                cashLabel.Text = reader["cash"].ToString();

                                int sum = Convert.ToInt32(cashLabel.Text) - total;
                                changeLabel.Text = sum.ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }
    }
}
