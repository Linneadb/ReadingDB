using System;
using System.Collections;
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


        string Author;
        string Nationality;
        string Language;
        string Location;
        string Genre;

        public int id { get { return Id; } set { id = Id; } }
        public string title { get { return Title; } set { Title = value; } }
        public int pages { get { return Pages; } set { Pages = value; } }
        public int series { get { return Series; } set { Series = value; } }

        public int year { get { return Year; } set { Year = value; } }


        public string author { get => Author; set => Author = value; }
        public string genre { get => Genre; set => Genre = value; }
        public string nationality { get => Nationality; set => Nationality = value; }
        public string language { get => Language; set => Language = value; }
        public string location { get => Location; set => Location = value; }

  
        public Book(int id, string title, int pages, int series, int year) {
            Id = id;
            Title = title;
            Pages = pages;
            Series = series;
            Year = year;
        }

        public Book(string title, string author, string genre)
        {
            Title = title;
            Author = author;
            Genre = genre;
        }

        public Book(string author, string nationality) 
        {
            Author = author;
            Nationality = nationality;
        }

        public Book(string author, string nationality, string title, int pages, int series, int year, string language, string location, string genre)
        {
            Author = author;
            Nationality = nationality;
            Title = title;
            Pages=pages;
            Series = series;
            Year=year;
            Language = language;
            Location = location;
            Genre = genre;
        }

        public Book(string title, int pages, int series, int year_written, string language, string location)
        {
            this.title = title;
            this.pages = pages;
            this.series = series;
            this.language = language;
            this.location = location;
        }

        public static List<Book> bookList = new List<Book>();
        public static List<Book> authorList = new List<Book>();
        public static List<Book> wishList = new List<Book>();

    }
}
