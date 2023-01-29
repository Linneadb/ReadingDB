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
        int selectedBookId;

        public MainWindow()
        {
            InitializeComponent();

            booksTable = (DataTable)((DataSourceProvider)FindResource("BooksTable")).Data;
        
            establishDBConnection();
            showPriorityList();
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
            selectedBookId = (int)((DataRowView)booksDataGrid.SelectedItem).Row[0];
            edit.Visibility = Visibility.Visible;
        }

        

   /*     private void deleteButton_Click(object sender, RoutedEventArgs e)
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
        } */

        private void showPriorityList()
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
            showPriorityList();
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            AddBook addBook = new AddBook(this, "editBook", selectedBookId);
            addBook.Show();
            this.Hide();
        }
    }
}
