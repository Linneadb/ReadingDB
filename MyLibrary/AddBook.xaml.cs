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
        string windowkey;
        string location = "";
        MySqlConnection conn;

        public AddBook(MainWindow mainWindow, string windowkey)
        {
            this.mainWindow = mainWindow;
            this.windowkey = windowkey;

            InitializeComponent();
            loadPage(windowkey);

        }

        internal void loadPage(string windowkey)
        {
            switch (windowkey)
            {
                case "Bookshelf":
                    location = "Bookshelf";
                    header.Text = "Add book to Library";
                    bookButton.Visibility = Visibility.Hidden;
                    pile.Visibility = Visibility.Visible;
                    break;

                case "Wishlist":
                    header.Text = "Add book to Wishlist";
                    location = "Wishlist";
                    bookButton.Visibility = Visibility.Hidden;
                    break;

                case "searchAuthor":
                    header.Text = "Search author name";
                    addButton.Visibility = Visibility.Hidden;
                    pile.Visibility = Visibility.Hidden;
                    bookButton.Visibility = Visibility.Hidden;
                    break;

                case "searchBook":
                    header.Text = "Search book title";
                    addButton.Visibility = Visibility.Hidden;
                    pile.Visibility = Visibility.Hidden;
                    authorButton.Visibility = Visibility.Hidden;
                    break;

                case "afterDelete":
                    header.Text = "Search or add new book";
                    location = "";
                    addButton.Visibility = Visibility.Hidden;
                    pile.Visibility = Visibility.Hidden;
                    bookButton.Visibility = Visibility.Visible;
                    authorButton.Visibility = Visibility.Visible;
                    addButton.Visibility = Visibility.Hidden;
                    break;

                case "authorFound":

                    addButton.Visibility = Visibility.Visible;
                    pile.Visibility = Visibility.Visible;
                    wishlistCheck.Visibility = Visibility.Visible;
                    break;

                default:
                    header.Text = "You have no matching windowkey";
                    break;
            }
        }

        internal void searchAuthor()
        {

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
                    int authorId = Convert.ToInt32(reader["author_id"]);
                    string author = reader["authors_name"].ToString();
                    string nationality = reader["authors_nationality"].ToString();

                    Book.authorList.Add(new Book(authorId, author, nationality));

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

            if (Book.authorList.Count == 0)
            {
                returnText.Text = titleBox.Text + " not found in database";
                foreach (TextBox box in boxes.Children.OfType<TextBox>())
                {
                    box.Clear();
                }
            }
            else
            {
                returnText.Text = authorBox.Text + " found in database";
                loadPage("authorFound");
            }
        }

        private void searchBook()
        {
            string keyword;
            conn = new MySqlConnection(DatabaseManager.connString);

            if (titleBox.Text != "")
            {
                keyword = titleBox.Text.Trim();
            }
            else
            {
                returnText.Text = "You have not entered a book title to search for.";
                return;
            }

            string query = $"CALL search_book('{keyword}');";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            try
            {
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                Book.bookList.Clear();

                while (reader.Read())
                {
                    int bookId = Convert.ToInt32(reader["books_id"]);
                    string author = reader["authors_name"].ToString();
                    string nation = reader["authors_nationality"].ToString();
                    string title = reader["books_title"].ToString();
                    int pages = Convert.ToInt32(reader["books_nr_pages"]);
                    int series = Convert.ToInt32(reader["books_nr_in_series"]);
                    int year_written = Convert.ToInt32(reader["books_year_written"]);
                    string language = reader["languages_languages"].ToString();
                    string genre = reader["genres_genres"].ToString();
                    string location = reader["locations_locations"].ToString();

                    Book.bookList.Add(new Book(bookId, author, nation, title, pages, series, year_written, language, location, genre));

                    if (Book.bookList.Count > 1)
                    {
                        //TODO: nextButton.Visability = Visibility.Visible;
                    }

                    authorBox.Text = author;
                    nationBox.Text = nation;
                    titleBox.Text = title;
                    pagesBox.Text = pages.ToString();
                    seriesBox.Text = series.ToString();
                    yearBox.Text = year_written.ToString();
                    languageBox.Text = language;
                    genreBox.Text = genre;
                }

                reader.Close();
                conn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            if (Book.bookList.Count == 0)
            {
                returnText.Text = titleBox.Text + " not found in database";
                foreach (TextBox box in boxes.Children.OfType<TextBox>())
                {
                    box.Clear();
                }
            }

            else
            {
                returnText.Text = titleBox.Text + " found in database";
            }

            editButton.Visibility = Visibility.Visible;
            deleteButton.Visibility = Visibility.Visible;
        }

        internal void createBook()
        {

            conn = new MySqlConnection(DatabaseManager.connString);

            bool valid = false;
            //validateTextboxes();
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

            if (location == "")
            {
                location = "Bookshelf";
            }

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

        internal void updateBook(Book book)
        {
            conn = new MySqlConnection(DatabaseManager.connString);
            bool valid = false; 
                //validateTextboxes();
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

            string bookId = book.bookId.ToString();
            string author = authorBox.Text;
            string nation = nationBox.Text;
            string title = titleBox.Text;
            int pages = Convert.ToInt32(pagesBox.Text);
            int series = Convert.ToInt32(seriesBox.Text);
            int year_written = Convert.ToInt32(yearBox.Text);
            string language = languageBox.Text;
            string genre = genreBox.Text;

            if (location == "")
            {
                location = "Bookshelf";
            }

            string query = $"CALL update_book('{bookId}','{author}','{nation}','{title}','{pages}','{series}','{year_written}', '{language}', '{location}', '{genre}');";
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

            returnText.Text = "Book is updated in database";
            foreach (TextBox box in boxes.Children.OfType<TextBox>())
            {
                box.Clear();
                returnText.Text = "";
            }
        }

        private void deleteBook(Book book)
        {
            conn = new MySqlConnection(DatabaseManager.connString);
            bool valid = false;
            //validateTextboxes();
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

            string bookId = book.bookId.ToString();


            string query = $"CALL delete_book('{bookId}');";
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

            returnText.Text = "Book is deleted from database";
            foreach (TextBox box in boxes.Children.OfType<TextBox>())
            {
                box.Clear();
                returnText.Text = "";
            }

            loadPage("afterDelete");
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

        private void wishlistCheck_Checked(object sender, RoutedEventArgs e)
        {
            location = "Wishlist";
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

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (Book.bookList.Count > 0)
            {
                updateBook(Book.bookList[0]);
            }
            else
                returnText.Text = "Search for a book to edit";
        }
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            deleteBook(Book.bookList[0]);
        }

       /* private bool validateTextboxes()
        {
            foreach (TextBox box in boxes.Children.OfType<TextBox>())
            {
                box.Text = box.Text.Trim();

                if (box.Text == "")
                {
                    box.Background = Brushes.Red;
                    return true;
                }
                else
                {
                    box.Background = Brushes.White;
                    return false;
                }
            }
        }*/
    }
}

