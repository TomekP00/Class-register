using System.Configuration;
using System.Data.SqlClient;


namespace Projekt_bazy
{
    public partial class LoginScreen : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        SqlConnection conn = new SqlConnection();

        public int IDucznia;

        public LoginScreen()
        {
            InitializeComponent();
        }

        private void OnZalogujBtnClick(object sender, EventArgs e)
        {
            conn.ConnectionString = connectionString;

            String log, pass;

            log = Login.Text;
            pass = Pass.Text;
            if (String.IsNullOrEmpty(log) == true || String.IsNullOrEmpty(pass) == true)
            {
                MessageBox.Show("Podaj dane do logowania");
            }
            else
            {
                conn.Open();

                string query = "SELECT * FROM [dbo].[Uczniowie] WHERE login = '" + log + "' AND haslo = '" + pass + "';";
                string query2 = "SELECT * FROM [dbo].[Nauczyciele] WHERE login = '" + log + "' AND haslo = '" + pass + "';";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlCommand cmd2 = new SqlCommand(query2, conn);

                if (cmd.ExecuteScalar() == null && cmd2.ExecuteScalar() == null)
                {
                    MessageBox.Show("bledne dane logowania");
                }
                else
                {
                    if (cmd.ExecuteScalar() != null && cmd2.ExecuteScalar() == null)
                    {

                        MainUczen form3 = new MainUczen();
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            form3.IDucznia = (int)reader["IDucznia"];
                        }

                        form3.Show();
                        this.Hide();

                    }
                    else
                    {
                        MainNauczyciel form = new MainNauczyciel();
                        SqlDataReader reader = cmd2.ExecuteReader();

                        while (reader.Read())
                        {
                            form.IDwychowawcy = (int)reader["IDwychowawcy"];
                        }

                        form.Show();
                        this.Hide();
                    }
                }

                conn.Close();
            }



        }

        private void OnZarejestrujBtnClick(object sender, EventArgs e)
        {
            RegisterForm form2 = new RegisterForm();
            form2.Show();
            this.Hide();
        }

        private void OnExitBtnClick(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}