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
using System.Diagnostics;
using System.Configuration;

namespace Projekt_bazy
{
    public partial class ChangePassword : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        SqlConnection conn = new SqlConnection();
        public int IDucznia;
        public ChangePassword()
        {
            InitializeComponent();
        }

        private void OnPassChangeBtnClick(object sender, EventArgs e)
        {
            conn.ConnectionString = connectionString;
            conn.Open();
            string pass = textBox1.Text, newpass = textBox2.Text, check = textBox3.Text, haslo = "";
            string query = "SELECT haslo FROM [dbo].[Uczniowie] WHERE IDucznia = '" + IDucznia + "';";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                haslo = (string)reader["haslo"];
                 
            }
            reader.Close();
            if (haslo.Equals(pass) && newpass.Equals(check) && String.IsNullOrEmpty(check) != true && String.IsNullOrEmpty(newpass) != true && this.textBox2.TextLength > 4)
            {
                string query2 = "UPDATE [dbo].[Uczniowie] SET haslo = @haslo WHERE IDucznia = '" + IDucznia + "';";
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                cmd2.Parameters.AddWithValue("@haslo", newpass);
                cmd2.ExecuteScalar();
                Debug.WriteLine("Zmieniono");
                this.Close();
            }
            else
            {
                MessageBox.Show("Bledne dane");
            }
            
                
            
            
            conn.Close();
            MessageBox.Show("Hasło zostało zmienione");
            this.Close();
        }

        private void OnAnulujBtnClick(object sender, EventArgs e)
        {
            
            this.Close();
        }
    }
}
