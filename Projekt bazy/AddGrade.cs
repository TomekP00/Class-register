using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Projekt_bazy
{
    public partial class AddGrade : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        SqlConnection conn = new SqlConnection();

        public AddGrade()
        {
            InitializeComponent();
        }

        private void AddGrade_Load(object sender, EventArgs e)
        {
            this.loadData();
        }

        private void onShowClass(object sender, EventArgs e)
        {
            if (this.comboBox4.SelectedValue == null) return;

            string query = "SELECT u.imie, u.nazwisko FROM [dbo].[Uczniowie] u INNER JOIN [dbo].[Klasy] k ON u.IDklasy = k.IDklasy" +
                           " WHERE k.IDklasy = @IDklasy;";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@IDklasy", this.comboBox4.SelectedValue);
            SqlDataAdapter reader = new SqlDataAdapter(cmd);

            var table = new DataTable();
            reader.Fill(table);
            this.dataGridView1.DataSource = table;

            query = "SELECT IDucznia, Try_Cast([IDucznia] AS varchar) + ' - ' +  [imie] + ' ' + [nazwisko] AS uczen FROM [dbo].[Uczniowie] WHERE IDklasy = @IDklasy;";
            cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@IDklasy", this.comboBox4.SelectedValue);
            reader = new SqlDataAdapter(cmd);

            var ds = new DataSet();
            reader.Fill(ds);
            this.comboBox1.ValueMember = "IDucznia";
            this.comboBox1.DisplayMember = "uczen";
            this.comboBox1.DataSource = ds.Tables[0];
            this.comboBox1.Enabled = true;
            this.comboBox1.SelectedIndex = -1;

            conn.Close();
        }

        private void onAddDegree(object sender, EventArgs e)
        {
            //combobox1 - uczen, combobox2 - przedmiot, combobox3 - typ oceny, textbox1 - ocena
            if (this.comboBox1.SelectedValue == null || this.comboBox2.SelectedValue == null || this.comboBox3.SelectedValue == null || this.textBox1.Text == "")
            {
                MessageBox.Show("Uzupelnij pola aby dodac ocene");
                return;
            }

            int ocena = 0;
            if (Int32.TryParse(this.textBox1.Text, out ocena))
                if (!(ocena >= 1 && ocena <= 6))
                {
                    MessageBox.Show("Ocena musi byc w przedziale 1-6");
                    return;
                }

            conn.ConnectionString = connectionString;
            conn.Open();

            string query = "INSERT INTO [dbo].[Oceny] (IDucznia, IDprzedmiotu, ocena, waga, data, IDtyp) VALUES (@IDucznia, @IDprzedmiotu, @ocena, @waga, @data, @IDtyp);";

            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@IDucznia", this.comboBox1.SelectedValue);
            command.Parameters.AddWithValue("@IDprzedmiotu", this.comboBox2.SelectedValue);
            command.Parameters.AddWithValue("@ocena", ocena);
            command.Parameters.AddWithValue("@waga", 3);
            command.Parameters.AddWithValue("@data", DateTime.Now);
            command.Parameters.AddWithValue("@IDtyp", this.comboBox3.SelectedValue);

            command.ExecuteNonQuery();
            this.comboBox1.SelectedIndex = -1;
            this.comboBox2.SelectedIndex = -1;
            this.comboBox3.SelectedIndex = -1;
            this.textBox1.Clear();
            MessageBox.Show("Ocena została dodana");

            conn.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void loadData()
        {
            conn.ConnectionString = connectionString;
            conn.Open();
            string query = "SELECT IDprzedmiotu, przedmiot FROM [dbo].[Przedmioty];";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter reader = new SqlDataAdapter(cmd);

            var ds = new DataSet();
            reader.Fill(ds);
            this.comboBox2.ValueMember = "IDprzedmiotu";
            this.comboBox2.DisplayMember = "przedmiot";
            this.comboBox2.DataSource = ds.Tables[0];
            this.comboBox2.Enabled = true;
            this.comboBox2.SelectedIndex = -1;

            query = "SELECT IDklasy, klasa FROM [dbo].[Klasy];";
            cmd = new SqlCommand(query, conn);
            reader = new SqlDataAdapter(cmd);

            ds = new DataSet();
            reader.Fill(ds);
            this.comboBox4.ValueMember = "IDklasy";
            this.comboBox4.DisplayMember = "klasa";
            this.comboBox4.DataSource = ds.Tables[0];
            this.comboBox4.Enabled = true;

            query = "SELECT IDtyp, typ FROM [dbo].[TypOceny];";
            cmd = new SqlCommand(query, conn);
            reader = new SqlDataAdapter(cmd);

            ds = new DataSet();
            reader.Fill(ds);
            this.comboBox3.ValueMember = "IDtyp";
            this.comboBox3.DisplayMember = "typ";
            this.comboBox3.DataSource = ds.Tables[0];
            this.comboBox3.Enabled = true;
            this.comboBox3.SelectedIndex = -1;

            conn.Close();
        }
    }
}
