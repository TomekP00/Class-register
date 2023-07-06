using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;

namespace Projekt_bazy
{
    public partial class RegisterForm : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        SqlConnection conn = new SqlConnection();

        int pom = 0;
        int ktorej = 0; // pomocnicza mowiaca czy rejestruje sie uczen czy nauczyciel

        public RegisterForm()
        {
            InitializeComponent();
            string[] opcje = { "Uczeń", "Nauczyciel" };

            comboBox1.DataSource = opcje;
        }

        private void OnZalogujBtnClick(object sender, EventArgs e)
        {
            LoginScreen form2 = new LoginScreen();
            form2.Show();
            this.Hide();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0) // jeżeli uczeń
            {
                if (pom == 1)
                {
                    textBox4.Show();
                    label4.Show();

                    int yLocation3 = textBox3.Location.Y;
                    textBox3.Location = new Point(105, yLocation3 + 29);
                    int yLocationL1 = label5.Location.Y;
                    label5.Location = new Point(12, yLocationL1 + 32);

                    int yLocation6 = textBox6.Location.Y;
                    textBox6.Location = new Point(105, yLocation6 + 29);
                    int yLocationL2 = label6.Location.Y;
                    label6.Location = new Point(12, yLocationL2 + 32);

                    int yLocation5 = textBox5.Location.Y;
                    textBox5.Location = new Point(105, yLocation5 + 29);
                    int yLocationL3 = label7.Location.Y;
                    label7.Location = new Point(12, yLocationL3 + 32);

                    int yLocation7 = textBox7.Location.Y;
                    textBox7.Location = new Point(105, yLocation7 + 29);
                    int yLocationL4 = label8.Location.Y;
                    label8.Location = new Point(12, yLocationL4 + 32);

                    int yLocation8 = textBox8.Location.Y;
                    textBox8.Location = new Point(105, yLocation8 + 29);
                    int yLocationL5 = label9.Location.Y;
                    label9.Location = new Point(12, yLocationL5 + 32);

                    pom = 0;
                    ktorej = 0;
                }
            }
            else //jeżeli nauczyciel
            {
                if (pom == 0)
                {
                    textBox4.Hide(); //ukrycie PESELU bo niepotrzebny
                    label4.Hide();

                    int yLocation3 = textBox3.Location.Y;
                    textBox3.Location = new Point(105, yLocation3 - 29);
                    int yLocationL1 = label5.Location.Y;
                    label5.Location = new Point(12, yLocationL1 - 32);

                    int yLocation6 = textBox6.Location.Y;
                    textBox6.Location = new Point(105, yLocation6 - 29);
                    int yLocationL2 = label6.Location.Y;
                    label6.Location = new Point(12, yLocationL2 - 32);

                    int yLocation5 = textBox5.Location.Y;
                    textBox5.Location = new Point(105, yLocation5 - 29);
                    int yLocationL3 = label7.Location.Y;
                    label7.Location = new Point(12, yLocationL3 - 32);

                    int yLocation7 = textBox7.Location.Y;
                    textBox7.Location = new Point(105, yLocation7 - 29);
                    int yLocationL4 = label8.Location.Y;
                    label8.Location = new Point(12, yLocationL4 - 32);

                    int yLocation8 = textBox8.Location.Y;
                    textBox8.Location = new Point(105, yLocation8 - 29);
                    int yLocationL5 = label9.Location.Y;
                    label9.Location = new Point(12, yLocationL5 - 32);
                    pom = 1;
                    ktorej = 1;
                }

            }
        }

        private void OnZarejestrujBtnClick(object sender, EventArgs e)
        {
            String imie, nazwisko, email, login, haslo, pothaslo, pesel, nrtel;
            conn.ConnectionString = connectionString;
            

            imie = textBox1.Text;
            nazwisko = textBox2.Text;
            email = textBox3.Text;
            login = textBox5.Text;
            haslo = textBox7.Text;
            pothaslo = textBox8.Text;
            pesel = textBox4.Text;
            nrtel = textBox6.Text;

            bool isValid = Regex.IsMatch(email, @"^(([^<>()[\]\\.,;:\s@\]+ (\.[^<> ()[\]\\.,;:\s@\]+)*)|(\.+\))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$");
            Debug.WriteLine(isValid);
            conn.Open();
            string query = "SELECT login FROM [dbo].[Uczniowie] WHERE login = '" + login + "';";
            SqlCommand cmd = new SqlCommand(query, conn);
            string query2 = "SELECT login FROM [dbo].[Nauczyciele] WHERE login = '" + login + "';";
            SqlCommand cmd2 = new SqlCommand(query2, conn);
            if (ktorej == 0 && cmd.ExecuteScalar() == null && this.textBox7.TextLength > 4 && Regex.IsMatch(pesel, "^[0-9]{11}$") && Regex.IsMatch(nrtel, "^[0-9]{9}$") && String.IsNullOrEmpty(imie) != true && String.IsNullOrEmpty(nazwisko) != true && String.IsNullOrEmpty(email) != true && isEmail(email) && String.IsNullOrEmpty(login) != true && String.IsNullOrEmpty(haslo) != true && String.IsNullOrEmpty(pothaslo) != true && haslo.Equals(pothaslo))
            {
                MessageBox.Show("Poprawne dane");
                 query = "INSERT INTO [dbo].[Uczniowie] (IDklasy, imie, nazwisko, pesel, email, telefon, login, haslo) VALUES (NULL, @imie, @nazwisko, @pesel, @email, @nrtel, @login, @haslo);";

                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@imie", imie);
                command.Parameters.AddWithValue("@nazwisko", nazwisko);
                command.Parameters.AddWithValue("@pesel", pesel);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@nrtel", nrtel);
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@haslo", haslo);

                command.ExecuteNonQuery();
            }
            else if (ktorej == 1 && cmd2.ExecuteScalar() == null && this.textBox7.TextLength > 4 && Regex.IsMatch(nrtel, "^[0-9]{9}$") && String.IsNullOrEmpty(imie) != true && String.IsNullOrEmpty(nazwisko) != true && String.IsNullOrEmpty(email) != true && String.IsNullOrEmpty(login) != true && isEmail(email) && String.IsNullOrEmpty(haslo) != true && String.IsNullOrEmpty(pothaslo) != true && haslo.Equals(pothaslo))
            {
                MessageBox.Show("Poprawne dane");
                query = "INSERT INTO [dbo].[Nauczyciele] (imie, nazwisko, email, telefon, login, haslo) VALUES (@imie, @nazwisko, @email, @nrtel, @login, @haslo);";

                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@imie", imie);
                command.Parameters.AddWithValue("@nazwisko", nazwisko);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@nrtel", nrtel);
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@haslo", haslo);

                command.ExecuteNonQuery();
            }
            else
            {
                MessageBox.Show("Błędne dane. Sprawdź poprawność podanych informacji");
            }
            conn.Close();

        }
        public static bool isEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }
    }
}