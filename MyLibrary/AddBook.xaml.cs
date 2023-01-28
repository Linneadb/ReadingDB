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
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

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

        //Book.X = new Book(author, nationality, title, pages, series, year_written, language, location, genre)

        public AddBook(MainWindow mainWindow, string location)
        {
            this.mainWindow = mainWindow;
            this.location = location;

            InitializeComponent();
            loadPage();
            
        }

        internal void loadPage() {
            if (location == "Bookshelf")
                header.Text = "Add book to Library";
                pile.Visibility = Visibility.Visible;

            if (location == "Wishlist")
                header.Text = "Add book to Wishlist";

            if (location == "searchAuthor")
                header.Text = "Search author name";
                addButton.Visibility = Visibility.Hidden;
                pile.Visibility = Visibility.Hidden;
                bookButton.Visibility = Visibility.Hidden;
        }

        internal void searchAuthor() {

            string keyword;
            conn = new MySqlConnection(DatabaseManager.connString);

            if (authorBox.Text != "")
            {
                keyword = authorBox.Text.Trim();
            }
            else
            {
                returnText.Text = "You have not entered a name to search for.";
                return;
            }

            string query = $"CALL search_author('{keyword}');";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            try
            {
                conn.Open();

                MySqlDataReader reader = cmd.ExecuteReader();
                Book.authorList.Clear();
                while (reader.Read())
                {
                    string author = reader["authors_name"].ToString();
                    string nationality = reader["authors_nationality"].ToString();
                   
                    Book.authorList.Add(new Book(author, nationality));

                    //print searchresult to textBoxes
                    authorBox.Text = author;
                    nationBox.Text = nationality;
                }

                reader.Close();
                conn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }


            if (author.Equals(null))
            {
                returnText.Text = authorBox.Text + " not found in database";
            }

            else 
            {
                returnText.Text = authorBox.Text + " found in database";
            }

            addButton.Visibility = Visibility.Visible;
        }

        private void searchBook()
        {
            throw new NotImplementedException();
        }



        internal void createBook() {

            conn = new MySqlConnection(DatabaseManager.connString);
            bool valid = true;

            foreach (TextBox box in boxes.Children.OfType<TextBox>())
            {
                box.Text = box.Text.Trim();

                if (box.Text == "")
                {
                    box.Background = Brushes.Red;
                    valid = true;
                }
                else
                {
                    box.Background = Brushes.White;
                }
            }

            if (!valid)
            {
                returnText.Text = "Complete missing data in marked fields.";
                return;
            }

            string author = authorBox.Text;
            string nation = nationBox.Text;
            string title = titleBox.Text;
            int pages = Convert.ToInt32(pagesBox.Text);
            int series = Convert.ToInt32(seriesBox.Text);
            int year_written = Convert.ToInt32(yearBox.Text);
            string language = languageBox.Text;
            string genre = genreBox.Text;

            string query = $"CALL create_book('{author}','{nation}','{title}','{pages}','{series}','{year_written}', '{language}', '{location}', '{genre}');";
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

            returnText.Text = "Book added to " + location.ToString();
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

        private void pile_Checked(object sender, RoutedEventArgs e)
        {
            location = "Priority Pile";
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            createBook();
        }

        private void authorButton_Click(object sender, RoutedEventArgs e)
        {
            searchAuthor();
        }

        private void bookButton_Click(object sender, RoutedEventArgs e)
        {
            searchBook();
        }
    }
}

