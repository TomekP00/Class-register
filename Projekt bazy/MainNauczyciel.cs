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
    public partial class MainNauczyciel : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public int IDwychowawcy;

        public MainNauczyciel()
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
            this.Close();
            form1.Show();
        }

        private void MainNauczyciel_Load(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT n.imie, n.nazwisko, n.email, n.telefon, (SELECT klasa FROM [dbo].[Klasy] WHERE IDwychowawcy = @IDwychowawcy) as klasa FROM [dbo].[Nauczyciele] n WHERE IDwychowawcy = @IDwychowawcy;";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IDwychowawcy", this.IDwychowawcy);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    label8.Text = reader["imie"].ToString();
                    label9.Text = reader["nazwisko"].ToString();
                    label10.Text = reader["email"].ToString();
                    label11.Text = reader["telefon"].ToString();
                    label1.Text = reader["klasa"].ToString();
                }
                conn.Close();
            }
        }

        private void onShowClass(object sender, EventArgs e)
        {
            Classes form = new Classes();
            form.Show();
        }

        private void onAddStudentToClass(object sender, EventArgs e)
        {
            AddStudentToClass form = new AddStudentToClass();
            form.Show();
        }

        private void onShowSubjects(object sender, EventArgs e)
        {
            Subjects form = new Subjects();
            form.Show();
        }

        private void onAddGrade(object sender, EventArgs e)
        {
            AddGrade form = new AddGrade();
            form.Show();
        }

        private void onAddClass(object sender, EventArgs e)
        {
            AddClasses form = new AddClasses();
            form.Show();
        }

        private void onShowStudent(object sender, EventArgs e)
        {
            Uczen form = new Uczen();
            form.Show();
        }
    }
}
