using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary
{
    internal class DatabaseTable
    {
        public static DataTable GetTable(String query, String sortBy)
        {
            MySqlConnection conn = new MySqlConnection(DatabaseManager.connString);
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataTable.DefaultView.Sort = sortBy;
            return dataTable;
        }

        public static void GetTable(String query, DataTable refreshDataTable)
        {
            refreshDataTable.Clear();
            MySqlConnection conn = new MySqlConnection(DatabaseManager.connString);
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
            adapter.Fill(refreshDataTable);
        }
    }
}
