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
    public partial class MainUczen : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        SqlConnection conn = new SqlConnection();
        public int IDucznia;
        
        public MainUczen()
        {
            InitializeComponent();
        }

        private void OnExitBtnClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OnLogoutBtnClick(object sender, EventArgs e)
        {
            LoginScreen form1 = new LoginScreen();
            this.Hide();
            form1.Show();
        }

        private void MainUczen_Load(object sender, EventArgs e)
        {
            Debug.WriteLine(IDucznia);
            conn.ConnectionString = connectionString;
            conn.Open();
            string query = "SELECT imie, nazwisko, email, telefon FROM [dbo].[Uczniowie] WHERE IDucznia = '" + IDucznia + "';";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                label8.Text = reader["Imie"].ToString();
                label9.Text = reader["nazwisko"].ToString();
                label10.Text = reader["email"].ToString();
                label11.Text = reader["telefon"].ToString();

                
            }
            reader.Close();

            string query2 = "SELECT TOP 3 Przedmioty.przedmiot, Oceny.ocena, Oceny.waga, TypOceny.typ FROM Oceny LEFT OUTER JOIN TypOceny ON Oceny.IDtyp = TypOceny.IDtyp LEFT OUTER JOIN Przedmioty ON Oceny.IDprzedmiotu = Przedmioty.IDprzedmiotu WHERE IDucznia = '" + IDucznia + "' ORDER BY IDoceny DESC;";
            SqlCommand cmd2 = new SqlCommand(query2, conn);
            SqlDataReader reader2 = cmd2.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader2);
            Oceny.DataSource = dt;
            conn.Close();
            reader.Close();
            
        }

        private void OnPassChangeBtnClick(object sender, EventArgs e)
        {
            ChangePassword form = new ChangePassword();
            form.IDucznia = IDucznia;
            form.Show();
        }

        private void OnOcenyBtnClick(object sender, EventArgs e)
        {
            OcenyUczen form = new OcenyUczen();
            form.IDucznia = IDucznia;
            form.Show();
        }
    }
}
