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
    public partial class Create_room : Form
    {
        List<String> selectedEmployee = new List<String>();
        static int RoomIndex = 0;
        public Create_room()
        {
            InitializeComponent();
            Connectdb();
            showAllEmployee();
        }

        String ListSelectedEmpyee = String.Empty;
        // connect database
        //string strCon = @"Data Source=DESKTOP-GK3VBNL\SQLEXPRESS;Initial Catalog=user;Integrated Security=True";
        SqlConnection sqlCon = null;
        string strCon = Properties.Settings.Default.userConnectionString;

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

        private void btnCreate_Click(object sender, EventArgs e)
        {
            RoomIndex++;
            if (TimeEnd.Value <= TimeStart.Value)
            {
                MessageBox.Show("Please choose Time End > Time Start");
            }
            else
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = sqlCon;

                string sqlCmd1 = "INSERT INTO RoomManager(TimeStart, TimeEnd, ListEmployee) VALUES(' " + TimeStart.Value.ToString("yyyy-MM-dd hh:mm:ss") + "', '" + TimeEnd.Value.ToString("yyyy-MM-dd hh:mm:ss") + "', '"+ ListSelectedEmpyee  + "');";
                cmd.CommandText = sqlCmd1;
                cmd.ExecuteNonQuery();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        { 
        
        }



        private void showAllEmployee()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlCon;

            string sqlCmd = "SELECT UserID, UserName FROM UserRegister";
            cmd.CommandText = sqlCmd;
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                comboBox1.Items.Add(reader["UserName"]);
            }

            reader.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            bool isExist = false;
            if(comboBox1.SelectedItem.ToString() == "")
            {

            }
            else
            {
                for(int i = 0; i < selectedEmployee.Count; i++)
                {
                    if(comboBox1.SelectedItem.ToString() == selectedEmployee.ElementAt(i))
                    {
                        isExist = true;
                        break;
                    }

                }

                if(!isExist)
                {
                    int index = 0;
                    DataTable dt = new DataTable();
                    selectedEmployee.Add(comboBox1.SelectedItem.ToString());
                    dt.Columns.Add("Index", typeof(int));
                    dt.Columns.Add("Employee Name", typeof(string));
                    foreach (string value in selectedEmployee)
                    {
                        index++;
                        DataRow row = dt.NewRow();
                        row["Index"] = index;
                        row["Employee Name"] = value;
                        dt.Rows.Add(row);
                    }
                    ListSelectedEmpyee += comboBox1.SelectedItem.ToString() +",";
                    dataGridView1.DataSource = dt;
                    dataGridView1.Columns[0].Width = 90;
                    dataGridView1.Columns[1].Width = 165;

                }

            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {

            int selectedCellCount = dataGridView1.GetCellCount(DataGridViewElementStates.Selected);
            selectedEmployee.RemoveAt(selectedCellCount - 1);

            int index = 0;
            DataTable dt = new DataTable();
            dt.Columns.Add("Index", typeof(int));
            dt.Columns.Add("Employee Name", typeof(string));
            foreach (string value in selectedEmployee)
            {
                index++;
                DataRow row = dt.NewRow();
                row["Index"] = index;
                row["Employee Name"] = value;
                dt.Rows.Add(row);
            }
            dataGridView1.DataSource = dt;
            dataGridView1.Columns[0].Width = 90;
            dataGridView1.Columns[1].Width = 165;

            dataGridView1.DataSource = dt;
            dataGridView1.Columns[0].Width = 90;
            dataGridView1.Columns[1].Width = 165;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //xử lý sự kiện cho button thoát
            this.Owner.Show();
            this.Close();
        }
    }
}
