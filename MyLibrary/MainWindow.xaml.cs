using MySql.Data.MySqlClient;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
using System.Xml.Linq;

namespace MyLibrary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MySqlConnection conn;
        String query = "";
        TextBox[] txtBooks;
        TextBox[] txtAuthors;
        public MainWindow()
        {
            InitializeComponent();
            establishDBConnection();
            //selectFromDB("select * from books");


           //is data loading?
            txtBooks = new TextBox[] { txtTitle, txtPages, txtAuthor };
            txtAuthors = new TextBox[] { txtName };
        }

        
        internal void establishDBConnection()
        {
            conn = new MySqlConnection(DatabaseManager.connString);

           //verify DB connection success
           try
           {
               conn.Open();
               MessageBox.Show("Connection Open!"); 
               conn.Close();
           }
           catch (Exception ex)
           {
               MessageBox.Show("Can not open connection!" + ex.Message);
           }
        }
       

        private void selectFromDB(string query)
        {

            //string query2 = $"CALL add_book('{books_title}', {}, '{}');";

            MySqlCommand cmd = new MySqlCommand(query, conn);

            try
            {
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                //Skriva till Grid
                //DataTable dataTable = new DataTable();
                //dataTable.Load(reader);
                //gridOutput.DataSource = dataTable;

                //Tömma output label
                //lblSelectOutput.Text = "";

                //empty persons list / static list in People class
                //People.persons.Clear();

                //For some reason the reader needs to be reconnected
                //reader = cmd.ExecuteReader();

                //While Loop för att skriva ut hämtad data
                while (reader.Read())
                {
                    //Hämta specifik data från Reader objekt

                    int id = Convert.ToInt32(reader["books_id"]);
                    string title = reader["books_title"].ToString();
                    int pages = Convert.ToInt32(reader["books_nr_pages"]);
                    int series = Convert.ToInt32(reader["books_nr_in_series"]);
                    int year = Convert.ToInt32(reader["books_year_written"]);
                    
                    int author = Convert.ToInt32(reader["authors_authors_id"]);

                    txtTitle.Text = title;
                    txtPages.Text = pages.ToString();
                    txtAuthor.Text = author.ToString();

                    //Skriva ut värden till label
                    //lblSelectOutput.Text += $"{name} är {age} år gammal. Husdjuret heter {petName}{Environment.NewLine}";

                    //add an instance of Books and save to static list
                    Book.books.Add(new Book(id, title, pages, series, year));

                    /*foreach (Book book in Book.bookList)
                    {
                        //Söker efter rätt post i listan
                        if (person.Id == id)
                        {
                            //Hittat rätt person

                            //Hämta properties från person och skriv in dem i textfält


                        }*/
                    reader.Close();
                    conn.Close();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void addBook_Click(object sender, RoutedEventArgs e)
        {
            AddBook addBook = new AddBook(this);
            addBook.Show();
            this.Hide();
        }

        private void addWishlist_Click(object sender, RoutedEventArgs e)
        {
            AddBook addBook = new AddBook(this, bookList);
            addBook.Show();
            this.Hide();
        }

        bool changingLocation = false;  // Prevent row change
        private void booksLocationsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changingLocation = true;
            if (booksLocationsComboBox.SelectedValue != null)
            {
                DataTable booksTable = (DataTable)((DataSourceProvider)FindResource("BooksTable")).Data;
                string value = (string)booksLocationsComboBox.SelectedValue;
                if (value == "Wishlist") {
                    DatabaseTable.GetTable($"CALL get_wishlist();", booksTable);
                }
                if (value == "Library") {
                    DatabaseTable.GetTable($"CALL get_library();", booksTable);
                }
           
            }
            changingLocation = false;
        }

        private void booksDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //not implemented
        }

       
    }
}
