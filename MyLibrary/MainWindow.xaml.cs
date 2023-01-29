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

            

            booksTable = (DataTable)((DataSourceProvider)FindResource("BooksTable")).Data;
            //booksTable.RowChanged += new DataRowChangeEventHandler(booksTable_RowChanged);
            //booksTable.RowChanging += new DataRowChangeEventHandler(booksTable_RowChanging);
 

            establishDBConnection();
            showWishlist();
        }

        private void booksTable_RowChanging(object sender, DataRowChangeEventArgs e)
        {
            //
        }

        internal void establishDBConnection()
        {
            MySqlConnection conn = new MySqlConnection(DatabaseManager.connString);

           try
           {
               conn.Open();
               //MessageBox.Show("Connection Open!"); 
               conn.Close();
           }
           catch (Exception ex)
           {
               MessageBox.Show("Can not open connection!" + ex.Message);
           }
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
            //MessageBox.Show("Selection changed event triggered");
            string title = booksDataGrid.SelectedItem.ToString();
            MessageBox.Show(title + "booktitel");
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
                
                query = "UPDATE books SET books_title=?Title, books_pages=?Pages, books_nr_series=?Nr, books_year_written=?Year WHERE 
                  "Key_Bookstable=?oldKey_Bookstable";
                MySqlCommand updateCommand = new MySqlCommand(query, conn);
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

        private void showWishlist()
        {
            MySqlConnection conn = new MySqlConnection(DatabaseManager.connString);
            query = "SELECT * FROM mylibrary.priority_pile;";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            try
            {
                conn.Open();

                MySqlDataReader reader = cmd.ExecuteReader();
                Book.priorityList.Clear();
                while (reader.Read())
                {
                    string title = reader["books_title"].ToString();
                    string author = reader["authors_name"].ToString();
                    string genre = reader["genres"].ToString();

                    //create instance of book and save to list
                    Book.priorityList.Add(new Book(title, author, genre));

                    //string to print to label
                    lblprioList.Content += $"{title} by {author} \nGenre:{genre}\n{Environment.NewLine}";
                }
                
                reader.Close();
                conn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
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

        private void searchBook_Click(object sender, RoutedEventArgs e)
        {
            AddBook addBook = new AddBook(this, "searchBook");
            addBook.Show();
            this.Hide();
        }

        private void searchAuthor_Click(object sender, RoutedEventArgs e)
        {
            AddBook addBook = new AddBook(this, "searchAuthor");
            addBook.Show();
            this.Hide();
        }

        private void updateLabel_Click(object sender, RoutedEventArgs e)
        {
            showWishlist();
        }
    }
}
