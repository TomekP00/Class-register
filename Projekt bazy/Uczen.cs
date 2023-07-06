using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekt_bazy
{
    public partial class Uczen : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public Uczen()
        {
            InitializeComponent();
        }

        private void Uczen_Load(object sender, EventArgs e)
        {
            this.loadData();
        }

        private void loadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT IDklasy, klasa FROM [dbo].[Klasy];";
                SqlCommand command = new SqlCommand(query, conn);
                SqlDataAdapter reader = new SqlDataAdapter(command);

                var ds = new DataSet();
                reader.Fill(ds);
                this.comboBox1.ValueMember = "IDklasy";
                this.comboBox1.DisplayMember = "klasa";
                this.comboBox1.DataSource = ds.Tables[0];
                this.comboBox1.Enabled = true;
                this.comboBox1.SelectedIndex = -1;

                query = "SELECT * FROM [dbo].[Przedmioty];";
                command = new SqlCommand(query, conn);
                reader = new SqlDataAdapter(command);

                ds = new DataSet();
                reader.Fill(ds);
                this.comboBox3.ValueMember = "IDprzedmiotu";
                this.comboBox3.DisplayMember = "Przedmiot";
                this.comboBox3.DataSource = ds.Tables[0];
                this.comboBox3.Enabled = true;
                this.comboBox3.SelectedIndex = -1;

                conn.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedValue == null) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT u.IDucznia, u.imie + ' ' + u.nazwisko AS Uczen FROM [dbo].[Klasy] k INNER JOIN [dbo].[Uczniowie] u ON k.IDklasy = u.IDklasy WHERE u.IDklasy = @IDklasy;";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@IDklasy", this.comboBox1.SelectedValue);
                SqlDataAdapter reader = new SqlDataAdapter(command);

                var ds = new DataSet();
                reader.Fill(ds);
                this.comboBox2.ValueMember = "IDucznia";
                this.comboBox2.DisplayMember = "Uczen";
                this.comboBox2.DataSource = ds.Tables[0];
                this.comboBox2.Enabled = true;
                this.comboBox2.SelectedIndex = -1;

                conn.Close();
            }
        }

        private void onShowGrades(object sender, EventArgs e)
        {
            if (this.comboBox2.SelectedValue == null || this.comboBox3.SelectedValue == null) return;

            this.gradestable();
        }


        private void onDeleteGrade(object sender, EventArgs e)
        {
            if (Oceny.SelectedRows.Count != 0)
            {
                DataGridViewRow row = this.Oceny.SelectedRows[0];
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM [dbo].[Oceny] WHERE IDoceny = @IDoceny";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@IDoceny", row.Cells["IDoceny"].Value.ToString());
                    cmd.ExecuteReader();

                    MessageBox.Show("Ocena usunieta");

                    conn.Close();
                    this.gradestable();
                }
            }
        }

        private void gradestable()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT o.IDoceny, o.ocena, o.waga, t.typ, o.data FROM [dbo].[Oceny] o INNER JOIN [dbo].[TypOceny] t ON o.IDtyp = t.IDtyp" +
                            " WHERE o.IDucznia like @IDucznia AND IDprzedmiotu like @IDprzedmiotu;";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IDucznia", this.comboBox2.SelectedValue);
                cmd.Parameters.AddWithValue("@IDprzedmiotu", this.comboBox3.SelectedValue);
                SqlDataAdapter reader = new SqlDataAdapter(cmd);

                var table = new DataTable();
                reader.Fill(table);
                Oceny.DataSource = table;

                conn.Close();
            }
        }
    }
}
