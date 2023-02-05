using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exLibrary2dataStructures
{
    class PaperBook : Book
    {
        private int copiesNumber;
        public PaperBook(string ISBNkey, int copiesNumber, string title, string bookAuthor, string bookGenre) : base(ISBNkey, title, bookAuthor, bookGenre) //This constractor is for the library's inputs of paper books.
        {
            this.copiesNumber = copiesNumber;
        }
        public PaperBook(string ISBNkey, string title, string bookAuthor, string bookGenre) : base(ISBNkey, title, bookAuthor, bookGenre) //This constractor is for the client's inputs (so he won't have to insert number of copies)
        {

        }

        public int getCopiesNumber()
        {
            return this.copiesNumber;
        }

        public void setCopiesNumber(int copiesNumber)
        {
            this.copiesNumber = copiesNumber;
        }

        public void increaseCopiesNumber()
        {
            this.copiesNumber++;
        }
        public void decreaseCopiesNumber()
        {
            this.copiesNumber--;
        }
        public override string ToString()
        {
            //toLower function returns the paperbook string in lowercase letters, so as the client's input (just for the values he's checking).
            return "ISBN: " + this.ISBNkey + ", of Book: " + this.title.ToLower() + ", from type: " + this.GetType().ToString().Split('.')[1] + ", book's author: " + this.bookAuthor.ToLower() + ", from genre: " + this.bookGenre + " and number of available copies: " + this.copiesNumber;
        }
    }// this.GetType().ToString().Split('.')[1] so it returns only the book type without the library object 
}
