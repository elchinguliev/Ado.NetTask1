using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;

namespace WpfApp12
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        SqlConnection conn;
        string connectinString = "";
        public MainWindow()
        {
            InitializeComponent();
        }


        private void insertBtn_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                connectinString = ConfigurationManager.ConnectionStrings["myconn"].ConnectionString;
                conn.ConnectionString = connectinString;
                conn.Open();
                string insertQuery = "INSERT INTO Authors (Id,Firstname, Lastname) VALUES (@Id,@FirstName, @LastName)";
                using (SqlCommand command = new SqlCommand(insertQuery, conn))
                {
                    command.Parameters.AddWithValue("@Id", idTxt.Text);
                    command.Parameters.AddWithValue("@FirstName", firstNameTxt.Text);
                    command.Parameters.AddWithValue("@LastName", lastNameTxt.Text);
                    command.ExecuteNonQuery();
                }
                conn.Close();
            }
            idTxt.Clear();
            firstNameTxt.Clear();
            lastNameTxt.Clear();
        }

        private void showBtn_Click(object sender, RoutedEventArgs e)
        {


            DataTable table;
            SqlDataReader reader;
            conn = new SqlConnection();
            connectinString = ConfigurationManager.ConnectionStrings["myconn"].ConnectionString;
            using (conn = new SqlConnection())
            {
                var da = new SqlDataAdapter();
                conn.ConnectionString = connectinString;
                conn.Open();
                var set = new DataSet();
                SqlCommand command = new SqlCommand("SELECT * FROM Authors", conn);
                da.SelectCommand = command;
                da.Fill(set, "Authors");
                authorsDataGrid.ItemsSource = set.Tables[0].DefaultView;
            }
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
         

            using (SqlConnection conn = new SqlConnection())
            {
                connectinString = ConfigurationManager.ConnectionStrings["myconn"].ConnectionString;
                conn.ConnectionString = connectinString;
                conn.Open();
                string deleteQuery = "DELETE FROM Authors WHERE  Id = @id  ";

                using (SqlCommand command = new SqlCommand(deleteQuery, conn))
                {
                    command.Parameters.AddWithValue("@id", idTxt.Text);

                   command.ExecuteNonQuery();
                }
                conn.Close();
            }

            idTxt.Clear();
            firstNameTxt.Clear();
            lastNameTxt.Clear();

        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            DataSet set = new DataSet();
            using (conn = new SqlConnection())
            {
                conn.ConnectionString = connectinString;
                conn.Open();

                var command = new SqlCommand("UPDATE Authors SET Firstname=@firstName WHERE Id=@id", conn);

                command.Parameters.Add(new SqlParameter
                {
                    DbType = DbType.Int32,
                    ParameterName = "@id",
                    Value = 13
                });

                command.Parameters.Add(new SqlParameter
                {
                    SqlDbType=SqlDbType.NVarChar,
                    ParameterName="@firstName",
                    Value="Rufet"
                });

                var da = new SqlDataAdapter();
                da.UpdateCommand = command;
                da.UpdateCommand.ExecuteNonQuery();

                da.Update(set, "authorsbooks");
                set.Clear();

                da = new SqlDataAdapter("SELECT * FROM Authors;SELECT * FROM Books", conn);

                da.Fill(set, "authorsbooks");

                authorsDataGrid.ItemsSource = set.Tables[0].DefaultView;

            }

        }
    }
}
