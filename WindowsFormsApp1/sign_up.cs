using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    public partial class sign_up : Form
    {
        public sign_up()
        {
            InitializeComponent();
            Connectdb();
        }
        // connect database
        string strCon = @"Data Source=DESKTOP-GK3VBNL\SQLEXPRESS;Initial Catalog=user;Integrated Security=True";
        SqlConnection sqlCon = null;

        // global variable
        bool isData1 = false;
        bool isData2 = false;
        int userCount = 0;

        private void Connectdb()
        {
            if(sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if(sqlCon.State != ConnectionState.Open)
            {
                sqlCon.Open(); // open database if state == close
            }
        }

        private void sign_in_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userName = "";
            string password = "";
            if (isData1 == true && isData2 == true)
            {
                userName = textBox1.Text;
                password = textBox2.Text;
                userCount++;
                textBox1.Text = String.Empty;
                textBox2.Text = String.Empty;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                string sqlCmd = "INSERT INTO UserRegister (UserID, UserName, Password) VALUES ('" + userCount +"'" + ",' "+ userName + "',' "+ password + "');";
                cmd.CommandText = sqlCmd;
                cmd.Connection = sqlCon;

                int n = cmd.ExecuteNonQuery();
                if(n == 0)
                {
                    MessageBox.Show("Failed to insert db !!!");
                }
                else
                {
                    MessageBox.Show("Create user successfully ");
                    sign_in sign_In = new sign_in();
                    this.Hide();
                    sign_In.ShowDialog();
                    this.Show();
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            isData1 = true;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            isData2 = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sign_in sign_In = new sign_in();
            this.Hide();
            sign_In.ShowDialog();
            this.Show();
        }

        private void sign_up_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you want to quit?", "Notification", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }
    }
}
