using MySql.Data.MySqlClient;
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
using System.Windows.Shapes;
using System.Xml.Linq;

namespace MyLibrary
{
    /// <summary>
    /// Interaction logic for AddBook.xaml
    /// </summary>
    public partial class AddBook : Window
    {
        private MainWindow mainWindow;
        string location;
        MySqlConnection conn;


        public AddBook(MainWindow mainWindow, string location)
        {
            this.mainWindow = mainWindow;
            this.location = location;

            InitializeComponent();
        }

        internal void createBook() {

            conn = new MySqlConnection(DatabaseManager.connString);
            bool valid = false;

            foreach (TextBox box in boxes.Children.OfType<TextBox>())
            {
                box.Text = box.Text.Trim();

                if (box.Text == "")
                {
                    box.Background = Brushes.Red;
                }
                else
                {
                    box.Background = Brushes.White;
                    valid = true;
                }
            }

            if (valid)
            {
                returnText.Text = "Complete missing data in marked fields.";
                return;
            }

            string author = authorBox.Text;
            string title = titleBox.Text;
            int pages = Convert.ToInt32(pagesBox.Text);
            int series = Convert.ToInt32(seriesBox.Text);
            int year_written = Convert.ToInt32(yearBox.Text);
            string language = languageBox.Text;
            string genre = genreBox.Text;

            string query = $"CALL create_book('{author}','{title}','{pages}','{series}','{year_written}', '{language}', '{location}', '{genre}');";
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

            //Hämta personer från DB
            //selectFromDB();

            //Markera sista raden
            //gridOutput.Rows[gridOutput.Rows.Count - 2].Selected = true;

            //Hämta data
            //getSelectedRow();

            returnText.Text = "Book added to library";
            foreach (TextBox box in boxes.Children.OfType<TextBox>())
            {
                box.Clear();
                returnText.Text = "";
            }


        }

        private void returnButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.Show();
            this.Close();
        }
    }
}

