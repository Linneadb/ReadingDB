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

namespace MyLibrary
{
    /// <summary>
    /// Interaction logic for AddBook.xaml
    /// </summary>
    public partial class AddBook : Window
    {
        private MainWindow mainWindow;
        private List bookList;

        public AddBook()
        {
            InitializeComponent();
        }

        public AddBook(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public AddBook(MainWindow mainWindow, List bookList)
        {
            this.mainWindow = mainWindow;
            this.bookList = bookList;
        }
    }
}
