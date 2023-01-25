using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;

namespace MyLibrary
{
    internal class Book
    {
        int Id;
        string Title;
        int Pages;
        int Series;
        int Year;

        public int id { get { return Id; } set { id = Id; } }
        public string title { get { return Title; } set { Title = value; } }
        public int pages { get { return Pages; } set { Pages = value; } }
        public int series { get { return Series; } set { Series = value; } }

        public int year { get { return Year; } set { Year = value; } }

        public Book(int id, string title, int pages, int series, int year) {
            Id = id;
            Title = title;
            Pages = pages;
            Series = series;
            Year = year;
        }

    public static List<Book> books = new List<Book>();

        /*
        string author;
        public string Author { get { return author; } set { author = value; } }
        */


    }
}
