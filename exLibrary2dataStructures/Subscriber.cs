using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exLibrary2dataStructures
{
    class Subscriber
    {
        private string id;
        private string firstName;
        private string lastName;
        public List<Book> subscriberBooks { get; set; }
        private int numberOfLoanPaperBooks;

        public Subscriber(string id, string firstName, string lastName, List<Book> subscriberBooks)
        {
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.subscriberBooks = subscriberBooks;
            this.numberOfLoanPaperBooks = 0; //for the function of loaning books. Each subscriber starts with 0 books. 
        }

        public Subscriber(string id ,string firstName, string lastName)
        {
            this.id=id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.numberOfLoanPaperBooks = 0;
        }

        public Subscriber(string id) //for PrintListOfSubscriberBooks function 
        {
            this.id=id;  
        }
        public string getId()
        {
            return this.id;
        }
        public void setId(string id) // Key has to be 9 digits, so there won't be an option for the client to set the id. 
        {
            if (!string.IsNullOrEmpty(id) && (id.Length ==9))
                this.id = id;
            else
                throw new Exception ("Id must contain 9 digit.");
        }
        public string getFirstName()
        {
            return this.firstName;
        }
        public void setFirstName(string firstName)
        {
            this.firstName = firstName;
        }
        public string getLastName()
        {
            return this.lastName;
        }
        public void setLastName(string lastName)
        {
            this.lastName = lastName;
        }
        public int getNumberOfLoanPaperBooks() 
        {
            return numberOfLoanPaperBooks;
        }
        public void setNumberOfLoanPaperBooks(int numberOfLoanPaperBooks) //So that the number of loan books will be limited to 3 books max.
        {           
            if ((numberOfLoanPaperBooks >= 0) && (numberOfLoanPaperBooks <= 3))
            {
                this.numberOfLoanPaperBooks = numberOfLoanPaperBooks;
            }
            else
            {
                throw new Exception("Can't loan more than 3 books.");
            }
        }

        public override string ToString()
        {
            return this.firstName + ", " + this.lastName;
        }
    }
}
