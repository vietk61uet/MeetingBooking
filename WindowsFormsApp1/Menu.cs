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
    struct Pizza
    {
        public string type;
        public int amount;
        public int money;
        public int price;
    };
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
            initObj();
        }

        // connect database
        string strCon = @"Data Source=DESKTOP-GK3VBNL\SQLEXPRESS;Initial Catalog=Order;Integrated Security=True";
        SqlConnection sqlCon = null;
        Pizza piz1;
        Pizza piz2;
        Pizza pizmix;
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        void initObj()
        {
            piz1.type = "Pizza1";
            piz2.type = "Pizza2";
            pizmix.type = "PizzaMix";

            piz1.amount = 0;
            piz2.amount = 0;
            pizmix.amount = 0;

            piz1.price = 10;
            piz2.price = 15;
            pizmix.price = 20;
        }
        private void QueryPizza()
        {
            sqlCon = new SqlConnection(strCon);
            SqlCommand cmd = new SqlCommand();
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State != ConnectionState.Open)
            {
                sqlCon.Open(); // open database if state == close
            }
            cmd.CommandType = CommandType.Text;
            string sqlCmd = "SELECT * FROM PizzaOrder;";
            cmd.CommandText = sqlCmd;
            cmd.Connection = sqlCon;

            SqlDataReader reader1;
            reader1 = cmd.ExecuteReader();
            if (reader1.Read())
            {
                lblTotal.Text = (piz1.money + piz2.money + pizmix.money).ToString();
            }
            else
            {
                MessageBox.Show("Cannot read database");
            }

        }
        private void add(ref Pizza piz)
        {
            sqlCon = new SqlConnection(strCon);
            SqlCommand cmd = new SqlCommand();
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State != ConnectionState.Open)
            {
                sqlCon.Open(); // open database if state == close
            }
            cmd.CommandType = CommandType.Text;
            string sqlCmd = " ";
            if (piz.amount == 0)
            {
                piz.amount++;
                piz.money = piz.amount * piz.price;
                sqlCmd = "INSERT INTO PizzaOrder (PizzaType, Amount, Price, Money) VALUES ( '" + piz.type + "', '" + piz.amount + "', '" + piz.price + "', '" + piz.money + "');";
            }
            else
            {
                piz.amount++;
                piz.money = piz.amount * piz.price;
                sqlCmd = "UPDATE PizzaOrder SET Amount = '" + piz.amount + "', Money = '" + piz.money + "' WHERE PizzaType = '" + piz.type + "';";
            }
            cmd.CommandText = sqlCmd;
            cmd.Connection = sqlCon;

            int n = cmd.ExecuteNonQuery();
            if (n == 0)
            {
                MessageBox.Show("Failed to insert db !!!");
            }
            if (piz.type == piz1.type)
            {
                lblPizz1_amount.Text = piz.amount.ToString();
                lblMoneyPizza1.Text = piz.money.ToString();
            }
            else if (piz.type == piz2.type)
            {
                lblPizz2_amount.Text = piz.amount.ToString();
                lblMoneyPizza2.Text = piz.money.ToString();
            }
            else if (piz.type == pizmix.type)
            {
                lblPizzMix_amount.Text = piz.amount.ToString();
                lblMoneyPizzaMix.Text = piz.money.ToString();
            }
        }

        private void sub(ref Pizza piz)
        {
            sqlCon = new SqlConnection(strCon);
            SqlCommand cmd = new SqlCommand();
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State != ConnectionState.Open)
            {
                sqlCon.Open(); // open database if state == close
            }
            cmd.CommandType = CommandType.Text;
            string sqlCmd = " ";
            if (piz.amount > 0)
            {
                piz.amount--;
                piz.money = piz.amount * piz.price;
                sqlCmd = "UPDATE PizzaOrder SET Amount = '" + piz.amount + "', Money = '" + piz.money + "' WHERE PizzaType = '" + piz.type + "';";

                cmd.CommandText = sqlCmd;
                cmd.Connection = sqlCon;

                int n = cmd.ExecuteNonQuery();
                if (n == 0)
                {
                    MessageBox.Show("Failed to insert db !!!");
                }
                if (piz.type == piz1.type)
                {
                    lblPizz1_amount.Text = piz.amount.ToString();
                    lblMoneyPizza1.Text = piz.money.ToString();
                }
                else if (piz.type == piz2.type)
                {
                    lblPizz2_amount.Text = piz.amount.ToString();
                    lblMoneyPizza2.Text = piz.money.ToString();
                }
                else if (piz.type == pizmix.type)
                {
                    lblPizzMix_amount.Text = piz.amount.ToString();
                    lblMoneyPizzaMix.Text = piz.money.ToString();
                }
            }
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            QueryPizza();
        }

        private void btnPizza1Add_Click(object sender, EventArgs e)
        {
            add(ref piz1);
        }

        private void btnPizza1Sub_Click(object sender, EventArgs e)
        {
            sub(ref piz1);
        }

        private void btnPizza2Add_Click(object sender, EventArgs e)
        {
            add(ref piz2);
        }

        private void btnPizza2Sub_Click(object sender, EventArgs e)
        {
            sub(ref piz2);
        }

        private void btnPizzaMixAdd_Click(object sender, EventArgs e)
        {
            add(ref pizmix);
        }

        private void btnPizzaMixSub_Click(object sender, EventArgs e)
        {
            sub(ref pizmix);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblClock.Text = DateTime.Now.ToString();
        }
    }
}
