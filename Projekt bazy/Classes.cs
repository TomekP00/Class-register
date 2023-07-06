using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekt_bazy
{
    public partial class Classes : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        public Classes()
        {
            InitializeComponent();
        }

        private void Subjects_Load(object sender, EventArgs e)
        {
            this.loadData();
        }

        private void onShowClass(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedValue == null) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT u.IDucznia, u.imie, u.nazwisko, u.pesel, n.imie, k.klasa FROM [dbo].[Klasy] k INNER JOIN [dbo].[Nauczyciele] n ON k.IDwychowawcy = n.IDWychowawcy " +
                            " INNER JOIN [dbo].[Uczniowie] u ON k.IDklasy = u.IDklasy" +
                            " WHERE k.IDklasy like @IDklasy;";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IDklasy", this.comboBox1.SelectedValue);
                SqlDataAdapter reader = new SqlDataAdapter(cmd);

                var table = new DataTable();
                reader.Fill(table);
                this.dataGridView1.DataSource = table;

                query = "SELECT n.imie, n.nazwisko FROM [dbo].[Klasy] k INNER JOIN [dbo].[Nauczyciele] n ON k.IDwychowawcy = n.IDWychowawcy " +
                           " WHERE k.IDklasy like @IDklasy;";
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IDklasy", this.comboBox1.SelectedValue);
                SqlDataReader reader2 = cmd.ExecuteReader();

                while (reader2.Read())
                {
                    this.label5.Text = (string)reader2["imie"] + " " + (string)reader2["nazwisko"];
                }

                conn.Close();
            }
        }

        private void loadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM [dbo].[Klasy];";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter reader = new SqlDataAdapter(cmd);

                var ds = new DataSet();
                reader.Fill(ds);
                this.comboBox1.ValueMember = "IDklasy";
                this.comboBox1.DisplayMember = "klasa";
                this.comboBox1.DataSource = ds.Tables[0];
                this.comboBox1.Enabled = true;

                conn.Close();
            }
        }

        private void onDeleteStudent(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count != 0)
            {
                DataGridViewRow row = this.dataGridView1.SelectedRows[0];
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE [dbo].[Uczniowie] SET IDklasy = NULL WHERE IDucznia = @IDucznia;";
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@IDucznia", row.Cells["IDucznia"].Value.ToString());
                    command.ExecuteNonQuery();

                    MessageBox.Show("Uczen usuniety");

                    conn.Close();
                    this.loadData();
                }
            }
        }
    }
}
