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
    public partial class DineIn : Form
    {
        private string connectionString = "Data Source=LAPTOP-TV64G129\\SQLEXPRESS;Initial Catalog=pansitBilao;Integrated Security=True";

        public DineIn()
        {
            InitializeComponent();
            LoadMenu();
        }
        public void UpdateStatus(string newText)
        {
            status.Text = newText;
        }

        public void AddDataToDataGridView(string data1, string data2, string data3, string data4)
        {
            itemTable.Rows.Add(data1, data2, data3, data4);
            decimal total1 = 0;

            // Replace 2 with the actual index of the column you want to total
            int columnIndex = 3;

            foreach (DataGridViewRow row in itemTable.Rows)
            {
                if (row.Cells[columnIndex].Value != null)
                {
                    if (decimal.TryParse(row.Cells[columnIndex].Value.ToString(), out decimal cellValue) && int.TryParse(row.Cells[2].Value.ToString(), out int quantity))
                    {
                        total1 += cellValue * quantity;
                    }
                }
            }

            total.Text = total1.ToString();

        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            AddForm add = new AddForm(this);
            add.Show();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            itemTable.Rows.Clear();
            total.Text = "";
        }

        public int LoadCustID()
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Customer", connection))
                {
                    int custId = 0;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int cust = reader.GetInt32(0);
                            custId = cust;
                        }
                    }
                    connection.Close();

                    return custId;
                }
            }
        }

        public void insertCustomer()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("InsertCustomerWithGeneratedName", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public void LoadMenu()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Item", connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader); // Load data from the reader into the DataTable

                        dataGridView1.DataSource = dataTable; // Set the DataTable as the DataSource
                    }
                }
                connection.Close();
            }
        }

        private void proceedBtn_Click(object sender, EventArgs e)
        {
            insertCustomer();
            int custId = LoadCustID();
            string itemNo = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("InsertCustomerAndOrder", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    foreach (DataGridViewRow row in itemTable.Rows)
                    {
                        command.Parameters.Clear();
                        itemNo = row.Cells["itemNo"].Value.ToString();
                        command.Parameters.AddWithValue("@itemNo", row.Cells["itemNo"].Value);
                        command.Parameters.AddWithValue("@quantity", row.Cells["quantity"].Value);
                        command.Parameters.AddWithValue("@price", row.Cells["price"].Value);
                        command.Parameters.AddWithValue("@cash", cash.Text);
                        command.Parameters.AddWithValue("@custId", custId);
                        command.Parameters.AddWithValue("@employee", 1);

                        int rowsAffected = command.ExecuteNonQuery();

                    }
                }
            }

            decimal.TryParse(cash.Text, out decimal cashAmount);
            COForm cOForm = new COForm();
            
            cOForm.SetCustID(custId);
            cOForm.SetItemNo(itemNo);
            cOForm.SetCash(cashAmount);
            this.Close();
            cOForm.Show();
        }
    }
}
