using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MikroTikTestingApp
{
    static class SQLiteClass
    {
        public static void CreateTables()
        {
            SQLiteConnection sqlite_conn;
            sqlite_conn = CreateConnection();
            CreateTable(sqlite_conn);
            sqlite_conn.Close();
        }

        public static void ClearTables()
        {
            SQLiteConnection sqlite_conn;
            sqlite_conn = CreateConnection();
            RemoveData(sqlite_conn);
            sqlite_conn.Close();
        }

        public static void FillTables(string date,string ethStatus,string wirStatus)
        {
            SQLiteConnection sqlite_conn;
            sqlite_conn = CreateConnection();
            InsertData(sqlite_conn,date,ethStatus,wirStatus);
            sqlite_conn.Close();
        }

        //for ethernet status table
        public static List<String> ReadFromEther()
        {
            List<string> toReturn = new List<string>();
            SQLiteConnection sqlite_conn;
            sqlite_conn = CreateConnection();
            toReturn = ReadDataEth(sqlite_conn);
            sqlite_conn.Close();
            return toReturn;
        }
        //TODO fore wireless status table
        public static List<String> ReadFromWir()
        {
            List<string> toReturn = new List<string>();
            SQLiteConnection sqlite_conn;
            sqlite_conn = CreateConnection();
            toReturn = ReadDataWir(sqlite_conn);
            sqlite_conn.Close();
            return toReturn;
        }

        //for exporting to file
        public static DataTable ExportToFile()
        {
            DataTable dt;
            SQLiteConnection sqlite_conn;
            sqlite_conn = CreateConnection();
            dt = ReadDataToFile(sqlite_conn);
            sqlite_conn.Close();
            return dt;
        }
        //------------------------------------------------------

        static SQLiteConnection CreateConnection()
        {
            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            sqlite_conn = new SQLiteConnection("Data Source=MTpabloDB.db; Version = 3; New = True; Compress = True; ");
            // Open the connection:
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return sqlite_conn;
        }

        static void CreateTable(SQLiteConnection conn)
        {

            SQLiteCommand sqlite_cmd;
            string Createsql = "CREATE TABLE IF NOT EXISTS MTStatus(CurTime VARCHAR(10), EthStatus VARCHAR(10), WirStatus VARCHAR(10))";
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            sqlite_cmd.ExecuteNonQuery();

        }

        static void RemoveData(SQLiteConnection conn)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();

            sqlite_cmd.CommandText = "DELETE FROM MTStatus;";
            sqlite_cmd.ExecuteNonQuery();
        }

        static void InsertData(SQLiteConnection conn, string date, string ethStatus, string wirStatus)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();

            sqlite_cmd.CommandText = $"INSERT INTO MTStatus(CurTime, EthStatus, WirStatus) VALUES('{date}','{ethStatus}','{wirStatus}'); ";
            sqlite_cmd.ExecuteNonQuery();
        }

        static List<string> ReadDataEth(SQLiteConnection conn)
        {
            List<string> toReturn = new List<string>();
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM MTStatus";

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                toReturn.Add(sqlite_datareader.GetString(1)+"\n"+sqlite_datareader.GetString(0));
            }

            return toReturn;
        }

        static List<string> ReadDataWir(SQLiteConnection conn)
        {
            List<string> toReturn = new List<string>();
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM MTStatus";

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                toReturn.Add(sqlite_datareader.GetString(2) + "\n" + sqlite_datareader.GetString(0));
            }

            return toReturn;
        }

        //exporting to file test
        static DataTable ReadDataToFile(SQLiteConnection conn)
        {
            DataTable dt = new DataTable();

            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "select * from MTStatus";

            SQLiteDataAdapter da = new SQLiteDataAdapter(sqlite_cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dt);
            da.Dispose();

           
           // sqlite_cmd.ExecuteNonQuery();

            return dt;
        }
    }
}
