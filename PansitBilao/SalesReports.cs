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
    }
}
