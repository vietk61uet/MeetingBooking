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
    public partial class sign_in : Form
    {
        public sign_in()
        {
            InitializeComponent();
            Connectdb();
        }

        // global variable
        bool isData1 = false;
        bool isData2 = false;

        // connect database
        string strCon = @"Data Source=DESKTOP-GK3VBNL\SQLEXPRESS;Initial Catalog=user;Integrated Security=True";
        SqlConnection sqlCon = null;

        private void Connectdb()
        {
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State != ConnectionState.Open)
            {
                sqlCon.Open(); // open database if state == close
            }
        }
        private void btnSignUp_Click(object sender, EventArgs e)
        {
            sign_up sign_Up = new sign_up();
            this.Hide();
            sign_Up.ShowDialog();
            this.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            isData1 = true;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            isData2 = true;
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            string userName = "";
            string password = "";
            bool isUser = false;
            if (isData1 == true && isData2 == true)
            {
                userName = textBox1.Text;
                password = textBox2.Text;
                textBox1.Text = String.Empty;
                textBox2.Text = String.Empty;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                string sqlCmd = "SELECT UserName,Password FROM UserRegister;";
                cmd.CommandText = sqlCmd;
                cmd.Connection = sqlCon;

                SqlDataReader reader = cmd.ExecuteReader();
           
                while (reader.Read())
                {
                    if (String.Equals(reader["UserName"].ToString().Replace(" ", ""), userName) &&
                        String.Equals( reader["Password"].ToString().Replace(" ", ""), password)) // must remove space after select from dtb 
                    {
                        isUser = true;
                        break;
                    }
                }
                if(isUser == true)
                {
                    Menu mainMenu = new Menu(userName);
                    this.Hide();
                    mainMenu.ShowDialog();
                    this.Show();
                    sqlCon.Close();
                    reader.Close();
                }
                else
                {
                    MessageBox.Show("Incorrect user. Please check again or register");
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void sign_in_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you want to quit?", "Notification", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }
    }
}
