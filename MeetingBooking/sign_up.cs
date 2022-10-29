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
    public partial class sign_up : Form
    {
        public sign_up()
        {
            InitializeComponent();
            Connectdb();
        }
        // connect database
        //string strCon = @"Data Source=DESKTOP-GK3VBNL\SQLEXPRESS;Initial Catalog=user;Integrated Security=True";
        string strCon = Properties.Settings.Default.userConnectionString;
        SqlConnection sqlCon = null;

        // global variable
        bool isData1 = false;
        bool isData2 = false;
        int userCount = 0;
        String imageLocation = "";

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
            string ID = "";
            if (isData1 == true && isData2 == true)
            {
                userName = textBox1.Text;
                ID = textBox2.Text;
                userCount++;
                textBox1.Text = String.Empty;
                textBox2.Text = String.Empty;
                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandType = CommandType.Text;
                string sqlCmd = "INSERT INTO UserRegister (UserID, UserName, ImageLocation) VALUES ('" + ID +"'" + ",' "+ userName  + "',' "+ imageLocation + "');";
                cmd1.CommandText = sqlCmd;
                cmd1.Connection = sqlCon;

                int n = cmd1.ExecuteNonQuery();
                if(n == 0)
                {
                    MessageBox.Show("Failed to create user !!!");
                }
                else
                {
                    MessageBox.Show("Create user successfully ");
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
            this.Owner.Show();
            this.Close();
        }

        private void sign_up_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you want to quit?", "Notification", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
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
                    image1.ImageLocation = imageLocation;
                }    

            }
            catch(Exception)
            {
                MessageBox.Show("Have error to load image employee ");
            }
        }
    }
}
