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
    public partial class AddClasses : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public AddClasses()
        {
            InitializeComponent();
        }

        private void AddClasses_Load(object sender, EventArgs e)
        {
            this.loadData();
        }

        private void onAddClass(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "" || this.comboBox1.SelectedValue == null) return;


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO [dbo].[Klasy] (IDWychowawcy, klasa) VALUES (@IDWychowawcy, @klasa);";

                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@IDWychowawcy", this.comboBox1.SelectedValue);
                command.Parameters.AddWithValue("@klasa", this.textBox1.Text);

                command.ExecuteNonQuery();
                this.comboBox1.SelectedIndex = -1;
                this.textBox1.Clear();
                MessageBox.Show("Klasa została dodana");

                conn.Close();
                this.loadData();
            }
        }

        private void onChangeTeacher(object sender, EventArgs e)
        {
            if (this.comboBox2.SelectedValue == null || this.comboBox3.SelectedValue == null) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE [dbo].[Klasy] SET IDwychowawcy = @IDwychowawcyNew WHERE IDwychowawcy = @IDwychowawcyOld;";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@IDwychowawcyNew", this.comboBox3.SelectedValue);
                command.Parameters.AddWithValue("@IDwychowawcyOld", this.comboBox2.SelectedValue);
                command.ExecuteReader();

                conn.Close();
                this.loadData();
            }
        }

        private void loadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT k.klasa, n.imie + ' ' + n.nazwisko AS Wychowawca, \r\n(SELECT COUNT(IDucznia) FROM [dbo].[Uczniowie] u WHERE u.IDklasy = k.IDklasy) AS 'Liczba Uczniow'\r\nFROM [dbo].[Klasy] k \r\nINNER JOIN [dbo].[Nauczyciele] n ON k.IDwychowawcy = n.IDwychowawcy\r\nGROUP BY k.klasa, n.imie, n.nazwisko, k.IDklasy";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter reader = new SqlDataAdapter(cmd);

                var table = new DataTable();
                reader.Fill(table);
                this.dataGridView1.DataSource = table;

                query = "SELECT IDwychowawcy, imie + ' ' + nazwisko AS Wychowawca\r\nFROM [dbo].[Nauczyciele] n\r\nwhere not exists (SELECT * from [dbo].[Klasy] k where n.IDwychowawcy = k.IDwychowawcy);";
                cmd = new SqlCommand(query, conn);
                reader = new SqlDataAdapter(cmd);

                var ds = new DataSet();
                reader.Fill(ds);
                this.comboBox1.ValueMember = "IDwychowawcy";
                this.comboBox1.DisplayMember = "Wychowawca";
                this.comboBox1.DataSource = ds.Tables[0];
                this.comboBox1.Enabled = true;
                this.comboBox1.SelectedIndex = -1;

                query = "SELECT IDklasy, klasa FROM [dbo].[Klasy];";
                cmd = new SqlCommand(query, conn);
                reader = new SqlDataAdapter(cmd);

                ds = new DataSet();
                reader.Fill(ds);
                this.comboBox4.ValueMember = "IDklasy";
                this.comboBox4.DisplayMember = "klasa";
                this.comboBox4.DataSource = ds.Tables[0];
                this.comboBox4.Enabled = true;
                this.comboBox4.SelectedIndex = -1;

                query = "SELECT n.IDwychowawcy, k.klasa FROM [dbo].[Klasy] k INNER JOIN [dbo].[Nauczyciele] n ON k.IDwychowawcy = n.IDwychowawcy;";
                cmd = new SqlCommand(query, conn);
                reader = new SqlDataAdapter(cmd);

                ds = new DataSet();
                reader.Fill(ds);
                this.comboBox2.ValueMember = "IDwychowawcy";
                this.comboBox2.DisplayMember = "klasa";
                this.comboBox2.DataSource = ds.Tables[0];
                this.comboBox2.Enabled = true;
                this.comboBox2.SelectedIndex = -1;

                conn.Close();
            }
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.comboBox2.SelectedValue == null) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT IDwychowawcy, imie + ' ' + nazwisko AS Wychowawca\r\nFROM [dbo].[Nauczyciele] n\r\nwhere not exists (SELECT * from [dbo].[Klasy] k where n.IDwychowawcy = k.IDwychowawcy);";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IDWychowawcy", this.comboBox2.SelectedValue);
                SqlDataAdapter reader = new SqlDataAdapter(cmd);

                var ds = new DataSet();
                reader.Fill(ds);
                this.comboBox3.ValueMember = "IDwychowawcy";
                this.comboBox3.DisplayMember = "Wychowawca";
                this.comboBox3.DataSource = ds.Tables[0];
                this.comboBox3.Enabled = true;
                this.comboBox3.SelectedIndex = -1;

                conn.Close();
            }
        }

        private void onDeleteClass(object sender, EventArgs e)
        {
            if (this.comboBox4.SelectedValue == null) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(IDucznia) as count FROM [dbo].[Uczniowie] u WHERE u.IDklasy = @IDklasy;";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IDklasy", this.comboBox4.SelectedValue);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if ((int)reader["count"] != 0)
                    {

                        MessageBox.Show("Klasa ma uczniów, nie mozna usunac");
                        conn.Close();
                        return;
                    }
                }
                reader.Close();
                query = "DELETE FROM [dbo].[klasy] WHERE IDklasy = @IDklasy;";
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IDklasy", this.comboBox4.SelectedValue);
                cmd.ExecuteReader();

                conn.Close();
                this.loadData();
            }
        }
    }
}
