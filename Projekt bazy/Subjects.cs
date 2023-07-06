using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Projekt_bazy
{
    public partial class Subjects : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        public Subjects()
        {
            InitializeComponent();
        }

        private void Subjects_Load(object sender, EventArgs e)
        {
            this.loadData();
        }

        private void onAddSubject(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "") return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM [dbo].[Przedmioty] WHERE przedmiot LIKE (@przedmiot);";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@przedmiot", this.textBox1.Text);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    MessageBox.Show("Przedmiot już istnieje.");
                    reader.Close();
                    conn.Close();
                    return;
                }
                reader.Close();

                query = "INSERT INTO [dbo].[Przedmioty] (przedmiot) VALUES (@przedmiot);";
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@przedmiot", this.textBox1.Text);
                cmd.ExecuteNonQuery();

                conn.Close();
                this.textBox1.Clear();
                this.loadData();
            }
        }

        private void onDeleteSubject(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedValue == null) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT count(IDprzedmiotu) as liczba FROM [dbo].[Oceny] WHERE IDprzedmiotu = (@IDprzedmiotu);";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IDprzedmiotu", this.comboBox1.SelectedValue);
                SqlDataReader reader = cmd.ExecuteReader();

                while(reader.Read()) 
                {
                    if ((int)reader["liczba"] != 0)
                    {
                        MessageBox.Show("Nie można usunąć przedmiotu");
                        conn.Close();
                        reader.Close();
                        return;
                    }
                }
                reader.Close();

                query = "DELETE FROM [dbo].[Przedmioty] WHERE IDprzedmiotu LIKE (@IDprzedmiotu);";
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IDprzedmiotu", this.comboBox1.SelectedValue);
                cmd.ExecuteReader();

                conn.Close();
                this.loadData();
            }
        }

        private void loadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM [dbo].[Przedmioty];";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter reader = new SqlDataAdapter(cmd);

                var table = new DataTable();
                reader.Fill(table);
                this.dataGridView1.DataSource = table;

                var ds = new DataSet();
                reader.Fill(ds);
                this.comboBox1.ValueMember = "IDprzedmiotu";
                this.comboBox1.DisplayMember = "przedmiot";
                this.comboBox1.DataSource = ds.Tables[0];
                this.comboBox1.Enabled = true;
                this.comboBox1.SelectedIndex = -1;

                this.comboBox2.ValueMember = "IDprzedmiotu";
                this.comboBox2.DisplayMember = "przedmiot";
                this.comboBox2.DataSource = ds.Tables[0];
                this.comboBox2.Enabled = true;
                this.comboBox2.SelectedIndex = -1;

                conn.Close();
            }
        }

        private void onEditSubject(object sender, EventArgs e)
        {
            if (this.comboBox2.SelectedValue == null || this.textBox2.Text == "") return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE [dbo].[Przedmioty] SET przedmiot = @przedmiot WHERE IDprzedmiotu = @IDprzedmiotu;";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@przedmiot", this.textBox2.Text);
                command.Parameters.AddWithValue("@IDprzedmiotu", this.comboBox2.SelectedValue);
                command.ExecuteNonQuery();

                conn.Close();
                this.textBox2.Clear();
                this.loadData();
            }
        }
    }
}
