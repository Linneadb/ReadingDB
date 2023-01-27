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
        DataTable booksTable;
        String query = "";

        public MainWindow()
        {
            InitializeComponent();

            

            //booksTable = (DataTable)((DataSourceProvider)FindResource("BooksTable")).Data;
            //booksTable.RowChanged += new DataRowChangeEventHandler(booksTable_RowChanged);
            //booksTable.RowChanging += new DataRowChangeEventHandler(booksTable_RowChanging);
 

            establishDBConnection();
            showWishlist();

            //is data loading?
            //txtBooks = new TextBox[] { txtTitle, txtPages, txtAuthor };
            //txtAuthors = new TextBox[] { txtName };
        }

        private void booksTable_RowChanging(object sender, DataRowChangeEventArgs e)
        {
            //
        }

        internal void establishDBConnection()
        {
            MySqlConnection conn = new MySqlConnection(DatabaseManager.connString);

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
       
        /*
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
                    /*
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



                                        reader.Close();
                                        conn.Close();
                                    }
                                }

                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            } }*/

        private void addBook_Click(object sender, RoutedEventArgs e)
        {
            AddBook addBook = new AddBook(this, "Bookshelf");
            addBook.Show();
            this.Hide();
        }

        private void addWishlist_Click(object sender, RoutedEventArgs e)
        {
            AddBook addBook = new AddBook(this, "Wishlist");
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
            MessageBox.Show("Selection changed event triggered");
        }

        void booksTable_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (!changingLocation)
            {
                MessageBox.Show("Row changed event triggered");
                //UpdateDBbooks();
            }
        }

        /*
        private void UpdateDBBooks()
        {
            conn = new MySqlConnection(DatabaseManager.connString);

            try
            {
                conn.Open();
                MessageBox.Show("Connection Open!");
                
                string updateString = $"CALL update_books();"
                $"CALL updatePeople({id}, '{name}', {age}, '{petName}');";
                string updateString = "UPDATE issues SET IssueTitle=?IssueTitle, Title=?Title, Volume=?Volume, Number=?Number, " +
                  "IssueDay=?IssueDay, IssueMonth=?IssueMonth, IssueYear=?IssueYear, ComicVine=?ComicVine WHERE " +
                  "Key_Issues=?oldKey_Issues";
                MySqlCommand updateCommand = new MySqlCommand(updateString, conn);
                updateCommand.Parameters.Add("?IssueTitle", MySqlDbType.VarChar, 100, "IssueTitle");
                updateCommand.Parameters.Add("?Title", MySqlDbType.Int32, 10, "Title");
                updateCommand.Parameters.Add("?Volume", MySqlDbType.Int32, 10, "Volume");
                updateCommand.Parameters.Add("?Number", MySqlDbType.VarChar, 10, "Number");
                updateCommand.Parameters.Add("?IssueDay", MySqlDbType.Int32, 10, "IssueDay");
                updateCommand.Parameters.Add("?IssueMonth", MySqlDbType.Int32, 10, "IssueMonth");
                updateCommand.Parameters.Add("?IssueYear", MySqlDbType.Int32, 10, "IssueYear");
                updateCommand.Parameters.Add("?ComicVine", MySqlDbType.VarChar, 100, "ComicVine");
                MySqlParameter parameter = updateCommand.Parameters.Add("?oldKey_Issues", MySqlDbType.Int32, 10, "Key_Issues");
                parameter.SourceVersion = DataRowVersion.Original;
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.UpdateCommand = updateCommand;
                

                query = $"CALL insert_books('{title}', '{name}', {age}, '{petName}' );"
                //string insertString = "INSERT INTO issues (Key_Issues, IssueTitle, Title, Volume, Number, IssueDay, IssueMonth, IssueYear, ComicVine) " +
                  "VALUES (?Key_Issues, ?IssueTitle, " + booksTitlesComboBox.SelectedValue + ", ?Volume, ?Number, ?IssueDay, ?IssueMonth, ?IssueYear, ?ComicVine)";
                MySqlCommand insertCommand = new MySqlCommand(insertString, conn);
                insertCommand.Parameters.Add("?Key_Issues", MySqlDbType.Int32, 10, "Key_Issues");
                insertCommand.Parameters.Add("?IssueTitle", MySqlDbType.VarChar, 100, "IssueTitle");
                insertCommand.Parameters.Add("?Volume", MySqlDbType.Int32, 10, "Volume");
                insertCommand.Parameters.Add("?Number", MySqlDbType.VarChar, 10, "Number");
                insertCommand.Parameters.Add("?IssueDay", MySqlDbType.Int32, 10, "IssueDay");
                insertCommand.Parameters.Add("?IssueMonth", MySqlDbType.Int32, 10, "IssueMonth");
                insertCommand.Parameters.Add("?IssueYear", MySqlDbType.Int32, 10, "IssueYear");
                insertCommand.Parameters.Add("?ComicVine", MySqlDbType.VarChar, 100, "ComicVine");
                adapter.InsertCommand = insertCommand;

                MySqlCommand deleteCommand = new MySqlCommand("DELETE FROM issues WHERE Key_Issues=?Key_Issues", conn);
                MySqlParameter delParameter = deleteCommand.Parameters.Add("?Key_Issues", MySqlDbType.Int32, 10, "Key_Issues");
                delParameter.SourceVersion = DataRowVersion.Original;
                adapter.DeleteCommand = deleteCommand;
                adapter.Update(booksTable);

                conn.Close();
                }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection!" + ex.Message);
            }
        }

        */

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (booksDataGrid.SelectedItems.Count == 0) return;
            IList items = booksDataGrid.SelectedItems;
            IEnumerable<DataRowView> toDelete = items.Cast<DataRowView>();
            try
            {
                List<DataRowView> rows = new List<DataRowView>(toDelete);
                foreach (DataRowView row in rows)
                {
                    row.Row.Delete();
                }
            }
            catch (InvalidCastException)
            {
                return;
            }
        }

        private void showWishlist(string keyword = "")
        {
            
            MySqlConnection connection = new MySqlConnection(DatabaseManager.connString);
            
            // Kontrollera om keyword har ett inkommade värde
            if (keyword == "")
            {
                //Skriv query för att hämta wishlist view
                query = "SELECT * FROM mylibrary.wishlist;";
            }
            else
            {
                //Query för att söka på spcifikt namn
                query = $"CALL searchName('{keyword}');";
            }

            //Skapar ett MySQLCommand objekt
            MySqlCommand command = new MySqlCommand(query, connection);

            //Try/Catch block
            try
            {
                //Öppna koppling till DB
                connection.Open();

                //Exekvera SQL querry
                MySqlDataReader reader2 = command.ExecuteReader();
                
                //Tömma wishList 
                Book.wishList.Clear();

                //Exekvera SQL query
                reader2 = command.ExecuteReader();

                //While Loop för att skriva ut hämtad data
                while (reader2.Read())
                {
                    //Hämta specifik data från Reader objekt
                    string title = reader2["books_title"].ToString();
                    string author = reader2["authors_author"].ToString();
                    string genre = reader2["genres_genres"].ToString();

                    //Skapa ett nytt objekt av People och sparar det i statisk lista
                    Book.wishList.Add(new Book(title, author, genre));

                        //Skriva ut värden till label
                        lblwishList.Content += $"{title} by {author} Genre:{genre}{Environment.NewLine}";
                }
                
                reader2.Close();
                connection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


    }
}
