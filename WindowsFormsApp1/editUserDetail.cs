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
    public partial class editUserDetail : Form
    {
        private string edit_name = string.Empty;
        private string edit_id = string.Empty;
        private string edit_location = string.Empty;

        bool editing_name = false;
        bool editing_id = false;
        public editUserDetail(string name, string id, string location)
        {
            InitializeComponent();
            this.edit_name = name;
            this.edit_id = id;
            this.edit_location = location;
            initGrad();
            Connectdb();
        }

        // connect database
        //string strCon = @"Data Source=DESKTOP-GK3VBNL\SQLEXPRESS;Initial Catalog=user;Integrated Security=True";
        string strCon = Properties.Settings.Default.userConnectionString;
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
        private void initGrad()
        {
            lblUser.Text = this.edit_name;
            lblID.Text = this.edit_id;
            picUser.Image = Image.FromFile(this.edit_location);
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string sqlCmd = String.Empty;
            if (editing_name == false && editing_id == false)
            {
                MessageBox.Show("Please edit or cancel");
            }
            else
            {
                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandType = CommandType.Text;
                if (editing_name == true && editing_id == false)
                {
                    sqlCmd = "UPDATE UserRegister SET UserName = '" + textBox1.Text + "' WHERE UserName = '" + this.edit_name + "';";
                }
                else if(editing_id == true && editing_name == false)
                {
                    sqlCmd = "UPDATE UserRegister SET UserID = '" + textBox2.Text + "' WHERE UserName = '" + this.edit_name + "';";
                }
                else
                {
                    sqlCmd = "UPDATE UserRegister SET UserName = '" + textBox1.Text + "', UserID = '"+ textBox2.Text +"' WHERE UserName = '" + this.edit_name + "';";
                }    
                
                cmd1.CommandText = sqlCmd;
                cmd1.Connection = sqlCon;

                int result = cmd1.ExecuteNonQuery();
                if(result > 0)
                {
                    MessageBox.Show("Successfully");
                    Menu mainMenu = new Menu();
                    this.Hide();
                    mainMenu.ShowDialog();
                    //this.Show();
                }
                else
                {
                    MessageBox.Show("Error");
                }    

            }    
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            editing_name = true;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            editing_id = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            editUser edit_user = new editUser();
            this.Hide();
            edit_user.ShowDialog();
        }
    }
}
