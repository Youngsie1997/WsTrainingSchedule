using System;
using System.Collections.Generic;
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
using Npgsql;
using System.Data;

namespace WsTrainingSchedule
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            
           
        }
        private DataSet ds = new DataSet();
        private DataTable dt = new DataTable();
        

        NpgsqlConnection conn = new NpgsqlConnection("Server=185.116.213.89;Port=5432;User Id=connor;Password=poop;Database=wstrainingschedule;");



        #region connectionStuff
        public void OpenConn()   //Opens a connection the Sql server
        {
            try
            {
                conn.Open();
            }
            catch (Exception Error)
            {
                MessageBox.Show(Error.ToString());
                conn.Close();
            }
        }

        public void CloseConn()   //closes the connection
        {
            try
            {
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error :S");
                conn.Close();
            }
        }
        #endregion

        private void tbSqlQuerry_Click(object sender, RoutedEventArgs e)  //Sends a sql querry
        {
           try
            {
                if(cbName.Text.ToString() == "Everyone" && cbDay.Text.ToString() != "All")   //Collects the schedule for everyone for a specific day
                {
                    this.OpenConn();
                    //Sql statement
                    string sql = "SELECT * FROM SCHEDULE WHERE \"Day\" = '"+cbDay.Text.ToString()+ "' ORDER BY CASE WHEN \"Day\"='Monday' THEN 1 WHEN \"Day\"='Tuesday' THEN 2 WHEN \"Day\"='Wednesday' THEN 3 WHEN \"Day\"='Thursday'Then 4 WHEN \"Day\" = 'Friday' THEN 5 end "; 
                    Npgsql.NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
                    //Reset Dataset
                    ds.Reset();
                    //Filling dataset with result from NpgsqlDataAdapter
                    da.Fill(ds);
                    //Since c# Data can handle multiple tables we will select the first.
                    dt = ds.Tables[0];
                    //Connect grid to datatable.
                    dgSchedule.ItemsSource = dt.DefaultView;
                    //Since we have our result we don't need the connection anymore
                    this.CloseConn();
                }

                if(cbName.Text.ToString() == "Everyone" && cbDay.Text.ToString() == "All")    //Collects the schedule for everyone for the whole week.
                {
                    this.OpenConn();
                    //sql statement
                    string sql = "SELECT * FROM Schedule ORDER BY CASE WHEN \"Day\"='Monday' THEN 1 WHEN \"Day\"='Tuesday' THEN 2 WHEN \"Day\"='Wednesday' THEN 3 WHEN \"Day\"='Thursday'Then 4 WHEN \"Day\" = 'Friday' THEN 5 end";
                    Npgsql.NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
                    //Reset Dataset
                    ds.Reset();
                    da.Fill(ds);
                    dt = ds.Tables[0];
                    //Connect grid to datatable
                    dgSchedule.ItemsSource = dt.DefaultView;
                    
                    this.CloseConn();

                }








                if (cbName.Text.ToString() != "Everyone" && cbDay.Text.ToString() == "All" )  //Collects schedule for one person
                {
                    this.OpenConn();
                    //Sql statement
                    string sql = "SELECT * FROM SCHEDULE WHERE \"Name\" ='"+cbName.Text.ToString()+ "' ORDER BY CASE WHEN \"Day\"='Monday' THEN 1 WHEN \"Day\"='Tuesday' THEN 2 WHEN \"Day\"='Wednesday' THEN 3 WHEN \"Day\"='Thursday'Then 4 WHEN \"Day\" = 'Friday' THEN 5 end";
                    Npgsql.NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
                    //Reset Dataset
                    ds.Reset();
                    //Filling dataset with result from NpgsqlDataAdapter
                    da.Fill(ds);
                    //Since c# Data can handle multiple tables we will select the first.
                    dt = ds.Tables[0];
                    //Connect grid to datatable.
                    dgSchedule.ItemsSource = dt.DefaultView;
                    //Since we have our result we don't need the connection anymore
                    this.CloseConn();
                }

                if (cbName.Text.ToString() != "Everyone" && cbDay.Text.ToString() != "All")  //Collects schedule for one person for a specfic day
                {
                    this.OpenConn();
                    //sql statement
                    string sql = "SELECT * FROM SCHEDULE WHERE \"Name\"='"+cbName.Text.ToString()+"' AND \"Day\"='"+cbDay.Text.ToString()+ "' ORDER BY CASE WHEN \"Day\"='Monday' THEN 1 WHEN \"Day\"='Tuesday' THEN 2 WHEN \"Day\"='Wednesday' THEN 3 WHEN \"Day\"='Thursday'Then 4 WHEN \"Day\" = 'Friday' THEN 5 end";
                    Npgsql.NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
                    //Reset Dataset
                    ds.Reset();
                    da.Fill(ds);
                    dt = ds.Tables[0];
                    dgSchedule.ItemsSource = dt.DefaultView;
                    this.CloseConn();

                }
                
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.ToString());
                this.CloseConn();
            }
        }

        private void dgSchedule_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            
        }
    }
}
