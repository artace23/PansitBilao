using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PansitBilao
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            string user = username.Text;
            string pass = password.Text;

            if (user == "user" && pass == "user")
            { 
                this.Hide();
                MainForm mainForm = new MainForm();

                mainForm.Show();
            }

            else if(user == "admin" && pass == "admin")
            {
                this.Hide();
                AdminForm admin = new AdminForm();

                admin.Show();
            }

            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
                username.Text = "";
                password.Text = "";
            }
        }
    }
}
