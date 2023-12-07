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
using System.Windows.Forms.DataVisualization.Charting;

namespace PansitBilao
{
    public partial class SalesReports : Form
    {
        private string connectionString = "Data Source=LAPTOP-TV64G129\\SQLEXPRESS;Initial Catalog=pansitBilao;Integrated Security=True";
        public SalesReports()
        {
            InitializeComponent();
            LoadSales();
            LoadYear();
        }

        public void LoadSales()
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using(SqlCommand command = new SqlCommand("SELECT CONVERT(date, date) as order_date, SUM(price * quantity) as Total\r\nFROM OrderTable\r\nGROUP BY CONVERT(date, date)\r\nORDER BY order_date;", connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Set up the Chart control
                        chartSales.Series.Clear();
                        chartSales.ChartAreas.Clear();

                        ChartArea chartArea = new ChartArea();
                        chartSales.ChartAreas.Add(chartArea);

                        Series series = new Series("Total");
                        series.ChartType = SeriesChartType.Column;
                        chartSales.Series.Add(series);

                        // Add data to the chart
                        foreach (DataRow row in dataTable.Rows)
                        {
                            DateTime orderDate = Convert.ToDateTime(row["order_date"]);
                            double total = Convert.ToDouble(row["Total"]);

                            series.Points.AddXY(orderDate.ToShortDateString(), total);
                        }
                    }
                }
            }
        }

        public void LoadSalesByDay()
        {
            object selecteditem1 = comboBox3.SelectedItem;
            object selecteditem2 = comboBox4.SelectedItem;
            object selecteditem3 = comboBox1.SelectedIndex + 1;

            int selectedDay = int.Parse(selecteditem1.ToString());
            int selectedMonth = int.Parse(selecteditem3.ToString());
            int selectedYear = int.Parse(selecteditem2.ToString());

            DateTime selectedDate = new DateTime(selectedYear, selectedMonth, selectedDay);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT CONVERT(date, date) as order_date, SUM(price * quantity) as Total\r\nFROM OrderTable\r\nWHERE CONVERT(date, date) = @date GROUP BY CONVERT(date, date)\r\nORDER BY order_date;", connection))
                {
                    command.Parameters.AddWithValue("@date",selectedDate);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Set up the Chart control
                        chartSales.Series.Clear();
                        chartSales.ChartAreas.Clear();

                        ChartArea chartArea = new ChartArea();
                        chartSales.ChartAreas.Add(chartArea);

                        Series series = new Series("Total");
                        series.ChartType = SeriesChartType.Column;
                        chartSales.Series.Add(series);

                        // Add data to the chart
                        foreach (DataRow row in dataTable.Rows)
                        {
                            DateTime orderDate = Convert.ToDateTime(row["order_date"]);
                            double total = Convert.ToDouble(row["Total"]);

                            series.Points.AddXY(orderDate.ToShortDateString(), total);

                            label2.Text = total.ToString();
                        }
                    }
                }
            }
        }


        public void LoadSalesByMonth()
        {
            object selecteditem3 = comboBox5.SelectedIndex + 1;

            int selectedMonth = int.Parse(selecteditem3.ToString());


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT FORMAT(CONVERT(date, date), 'MMMM')  as order_date, SUM(price * quantity) as Total \r\nFROM OrderTable WHERE MONTH(CONVERT(date, date)) = @date GROUP BY FORMAT(CONVERT(date, date), 'MMMM') ORDER BY order_date", connection))
                {
                    command.Parameters.AddWithValue("@date", selectedMonth);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Set up the Chart control
                        chartSales.Series.Clear();
                        chartSales.ChartAreas.Clear();

                        ChartArea chartArea = new ChartArea();
                        chartSales.ChartAreas.Add(chartArea);

                        Series series = new Series("Total");
                        series.ChartType = SeriesChartType.Column;
                        chartSales.Series.Add(series);

                        // Add data to the chart
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string orderDate = row["order_date"].ToString();
                            double total = Convert.ToDouble(row["Total"]);

                            series.Points.AddXY(orderDate, total);

                            label2.Text = total.ToString();
                        }
                    }
                }
            }
        }

        public void LoadSalesByYear()
        {
            object selecteditem3 = comboBox6.SelectedItem;

            int selectedYear = int.Parse(selecteditem3.ToString());


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT FORMAT(CONVERT(date, date), 'yyyy')  as order_date, SUM(price * quantity) as Total \r\nFROM OrderTable WHERE YEAR(CONVERT(date, date)) = @date GROUP BY FORMAT(CONVERT(date, date), 'yyyy') ORDER BY order_date", connection))
                {
                    command.Parameters.AddWithValue("@date", selectedYear);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Set up the Chart control
                        chartSales.Series.Clear();
                        chartSales.ChartAreas.Clear();

                        ChartArea chartArea = new ChartArea();
                        chartSales.ChartAreas.Add(chartArea);

                        Series series = new Series("Total");
                        series.ChartType = SeriesChartType.Column;
                        chartSales.Series.Add(series);

                        // Add data to the chart
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string orderDate = row["order_date"].ToString();
                            double total = Convert.ToDouble(row["Total"]);

                            series.Points.AddXY(orderDate, total);

                            label2.Text = total.ToString();
                        }
                    }
                }
            }
        }

        public void LoadYear()
        {
            comboBox4.Items.Clear();
            // Get the current date and time
            DateTime currentDate = DateTime.Now;

            // Extract only the year
            int currentYear = currentDate.Year;

            for (int i = 2022; i <= currentYear; i++)
            {
                comboBox4.Items.Add(i);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedMonthIndex = comboBox1.SelectedIndex;
            comboBox3.Items.Clear();

            // Check if the selected month has 31 days
            if (selectedMonthIndex == 0 || selectedMonthIndex == 2 || selectedMonthIndex == 4 ||
                selectedMonthIndex == 6 || selectedMonthIndex == 7 || selectedMonthIndex == 9 || selectedMonthIndex == 11)
            {
                // Add items to comboBox2 for months with 31 days
                for (int day = 1; day <= 31; day++)
                {
                    comboBox3.Items.Add(day);
                }
            }
            // Check if the selected month has 30 days
            else if (selectedMonthIndex == 3 || selectedMonthIndex == 5 || selectedMonthIndex == 8 || selectedMonthIndex == 10)
            {
                // Add items to comboBox2 for months with 30 days
                for (int day = 1; day <= 30; day++)
                {
                    comboBox3.Items.Add(day);
                }
            }
            // Check if the selected month is February (28 or 29 days)
            else if (selectedMonthIndex == 1)
            {
                // Add items to comboBox2 for February
                for (int day = 1; day <= 29; day++)
                {
                    comboBox3.Items.Add(day);
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox2.SelectedIndex == 0)
            {
                comboBox3.Visible = true;
                comboBox4.Visible = true;
                comboBox1.Visible = true;

                comboBox5.Visible = false;

                comboBox6.Visible = false;
            } else if (comboBox2.SelectedIndex == 1)
            {
                comboBox3.Visible = false;
                comboBox4.Visible = false;
                comboBox1.Visible = false;

                comboBox5.Visible = true;

                comboBox6.Visible = false;
            } else if (comboBox2.SelectedIndex == 2)
            {
                comboBox3.Visible = false;
                comboBox4.Visible = false;
                comboBox1.Visible = false;

                comboBox5.Visible = false;

                comboBox6.Visible = true;

                comboBox6.Items.Clear();
                // Get the current date and time
                DateTime currentDate = DateTime.Now;

                // Extract only the year
                int currentYear = currentDate.Year;

                for (int i = 2022; i <= currentYear; i++)
                {
                    comboBox6.Items.Add(i);
                }
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > 0 && comboBox3.SelectedIndex > 0)
            {
                LoadSalesByDay();
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox5.SelectedIndex > 0)
            {
                LoadSalesByMonth();
            }
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox6.SelectedIndex > 0)
            {
                LoadSalesByYear();
            }
        }

    }
}
