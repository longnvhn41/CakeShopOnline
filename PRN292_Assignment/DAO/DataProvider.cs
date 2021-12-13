using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CakeShop.DAO
{
    public class DataProvider
    {
        public DataProvider()
        {
            connect();
        }

        SqlConnection cnn;  
        SqlDataAdapter da;  
        SqlCommand cmd; 
        private void connect()
        {
            try
            {
                string strCnn = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
                cnn = new SqlConnection(strCnn);
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
                cnn.Open();
                Console.WriteLine("Connect success!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Loi ket noi: " + ex.Message);
            }
        }

        public DataTable executeQuery(string strSelect, string nameMethod)
        {
            DataTable dt = new DataTable(); 
            try
            {
                da = new SqlDataAdapter(strSelect, cnn);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                Console.WriteLine(nameMethod + ": " + ex.Message);
            }
            return dt;
        }

        public bool executeNonQuery(string strExecute, string nameMethod)
        {
            try
            {
                cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = strExecute;
                cmd.ExecuteNonQuery();
                return true;
            }

            catch (Exception ex)
            {
                Console.WriteLine(nameMethod + ": " + ex.Message);
            }
            return false;
        }
    }
}