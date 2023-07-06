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
using System.Diagnostics;
using System.Reflection.Emit;
using Microsoft.VisualBasic.Logging;
using System.Security.Policy;

namespace Projekt_bazy
{
    public partial class AddStudentToClass : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public int IDwychowawcy;

        public AddStudentToClass()
        {
            InitializeComponent();
        }

        private void AddStudentToClass_Load(object sender, EventArgs e)
        {
            this.updateData();//wczytanie danych do kontrolek
        }

        private void onSubmit(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedValue == null || this.comboBox2.SelectedValue == null) return;
            //combobox1 - uczen, combobox2 - klasa
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE [dbo].[Uczniowie] SET IDklasy = @IDklasy WHERE IDucznia = @IDucznia;";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@IDklasy", this.comboBox2.SelectedValue);
                command.Parameters.AddWithValue("@IDucznia", this.comboBox1.SelectedValue);
                command.ExecuteNonQuery();
                
                conn.Close();   
                //update everything
                this.updateData();
            }
        }

        private void updateData()
        {
            //uczeń musi mieć null w IDklasa
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT IDucznia, imie, nazwisko, pesel, email FROM [dbo].[Uczniowie] WHERE IDklasy IS NULL;";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter reader = new SqlDataAdapter(cmd);

                var table = new DataTable();
                reader.Fill(table);
                this.dataGridView1.DataSource = table;

                query = "SELECT IDucznia, Try_Cast([IDucznia] AS varchar) + ' - ' +  [imie] + ' ' + [nazwisko] AS uczen FROM [dbo].[Uczniowie] WHERE IDklasy IS NULL;";
                cmd = new SqlCommand(query, conn);
                reader = new SqlDataAdapter(cmd);

                var ds = new DataSet();
                reader.Fill(ds);
                this.comboBox1.ValueMember = "IDucznia";
                this.comboBox1.DisplayMember = "uczen";
                this.comboBox1.DataSource = ds.Tables[0];
                this.comboBox1.Enabled = true;
                this.comboBox1.SelectedIndex = -1;

                query = "SELECT IDklasy, klasa FROM [dbo].[Klasy];";
                cmd = new SqlCommand(query, conn);
                reader = new SqlDataAdapter(cmd);

                ds = new DataSet();
                reader.Fill(ds);
                this.comboBox2.ValueMember = "IDklasy";
                this.comboBox2.DisplayMember = "klasa";
                this.comboBox2.DataSource = ds.Tables[0];
                this.comboBox2.Enabled = true;
                this.comboBox2.SelectedIndex = -1;

                conn.Close();
            }
        }
    }
}
