using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exLibrary2dataStructures
{
    class Book
    {
        protected string ISBNkey;
        protected string title;
        protected string bookAuthor;
        protected string bookGenre;

        public Book(string ISBNkey, string title, string bookAuthor, string bookGenre)
        {
            this.ISBNkey = ISBNkey;
            this.title = title;
            this.bookAuthor = bookAuthor;
            this.bookGenre = bookGenre;

        }
        public Book() { }
        public Book(string ISBNkey, string title, string bookAuthor)
        {
            this.ISBNkey= ISBNkey;
            this.title = title;
            this.bookAuthor = bookAuthor;
        }
        public string getISBNkey()
        {
            return this.ISBNkey;
        }

        public void setISBNkey(string ISBNkey) // Key has to be 5 digits max 
        {
            if ((this.ISBNkey.Length <= 5) && (this.ISBNkey.Length >= 1))
                this.ISBNkey = ISBNkey;
            else
                throw new Exception("ISBN must contain 5 digits only.");
        }
        public string getTitle()
        {
            return this.title;
        }

        public void setTitle(string title)
        {
            this.title = title;
        }

        public string getbookAuthor()
        {
            return this.bookAuthor;
        }

        public void setbookAuthor(string bookAuthor)
        {
            this.bookAuthor = bookAuthor;
        }

        public string getbookGenre()
        {
            return this.bookGenre;
        }

        public void setbookGenre(string bookGenre)
        {
            this.bookGenre = bookGenre;
        }

        public override string ToString()
        {
            return this.ISBNkey + ", " + this.title + ", " + this.bookAuthor + ", " + this.bookGenre;
        }
    }
}
