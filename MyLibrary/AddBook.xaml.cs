﻿using MySql.Data.MySqlClient;
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
        }

        internal void searchAuthor() {
            string keyword = authorBox.Text;
            
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

            //Hämta personer från DB
            //selectFromDB();

            //Markera sista raden
            //gridOutput.Rows[gridOutput.Rows.Count - 2].Selected = true;

            //Hämta data
            //getSelectedRow();

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

        }

        private void bookButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

