using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO.Ports;
using System.IO;
using System.Threading;
using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    public partial class Menu : Form
    {
        private DataTable dt = new DataTable();
        private SerialPort port = new SerialPort();
        delegate void SetTextCallback(string text);
        private bool isSerialChoosed = false;
        private bool isComChoosed = false;
        string InputData = String.Empty;
        //delegate void SetTextCallback(string text); // Khai bao delegate SetTextCallBack voi tham so string
        //List<String> dataPort;
        public Menu()
        {
            InitializeComponent();
            DateTime dt = DateTime.Now;
            lblTime.Text = dt.ToString("ddd, dd MMM yyyy");
            ListPort();
            ListSerial();
            Connectdb();
            lblStatus.ForeColor = Color.Red;
        }
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

        private void btnRegister_Click(object sender, EventArgs e)
        {
            sign_up sign_Up = new sign_up();
            sign_Up.Owner = this;
            this.Hide();
            sign_Up.ShowDialog();
            //this.Show();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            removeUser removeUser = new removeUser();
            this.Hide();
            removeUser.Owner = this;
            removeUser.ShowDialog();
            //this.Show();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            editUser editUser = new editUser();
            this.Hide();
            editUser.Owner = this;
            editUser.ShowDialog();
            //this.Show();
        }

        private void ListPort()
        {
            string[] ports = SerialPort.GetPortNames(); //Get list COM ports
            for (int i = 0; i < ports.Length; i++)
            {
                cmbPort.Items.Add(ports[i]);
            }
        }

        private void ListSerial()
        {
            List<int> serial = new List<int>();
            serial.Add(9600);
            serial.Add(57600);
            serial.Add(115200);
            for (int i = 0; i < serial.Count; i++)
            {
                cmbSerial.Items.Add(serial[i]);
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        { 
            if (isSerialChoosed == true && isComChoosed == true)
            {
                lblStatus.ForeColor = Color.Green;
                lblStatus.Text = "CONNECTED";
                port.PortName = cmbPort.Text;
                port.BaudRate = Convert.ToInt32(cmbSerial.Text);
                port.DataReceived += new SerialDataReceivedEventHandler(DataReceive);
                port.Open();
            }
            else
            {
                MessageBox.Show("Please select COM PORT and Serial to connect");
            }
        }

        private void DataReceive(object obj, SerialDataReceivedEventArgs e)
        {
            InputData = port.ReadExisting();
            if (InputData.Contains("Found ID"))
            {
                // txtIn.Text = InputData; // Ko dùng đc như thế này vì khác threads .
                SetText(InputData); // Chính vì vậy phải sử dụng ủy quyền tại đây. Gọi delegate đã khai báo trước đó.
            }

        }
        private void SetText(string text)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            string[] textTrim = text.Split('#');
            string status = string.Empty;
            string userName = string.Empty ;
            int roomID = 0;
            int id = 0;
            foreach (char ch in textTrim[1])
            {
                id = Convert.ToInt32(ch);
                if(id > 0)
                {
                    break;
                }    
            }

            if (id > 0)
            {
                string sqlCmd = "select UR.RoomID, UR.UserName, HM.Status from UserRegister UR left join HistoryManager HM on UR.RoomID = HM.RoomID where UR.UserID like " + id + ";";
                cmd.CommandText = sqlCmd;
                cmd.Connection = sqlCon;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    roomID = Convert.ToInt32(reader["RoomID"].ToString());
                    userName = reader["UserName"].ToString();
                    status = reader["Status"].ToString();
                }
                reader.Close();
                if (status == string.Empty || status.Trim(' ') == "out")
                {
                    status = "in";
                }
                else
                {
                    status = "out";
                }

                string sqlCmd1 = "INSERT INTO HistoryManager(RoomID, UserName, Time, Status) values (" + roomID + ", '" + userName + "', '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "', '" + status + "');";
                cmd.CommandText = sqlCmd1;
                cmd.ExecuteNonQuery();
            }
        }


        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            port.Close();
            lblStatus.ForeColor = Color.Red;
            lblStatus.Text = "DISCONNECTED";
            isSerialChoosed = false;
            isComChoosed = false;
            cmbSerial.Text = "";
            cmbPort.Text = "";
        }

        private void cmbSerial_SelectedIndexChanged(object sender, EventArgs e)
        {
            isSerialChoosed = true;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            Create_room room = new Create_room();
            this.Hide();
            room.Owner = this;
            room.ShowDialog();

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            View_room view = new View_room();
            view.Owner = this;
            this.Hide();
            view.ShowDialog();
        }

        private void cmbPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            isComChoosed = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RoomManager();
        }

        private void RoomManager()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;

            // Xóa các room có timeEnd < now
            String sqlCmd = "DELETE FROM RoomManager WHERE TimeEnd < '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "';";
            cmd.CommandText = sqlCmd;
            cmd.Connection = sqlCon;
            cmd.ExecuteNonQuery();

            // Update các user được đky trong room gần nhất
            String sqlCmd1 = "select top(1) *  from RoomManager order by TimeStart asc";
            cmd.CommandText = sqlCmd1;
            cmd.Connection = sqlCon;
            SqlDataReader reader = cmd.ExecuteReader();
            int roomId = 0;
            string[] ListEmployee = new string[] {};

            while (reader.Read())
            {
                roomId = Convert.ToInt32(reader["RoomID"].ToString());
                ListEmployee = reader["ListEmployee"].ToString().Split(',');
            }

            reader.Close();

            foreach (string EmployeeId in ListEmployee)
            {
                String sqlCmd2 = "UPDATE UserRegister SET RoomID = " + roomId + " WHERE UserName = '" + EmployeeId + "';";
                cmd.CommandText = sqlCmd2;
                cmd.Connection = sqlCon;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
