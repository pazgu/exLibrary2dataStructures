using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exLibrary2dataStructures
{
    class DigitalBook : Book
    {
        public double fileSize { get; set; }
        public DigitalBook(string ISBNkey, double fileSize, string title, string bookAuthor, string bookGenre) : base( ISBNkey ,title, bookAuthor, bookGenre) //This constractor is for the library's inputs of digital books.
        {
            this.fileSize = fileSize;
        }

        public DigitalBook(string ISBNkey, string title, string bookAuthor, string bookGenre) : base(ISBNkey, title, bookAuthor, bookGenre) //This constractor is for the client's inputs (so he won't have to insert file size)
        {

        }

        public override string ToString()
        {

            //toLower function returns the digitalbook string in lowercase letters, so as the client's input (just for the values he's checking).
            return "ISBN: " + this.ISBNkey + ", of Book: " + this.title.ToLower() + ", from type: " + this.GetType().ToString().Split('.')[1] + ", book's author: " + this.bookAuthor.ToLower() + ", from genre: " + this.bookGenre + " and the file size: " + this.fileSize;
        }
        // this.GetType().ToString().Split('.')[1] so it returns obly the book type without the library object 
    }
}