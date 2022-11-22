using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

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
        bool editing_image = false;
        String imageLocation = "";
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
            string sqlCmd1 = String.Empty;
            string sqlCmd2 = String.Empty;
            string sqlCmd3 = String.Empty;
            int result1 = 0;
            int result2 = 0;
            int result3 = 0;
            if (editing_name == false && editing_id == false)
            {
                MessageBox.Show("Please edit or cancel");
            }
            else
            {
                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandType = CommandType.Text;

                if (editing_name == true)
                {
                    sqlCmd1 = "UPDATE UserRegister SET UserName = '" + textBox1.Text + "' WHERE UserName = '" + this.edit_name + "';";
                    cmd1.CommandText = sqlCmd1;
                    cmd1.Connection = sqlCon;

                    result1 = cmd1.ExecuteNonQuery();
                }
                if(editing_id == true)
                {
                    sqlCmd2 = "UPDATE UserRegister SET UserID = '" + textBox2.Text + "' WHERE UserName = '" + this.edit_name + "';";
                    cmd1.CommandText = sqlCmd2;
                    cmd1.Connection = sqlCon;

                    result2 = cmd1.ExecuteNonQuery();
                }
                if(editing_image == true)
                {
                    sqlCmd3 = "UPDATE UserRegister SET ImageLocation = '" + imageLocation + "' WHERE UserName = '" + this.edit_name + "';";
                    cmd1.CommandText = sqlCmd3;
                    cmd1.Connection = sqlCon;

                    result3 = cmd1.ExecuteNonQuery();
                } 
                
                if(result1 > 0 || result2 > 0 || result3 > 0)
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
            this.Owner.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;.*.gif;)|*.jpg;*.jpeg;.*.gif";
                string workingDirectory = Environment.CurrentDirectory;
                openFileDialog.InitialDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName + "\\MeetingBooking\\image";

                if (DialogResult.OK == openFileDialog.ShowDialog())
                {
                    imageLocation = openFileDialog.FileName;
                    picUser.ImageLocation = imageLocation;
                    editing_image = true;
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Have error to load image employee ");
            }
        }
    }
}
