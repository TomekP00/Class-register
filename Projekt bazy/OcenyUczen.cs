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
    public partial class OcenyUczen : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        SqlConnection conn = new SqlConnection();
        public int IDucznia;
        public OcenyUczen()
        {
            InitializeComponent();
        }

        private void OcenyUczenLoad(object sender, EventArgs e)
        {
            conn.ConnectionString = connectionString;
            conn.Open();
            string query = "SELECT * FROM [dbo].[Przedmioty];";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter reader = new SqlDataAdapter(cmd);

            var ds = new DataSet();
            reader.Fill(ds);
            this.comboBox1.ValueMember = "IDprzedmiotu";
            this.comboBox1.DisplayMember = "Przedmiot";
            this.comboBox1.DataSource = ds.Tables[0];
            this.comboBox1.Enabled = true;
            this.comboBox1.SelectedIndex = -1;

            conn.Close();

        }

        private void OnShowGradesClick(object sender, EventArgs e)
        {
            conn.ConnectionString = connectionString;
            conn.Open();
            string query = "SELECT o.ocena, o.waga, t.typ, o.data FROM [dbo].[Oceny] o INNER JOIN [dbo].[TypOceny] t ON o.IDtyp = t.IDtyp" +
                            " WHERE o.IDucznia like @IDucznia AND IDprzedmiotu like @IDprzedmiotu;";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@IDucznia", IDucznia);
            cmd.Parameters.AddWithValue("@IDprzedmiotu", this.comboBox1.SelectedValue);
            SqlDataAdapter reader = new SqlDataAdapter(cmd);

            var table = new DataTable();
            reader.Fill(table);
            Oceny.DataSource = table;
          /*  query = "SELECT n.nazwisko FROM Nauczyciele n INNER JOIN Oceny o ON n.IDwychowawcy = o.IDwychowawcy WHERE IDprzedmiotu LIKE @IDprzedmiotu";
            SqlCommand cmd2 = new SqlCommand(query, conn);
            cmd2.Parameters.AddWithValue("@IDprzedmiotu", this.comboBox1.SelectedValue);
            SqlDataReader reader2 = cmd2.ExecuteReader();
            while (reader2.Read())
            {
                label1.Text = reader2["przedmiot"].ToString();
            }*/
           



            conn.Close();
        }
    }
}

