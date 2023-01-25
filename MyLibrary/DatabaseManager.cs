using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyLibrary
{
    internal static class DatabaseManager
    {
        static string server = "localhost";
        static string database = "mylibrary";
        static string user = "linnea";
        static string password = "ninnenea";
        public static string connString = $"SERVER={server};DATABASE={database};UID={user};PWD={password};";

     

        /*
               private void addBooktoDB()
               {
                   bool valid = true;
                   foreach (TextBox textbox in txtBoxes)
                   {
                       textbox.Text = textbox.Text.Trim();
                       if (textbox.Text == "")
                       {
                           textbox.BackColor = Color.Red;
                           valid = false;
                       }
                       else
                       {
                           textbox.BackColor = TextBox.DefaultBackColor;
                       }
                   }

                   if (!valid)
                   {
                       MessageBox.Show("Missing data, please resubmit");
                       return;
                   }

                   //Hämta data från textfält
                   //string name = txtName.Text;
                   //int age = Convert.ToInt32(txtAge.Text);
                   //string petName = txtPet.Text;

                   //Bygg upp SQL querry
                   //string SQLquerry = $"INSERT INTO people(people_name, people_age,people_pet) VALUES ('{name}', {age}, '{petName}');";
                   //string SQLquerry = $"CALL insertPeople('{name}', {age}, '{petName}');";

                   MySqlCommand cmd = new MySqlCommand(query, conn);

                   try
                   {
                       conn.Open();
                       cmd.ExecuteReader();
                       conn.Close();
                   }
                   catch (Exception ex)
                   {
                       MessageBox.Show(ex.Message);
                   }
                   MessageBox.Show("Book added to " + location);
               }*/
    }
}
