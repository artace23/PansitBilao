using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PansitBilao
{
    public partial class AddForm : Form
    {
        private DineIn dineForm;
        public AddForm(DineIn dine)
        {
            InitializeComponent();
            dineForm = dine;
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            string data1 = itemNo.Text; 
            string data2 = orderName.Text;
            string data3 = qty.Text;
            string data4 = price.Text;

            dineForm.AddDataToDataGridView(data1, data2, data3, data4);
            this.Hide();
        }
    }
}
