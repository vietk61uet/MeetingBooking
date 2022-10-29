using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;

namespace WindowsFormsApp1
{
    public partial class View_room : Form
    {
        public View_room()
        {
            InitializeComponent();
            Connectdb();
            showAllRoom();
        }

        bool view = false;
        // connect database
        //string strCon = @"Data Source=DESKTOP-GK3VBNL\SQLEXPRESS;Initial Catalog=user;Integrated Security=True";
        SqlConnection sqlCon = null;
        string strCon = Properties.Settings.Default.userConnectionString;
        public delegate void AddRows(DataGridView view);

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

        private void showAllRoom()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlCon;

            string sqlCmd = "SELECT RoomID FROM RoomManager WHERE TimeStart <= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "' AND TimeEnd >= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "';";
            cmd.CommandText = sqlCmd;
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                comboBox1.Items.Add(reader["RoomID"]);
            }

            reader.Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
               view = true;
            }
            else
            {
                MessageBox.Show("Please choose room to view");
            }

        }


        private void Load()
        {
            Thread primarythrd = new Thread(primaryload);
            primarythrd.IsBackground = true;
            primarythrd.SetApartmentState(System.Threading.ApartmentState.STA);
            primarythrd.Start();
        }
        public void primaryload()
        {
            AddRows addRows = new AddRows(ViewSelectedRoom);
            this.Invoke(addRows, dataGridView1);
        }

        private void ViewSelectedRoom(DataGridView view)
        {
             
            try {
                if (view.InvokeRequired)
                {
                    AddRows rows = new AddRows(ViewSelectedRoom);
                    this.Invoke(rows, view);
                }
                else
                {
                    //while (true)
                    //{
                        //khởi tạo các đối tượng SqlDataAdapter, DataTable
                        SqlDataAdapter da = new SqlDataAdapter();
                        DataTable dt = new DataTable();

                        String roomId = comboBox1.SelectedItem.ToString();
                        string sqlCmd = "SELECT * FROM HistoryManager WHERE RoomID = " + roomId + ";";

                        da.SelectCommand = new SqlCommand();
                        da.SelectCommand.CommandText = sqlCmd;
                        da.SelectCommand.CommandType = CommandType.Text;
                        da.SelectCommand.Connection = sqlCon;
                        da.Fill(dt);
                        view.DataSource = dt;

                        //set up độ dài và tiêu đề cho các cột
                        view.Columns[0].Width = 60;
                        view.Columns[0].HeaderText = "RoomID";
                        view.Columns[1].Width = 80;
                        view.Columns[1].HeaderText = "UserName";
                        view.Columns[2].Width = 120;
                        view.Columns[2].HeaderText = "Time";
                        view.Columns[3].Width = 60;
                        view.Columns[3].HeaderText = "Status";
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            view = false;
            //xử lý sự kiện cho button thoát
            this.Owner.Show();
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (view == true)
            {
                Load();
            }
        }
    }
}
