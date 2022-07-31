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
    public partial class editUser : Form
    {
        public editUser()
        {
            InitializeComponent();
            Connectdb();
        }

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

        private void btnExit_Click(object sender, EventArgs e)
        {
            //xử lý sự kiện cho button thoát
            Application.Exit();
        }

        private void btnSeach_Click(object sender, EventArgs e)
        {
            // gọi hàm show các nhân viên
            ShowUser();
        }

        //viết hàm hiển thị danh sách học sinh
        private void ShowUser()
        {
            //khởi tạo các đối tượng SqlDataAdapter, DataTable
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();

            try
            {
                string sqlCmd = "SELECT UserID, UserName FROM UserRegister WHERE UserName LIKE '%" + txtSearch.Text + "%' ;";
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.CommandText = sqlCmd;
                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.Connection = sqlCon;
                da.Fill(dt);
                dtgDSHS.DataSource = dt;
                //set up độ dài và tiêu đề cho các cột
                dtgDSHS.Columns[0].Width = 100;
                dtgDSHS.Columns[0].HeaderText = "UserID";
                dtgDSHS.Columns[1].Width = 150;
                dtgDSHS.Columns[1].HeaderText = "UserName";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            editUserDetail editUserDetail = new editUserDetail();
            this.Hide();
            editUserDetail.ShowDialog();
            this.Show();
        }
    }
}

