using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Runtime;

namespace Ferret_Data_Dumper
{
    class SqlConnecter
    {
        string SqlConnectionString;
        public SqlConnecter(string ip, string username, string password, string database)
        {
            SqlConnectionString = $"Server={ip};Database={database};User Id={username};Password={password};";
        }

        public bool SqlTest()
        {
            try
            {
                SqlConnection conn = new SqlConnection(SqlConnectionString);
                conn.Open();
                conn.Close();
                return true;
            }
            catch { return false; }
        }

        public object Get(string command)
        {
            SqlConnection conn = new SqlConnection(SqlConnectionString);
            SqlCommand cmd = new SqlCommand(command, conn);
            conn.Open();
            SqlDataReader data = null;
            data = cmd.ExecuteReader();
            object[] fetchedData = new object[data.FieldCount];
            while (data.Read())
                for (int i = 0; i < data.FieldCount; i++)
                    fetchedData[i] = data[i].ToString();
            conn.Close();
            return fetchedData;
        }
        public object[] GetObject(string command)
        {
            SqlConnection conn = new SqlConnection(SqlConnectionString);
            SqlCommand cmd = new SqlCommand(command, conn);
            conn.Open();
            SqlDataReader data = null;
            data = cmd.ExecuteReader();
            object[] fetchedData = new object[data.FieldCount];
            while (data.Read())
                for (int i = 0; i < data.FieldCount; i++)
                    fetchedData[i] = data[i].ToString();
            conn.Close();
            return fetchedData;
        }
    }
}