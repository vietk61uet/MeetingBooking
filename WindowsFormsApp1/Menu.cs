using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            sign_up sign_Up = new sign_up();
            this.Hide();
            sign_Up.ShowDialog();
            this.Show();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            removeUser removeUser = new removeUser();
            this.Hide();
            removeUser.ShowDialog();
            this.Show();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            editUser editUser = new editUser();
            this.Close();
            editUser.ShowDialog();
            this.Show();
        }
    }
}
