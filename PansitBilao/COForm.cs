using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PansitBilao
{
    public partial class COForm : Form
    {
        private string connectionString = "Data Source=LAPTOP-TV64G129\\SQLEXPRESS;Initial Catalog=pansitBilao;Integrated Security=True";
        public COForm()
        {
            InitializeComponent();
        }

        public void SetCash(decimal cash)
        {
            cashLabel.Text = cash.ToString();
        }

        public void SetItemNo(string itemNo)
        {
            itemNoLabel.Text = itemNo.ToString();
        }

        public void SetCustID(int custId)
        {
            label6.Text = custId.ToString();
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            this.Close();
            DineIn main = new DineIn();
            main.Show();
        }

        private void COForm_Load(object sender, EventArgs e)
        {
            DataTable orderTable = new DataTable();
            dataGridView1.Rows.Clear();
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM GetOrderDetails(@custId)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Assuming itemNoLabel.Text is the item number as a string
                    // Convert it to int before passing it as a parameter
                    command.Parameters.AddWithValue("@custId", int.Parse(label6.Text));

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(orderTable);
                    }
                }
            }

            dataGridView1.DataSource = orderTable;

            decimal totalAmount = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["TotalAmount"].Value != null)
                {
                    if (decimal.TryParse(row.Cells["TotalAmount"].Value.ToString(), out decimal rowTotal))
                    {
                        totalAmount += rowTotal;
                    }
                }
            }

            if (decimal.TryParse(cashLabel.Text, out decimal cashAmount))
            {
                decimal change = cashAmount - totalAmount;
                totalLabel.Text = totalAmount.ToString();
                changeLabel.Text = change.ToString();
                dateLabel.Text = DateTime.Now.ToString();
            }

        }

        private void printBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = $"C:\\Receipts\\Receipt_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.txt";
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    writer.WriteLine("Qty, itemName, price, total amount");
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        string qty = row.Cells["Qty"].Value.ToString();
                        string itemName = row.Cells["itemName"].Value.ToString();
                        string price = row.Cells["price"].Value.ToString();
                        string totalAmount = (Convert.ToDouble(row.Cells["Qty"].Value) * Convert.ToDouble(row.Cells["price"].Value)).ToString();

                        string line = $"{qty}, {itemName}, {price}, {totalAmount}";
                        writer.WriteLine(line);
                    }
                    writer.WriteLine("Total: "+$"{totalLabel.Text}");
                    writer.WriteLine("Cash: " + $"{cashLabel.Text}");
                    writer.WriteLine("Change: " + $"{changeLabel.Text}");
                }

                MessageBox.Show("Printed Successfully!", "Done", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
