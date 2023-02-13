using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace exLibrary2dataStructures
{
    class Library
    {
        public Dictionary<string, Book> books;
        public Dictionary<string, Subscriber> subscribers;
        private string _connectionString;

        public Library(Dictionary<string, Book> books, Dictionary<string, Subscriber> subscribers, string connectionString)
        {
            this.books = books;
            this.subscribers = subscribers;
            this._connectionString = connectionString; //It will get the current connection from the Main.cs page
        }
        public Library(string connectionString)
        {
            _connectionString = connectionString; //It will get the current connection from the Main.cs page
        }
        public void AddBook() //Add a new book to the library's collection
        {
            //for each function I inserted the data to the db after I refer to the exceptions and validations needed, so the data should be fitting.
            while (true) //to allow multiple attempts
            {
                try
                {
                    if (books == null)
                    {
                        Console.WriteLine("Books dictionary is null.");
                        return;
                    }
                    Console.WriteLine("Please enter the book's ISBN: ");
                    string bookISBN = Console.ReadLine();
                    bool isBookISBN = int.TryParse(bookISBN, out _);
                    if ((!isBookISBN) || (bookISBN == "") || (bookISBN.Length > 5))
                    {
                        throw new Exception("ISBN is not valid. Please check your input is 5 digits max.");
                    }
                    Console.WriteLine("Please enter the book title: ");
                    string bookTitle = Console.ReadLine();
                    //bookTitle variable can contain digits alongside charcters and also can contain digits only.
                    bool isTitleNull = string.IsNullOrWhiteSpace(bookTitle);
                    if (isTitleNull)
                    {
                        throw new Exception("Please fill in the title field.");
                    }
                    Console.WriteLine("Please enter the book's author name: ");
                    string bookAuthor = Console.ReadLine();
                    bool isBookAuthorNumeric = bookAuthor.Any(char.IsDigit); //So the author's name won't contain numbers
                    bool isAuthorNull = string.IsNullOrWhiteSpace(bookAuthor);
                    if ((isBookAuthorNumeric) || (isAuthorNull))
                    {
                        throw new Exception("Author's name can't contain digits or be empty.");
                    }
                    Console.WriteLine("Please enter the book genre name: ");
                    string bookGenre = Console.ReadLine();
                    bool isBookGenreNumeric = bookGenre.Any(char.IsDigit);
                    bool isGenreNull = string.IsNullOrWhiteSpace(bookGenre);
                    if ((isBookGenreNumeric) || (isGenreNull))
                    {
                        throw new Exception("Genre's name can't contain digits or be empty.");
                    }
                    Console.WriteLine("Please enter the book type (D - Digital, P - Paper)");
                    string bookType = Console.ReadLine().ToUpper(); //So the client could type d or p in small letters too.
                    if (bookType == "D")
                    {
                        if (books.ContainsKey(bookISBN))
                        {
                            if ((string.Compare(bookTitle, books[bookISBN].getTitle(), StringComparison.CurrentCultureIgnoreCase) != 0) || (string.Compare(bookAuthor, books[bookISBN].getbookAuthor(), StringComparison.CurrentCultureIgnoreCase) != 0) ||
                                (string.Compare(bookGenre, books[bookISBN].getbookGenre(), StringComparison.CurrentCultureIgnoreCase) != 0)) //In case the title, author or genre don't match the isbn number, consider insensative case manner
                            {
                                throw new Exception("\nNeither do title, author nor genre match the ones associated with this ISBN number. This ISBN belongs to another book");
                            }
                            if (books[bookISBN] is DigitalBook)
                            {
                                Console.WriteLine("\nBook is already exists.");
                                return;
                            }
                            if (books[bookISBN] is PaperBook) //So it won't be an option to add book that already exists, even if is a paper book 
                            {
                                Console.WriteLine("\nBook is already exists and it has paper version.");
                                return;
                            }
                        }
                        else
                        {
                            double fileSize = 250;
                            Book newDigitalBook = new DigitalBook(bookISBN, fileSize, bookTitle, bookAuthor, bookGenre);
                            books.Add(bookISBN, newDigitalBook); //book has been inserted to the dictionary
                            using (SqlConnection connection = new SqlConnection(_connectionString)) //now it would be inserted to the database
                            {
                                connection.Open();
                                string insertNewDigitalBook = "INSERT INTO Books (ISBN_key, bookType, bookTitle, author, genre, fileSize, copiesNumber) VALUES (@bookISBN, 'Digital', @bookTitle, @bookAuthor, @bookGenre, @fileSize, null)";
                                using (SqlCommand insertCommand = new SqlCommand(insertNewDigitalBook, connection))
                                {
                                    // Add parameters for each variable in the INSERT statement
                                    insertCommand.Parameters.AddWithValue("@bookISBN", newDigitalBook.getISBNkey());
                                    insertCommand.Parameters.AddWithValue("@bookTitle", newDigitalBook.getTitle());
                                    insertCommand.Parameters.AddWithValue("@bookAuthor", newDigitalBook.getbookAuthor());
                                    insertCommand.Parameters.AddWithValue("@bookGenre", newDigitalBook.getbookGenre());
                                    insertCommand.Parameters.AddWithValue("@fileSize", 250); //each new book gets fixed value
                                    // I used this way of sql query so it be more readable and more reuseable (reapeting pattern)
                                    insertCommand.ExecuteNonQuery();
                                }
                                connection.Close();
                            }              
                            Console.WriteLine("\nSuccess. New Digital book is added.");
                            return;
                        }
                    }
                    if (bookType == "P")
                    {
                        if (books.ContainsKey(bookISBN))
                        {
                            if ((string.Compare(bookTitle, books[bookISBN].getTitle(), StringComparison.CurrentCultureIgnoreCase) != 0) || (string.Compare(bookAuthor, books[bookISBN].getbookAuthor(), StringComparison.CurrentCultureIgnoreCase) != 0) ||
                               (string.Compare(bookGenre, books[bookISBN].getbookGenre(), StringComparison.CurrentCultureIgnoreCase) != 0))
                            {
                                throw new Exception("\n Neither do title, author nor genre match the ones associated with this ISBN number.  This ISBN belongs to another book");
                            }
                            if (books[bookISBN] is PaperBook)
                            {
                                ((PaperBook)books[bookISBN]).increaseCopiesNumber();
                                Console.WriteLine("\nBook is already exists.");
                                return;
                            }
                            if (books[bookISBN] is DigitalBook) //So it won't be an option to add book that already exists, even if is a digital book 
                            {
                                Console.WriteLine("\nBook is already exists and it has digital version.");
                                return;
                            }
                        }
                        else
                        {
                            int numberOfCopies = 1;
                            Book newPaperBook = new PaperBook(bookISBN, numberOfCopies, bookTitle, bookAuthor, bookGenre);
                            books.Add(bookISBN, newPaperBook);
                            using (SqlConnection connection = new SqlConnection(_connectionString))
                            {
                                connection.Open();
                                string insertNewPaperBook = "INSERT INTO Books (ISBN_key, bookType, bookTitle, author, genre, fileSize, copiesNumber) VALUES (@bookISBN, 'Paper', @bookTitle, @bookAuthor, @bookGenre, null, @numberOfCopies)";
                                using (SqlCommand insertCommand = new SqlCommand(insertNewPaperBook, connection))
                                {
                                    // Add parameters for each variable in the INSERT statement
                                    insertCommand.Parameters.AddWithValue("@bookISBN", newPaperBook.getISBNkey());
                                    insertCommand.Parameters.AddWithValue("@bookTitle", newPaperBook.getTitle());
                                    insertCommand.Parameters.AddWithValue("@bookAuthor", newPaperBook.getbookAuthor());
                                    insertCommand.Parameters.AddWithValue("@bookGenre", newPaperBook.getbookGenre());
                                    insertCommand.Parameters.AddWithValue("@numberOfCopies", 1); //each new book gets fixed value

                                    insertCommand.ExecuteNonQuery();
                                }
                                connection.Close();
                            }
                            Console.WriteLine("\nSuccess. New Paper book is added.");
                            return;
                        }
                    }
                    else // If the client type something that isn't d or p character
                    {
                        throw new Exception("\nInput is wrong. Please enter D for digital book or P for paper book.");
                    }
                }
                catch (NullReferenceException ex) //If there are no books in the library's dictionary 
                {
                    Console.WriteLine("An error occurred: The books dictionary is null.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("\nWhat would you like to do?\n1. Try again\n2. Go back to main menu\n"); //so client will had a choice and won't get stuck in the loop
                    //My guiding principle is that when I print messege to the console, it won't be throwen to here, but when I throw an exception, I want client will be able to choose
                    //whether he wants to try again or go out the function.  
                    if (!int.TryParse(Console.ReadLine(), out int userChoice) || (userChoice < 1) || (userChoice > 2))
                    {
                        Console.WriteLine("Invalid input. Please enter a valid choice next time.");
                        break;
                    }
                    if (userChoice == 1)
                    {
                        continue;
                    }
                    else if (userChoice == 2)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("No input received. Please try again.");
                        break;
                    }
                }
            }
        }

        public void AddSubscriber() //Add a new subscriber to the system
        {
            while (true)
            {
                Console.WriteLine("Please enter your id number: ");
                string id = Console.ReadLine();
                bool isIDint = int.TryParse(id, out _);
                try
                {
                    if (subscribers == null)
                    {
                        Console.WriteLine("Subscribers dictionary is null.");
                        return;
                    }
                    if ((!isIDint) || (id == "") || (id.Length != 9)) //Checks whether the ID input contains only digits
                    {
                        throw new Exception("\nInput is wrong. Please check you'd insert your id and that it contains only 9 digits.");
                    }             
                    Console.WriteLine("Please enter your first name: ");
                    string firstName = Console.ReadLine();
                    bool isFirstNameNumeric = firstName.Any(char.IsDigit); //Checking if input contains number or not.
                    Console.WriteLine("Please enter your last name: ");
                    string lastName = Console.ReadLine();
                    bool isLastNameNumeric = lastName.Any(char.IsDigit);
                    if ((!isFirstNameNumeric) && (!isLastNameNumeric) && (firstName != "") && (lastName != "")) //Continue just if the input doesn't contain digits and isn't empty.  
                    {
                        if (subscribers.ContainsKey(id))
                        {
                            if ((string.Compare(firstName, subscribers[id].getFirstName(), StringComparison.CurrentCultureIgnoreCase) != 0) || (string.Compare(lastName, subscribers[id].getLastName(), StringComparison.CurrentCultureIgnoreCase) != 0)) //In case the first and last name don't match the id, consider insensative case manner
                            {
                                throw new Exception("\n First and last names do not match the names associated with this ID number.");
                            }
                            else
                            {
                                Console.WriteLine("\nSubscriber already exists. Log in to start reading.");
                                return;
                            }
                        }
                        else
                        {
                            Subscriber subscriber = new Subscriber(id, firstName, lastName);
                            subscribers.Add(id, subscriber);
                            subscribers[id].subscriberBooks = new List<Book>(); //bulid for each subscriber new list
                            using (SqlConnection connection = new SqlConnection(_connectionString))
                            {
                                connection.Open();
                                string insertNewSubscriber = "INSERT INTO Subscribers (idNumber, firstName, lastName, firstBook_id, secondBook_id, thirdBook_id) VALUES (@id, @firstName, @lastName, null, null, null)";
                                using (SqlCommand insertCommand = new SqlCommand(insertNewSubscriber, connection))
                                {
                                    insertCommand.Parameters.AddWithValue("@id", subscriber.getId());
                                    insertCommand.Parameters.AddWithValue("@firstName", subscriber.getFirstName());
                                    insertCommand.Parameters.AddWithValue("@lastName", subscriber.getLastName());

                                    insertCommand.ExecuteNonQuery();
                                }
                                connection.Close();
                            }
                            Console.WriteLine("\nSuccess. You are now part of our system :).");
                        }
                        break;
                    }
                    else
                        throw new Exception ("\nInput can't be empty or contain digits. Please enter your first and last name again.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\n An error ocuur: " + ex.Message + " Please try again.");
                    Console.WriteLine("\nWhat would you like to do?\n1. Try again\n2. Go back to main menu\n"); //so client will had a choice and won't get stuck in the loop
                    if (!int.TryParse(Console.ReadLine(), out int userChoice) || (userChoice < 1) || (userChoice > 2))
                    {
                        Console.WriteLine("Invalid input. Please enter a valid choice next time.");
                        break;
                    }
                    if (userChoice == 1)
                    {
                        continue;
                    }
                    else if (userChoice == 2)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("No input received. Please try again.");
                        break;
                    }
                }
            }
        }

        public void DeleteSubscriber() //Delete subscriber's details from the system
        {
             while (true)
             {
                Console.WriteLine("To delete your account, please enter your ID number: ");
                string id = Console.ReadLine();
                bool isIDint = int.TryParse(id, out _);
                try
                {
                    if (subscribers == null)
                    {
                        Console.WriteLine("Subscribers dictionary is null.");
                        return;
                    }
                    if ((!isIDint) || (id == "") || (id.Length != 9)) 
                    {
                        throw new Exception("\nInput is wrong. Please check you'd insert your id and that it contains only 9 digits.");
                    }
                    Console.WriteLine("Please enter your first name: ");
                    string firstName = Console.ReadLine();
                    bool isFirstNameNumeric = firstName.Any(char.IsDigit); 
                    Console.WriteLine("Please enter your last name: ");
                    string lastName = Console.ReadLine();
                    bool isLastNameNumeric = lastName.Any(char.IsDigit);
                    if ((!isFirstNameNumeric) && (!isLastNameNumeric) && (firstName != "") && (lastName != ""))  
                    {
                        if (subscribers.ContainsKey(id))
                        {
                            if ((string.Compare(firstName, subscribers[id].getFirstName(), StringComparison.CurrentCultureIgnoreCase) != 0) || (string.Compare(lastName, subscribers[id].getLastName(), StringComparison.CurrentCultureIgnoreCase) != 0)) //In case the first and last name don't match the id, consider insensative case manner
                            {
                                throw new Exception("\n First and last names do not match the names associated with this ID number.");
                            }
                            PaperBook paperbook;
                            if (subscribers[id].subscriberBooks.Count > 0 ) //If subscriber loaned any book
                            {
                                foreach (Book item in subscribers[id].subscriberBooks)
                                {
                                    if (item is PaperBook)
                                    {
                                        paperbook = (PaperBook)item;
                                        paperbook.increaseCopiesNumber(); //Increase the number of copies as client is unsubscribed
                                        using (SqlConnection connection = new SqlConnection(_connectionString))
                                        {
                                            connection.Open();
                                            string updateBooks = "UPDATE Books SET copiesNumber += 1 WHERE ISBN_key = @ISBN_key";
                                            //for each book subscriber has, increase the number of copies available
                                            using (SqlCommand updateCommand = new SqlCommand(updateBooks, connection))
                                            {
                                                updateCommand.Parameters.AddWithValue("@ISBN_key", paperbook.getISBNkey());
                                                updateCommand.ExecuteNonQuery();
                                            }
                                            connection.Close();
                                        }
                                    }
                                }
                            }
                            using (SqlConnection connection = new SqlConnection(_connectionString)) //Then delete the subscriber from database
                            {
                                connection.Open();
                                string deleteSubscriber= "DELETE from Subscribers WHERE idNumber = @id";
                                using (SqlCommand deleteCommand = new SqlCommand(deleteSubscriber, connection))
                                {
                                    deleteCommand.Parameters.AddWithValue("@id", subscribers[id].getId());
                                    deleteCommand.ExecuteNonQuery();
                                }
                                connection.Close();
                            }                         
                            subscribers.Remove(id); //Delete the subscriber from dictionary 
                            Console.WriteLine("\nSuccess. Your details were deleted.");
                        }
                        else
                        {
                            Console.WriteLine("\nSubscriber does not exists.");
                            return;
                         
                        }
                        break;
                    }
                    else
                        throw new Exception ("\nInput can't be empty or contain digits. Please enter your first and last name again.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\n An error ocuur: " + ex.Message + " Please try again.");
                    Console.WriteLine("\nWhat would you like to do?\n1. Try again\n2. Go back to main menu\n"); 
                    if (!int.TryParse(Console.ReadLine(), out int userChoice) || (userChoice < 1) || (userChoice > 2))
                    {
                        Console.WriteLine("Invalid input. Please enter a valid choice next time.");
                        break;
                    }
                    if (userChoice == 1)
                    {
                        continue;
                    }
                    else if (userChoice == 2)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("No input received. Please try again.");
                        break;
                    }
                }
             }
        } 

        public void LoanBook() //Manage the loaning system of the library
        {
            while(true)
            {
                try
                {
                    if ((subscribers == null) || (books == null))
                    {
                        Console.WriteLine("Dictionaries are null.");
                        return;
                    }
                    Console.WriteLine("Please enter your id number: ");
                    string id = Console.ReadLine();
                    bool isIDint = int.TryParse(id, out _);
                    if ((!isIDint) || (id == "") || (id.Length != 9))
                    {
                        throw new Exception("Id is not valid. Please check your input.");
                    }
                    if (!subscribers.ContainsKey(id))
                    {
                        throw new Exception("Subscriber was not found. Press 2 to sign up for free.");
                    }
                    if (subscribers[id].getNumberOfLoanPaperBooks() == 3)
                    {
                        Console.WriteLine("Subscriber has maximum number of allowed books on loan");
                        return;
                    }
                    PaperBook paperbook;
                    bool hasEnoughCopies = true;
                    Console.WriteLine("Please enter the book's ISBN. If you prefer to search the book by its title please type random digit");
                    string bookISBN = Console.ReadLine();
                    bool isBookISBN = int.TryParse(bookISBN, out _);
                    if ((!isBookISBN) || (bookISBN == "") || (bookISBN.Length > 5))
                    {
                        throw new Exception("ISBN is not valid. Please check your input is 5 digits max.");
                    }
                    /*
                     The reason I chose to let the user insert the isbn before typing his kind of search it's because I want the variable will be outside 
                     the conditions so I will be able to use it when I insert data to db. 
                    */
                    Console.WriteLine("\nType I for searching a book by its ISBN or type T for searching by its title name.");
                    string typeOfSearch = Console.ReadLine().ToUpper();
                    if (typeOfSearch == "I")
                    {
                        if (books.ContainsKey(bookISBN))
                        {
                            if (books[bookISBN] is PaperBook) // Checking if book exists and if it's paper book it will decrease the number of copies.
                            {
                                paperbook = (PaperBook)books[bookISBN]; //I added this vairable so I could get an access to paperbook functions 
                                if (paperbook.getCopiesNumber() <= 0)
                                {
                                    Console.WriteLine("All copies of the book are already taken.");
                                    hasEnoughCopies = false;
                                    return;
                                }
                                paperbook.decreaseCopiesNumber();
                            }
                            subscribers[id].setNumberOfLoanPaperBooks(subscribers[id].getNumberOfLoanPaperBooks() + 1); // number of loan books of subscriber will increase                      
                            if (subscribers[id].subscriberBooks != null) //In case there is list item to the new subscriber
                            {
                                subscribers[id].subscriberBooks.Add(books[bookISBN]);
                            }
                            else
                            {
                                subscribers[id].subscriberBooks = new List<Book>(); //then build it a new list of books
                                subscribers[id].subscriberBooks.Add(books[bookISBN]);
                            }
                        }
                        else
                        {
                            throw new Exception("Book was not found. Please check you'd type the right ISBN.");
                        }
                    }
                    else if (typeOfSearch == "T")
                    {
                        Console.WriteLine("Please enter the book title: ");
                        string bookTitle = Console.ReadLine();
                        //bookTitle variable can contain digits alongside charcters and also can contain digits only.
                        bool isTitleNull = string.IsNullOrWhiteSpace(bookTitle);
                        List<Book> foundBooks = new List<Book>(); //new list that contains all books with this title
                        if (isTitleNull)
                        {
                            throw new Exception("Please fill in the title field.");
                        }
                        foreach (KeyValuePair<string, Book> element in books)
                        {
                            if (bookTitle.Equals(element.Value.getTitle(), StringComparison.CurrentCultureIgnoreCase)) //for case-insensitive manner
                            {
                                foundBooks.Add(element.Value);
                                Console.WriteLine(element.Value.ToString());
                            }
                        }
                        if (foundBooks.Count > 1) //In case there are more than one book printed with this title
                        {
                            Console.WriteLine("\nPlease choose one book and type its ISBN: "); //The subscriber is allowed to choose one of the books by typing its ISBN
                            bookISBN = Console.ReadLine();
                            isBookISBN = int.TryParse(bookISBN, out _);
                            if ((!isBookISBN) || (bookISBN == "") || (bookISBN.Length > 5))
                            {
                                throw new Exception("ISBN is not valid. Please check your input is 5 digits max.");
                            }
                            if (!foundBooks.Contains(books[bookISBN])) //In case subscriber insert exciting ISBN, but not the one from the list of presented books
                            {
                                throw new Exception("\nYou didn't insert ISBN from the list above. Please try again");
                            }
                            if (books.ContainsKey(bookISBN))
                            {
                                if (books[bookISBN] is PaperBook)
                                {
                                    paperbook = (PaperBook)books[bookISBN];
                                    if (paperbook.getCopiesNumber() <= 0)
                                    {
                                        Console.WriteLine("All copies of the book are already taken.");
                                        hasEnoughCopies = false;
                                        return;
                                    }
                                    else
                                    {
                                        paperbook.decreaseCopiesNumber();
                                    }
                                }
                                subscribers[id].setNumberOfLoanPaperBooks(subscribers[id].getNumberOfLoanPaperBooks() + 1);
                                if (subscribers[id].subscriberBooks != null)
                                {
                                    subscribers[id].subscriberBooks.Add(books[bookISBN]);
                                }
                                else
                                {
                                    subscribers[id].subscriberBooks = new List<Book>();
                                    subscribers[id].subscriberBooks.Add(books[bookISBN]);
                                }
                            }
                        }
                        else if (foundBooks.Count == 1) //in case there is one book only with this title
                        {
                            bookISBN = foundBooks[0].getISBNkey(); //so ISBN will be updated to the current one (I will use it later while inserting the data to db)
                            if (foundBooks.First() is PaperBook)
                            {
                                paperbook = (PaperBook)foundBooks[0];
                                if (paperbook.getCopiesNumber() <= 0)
                                {
                                    Console.WriteLine("All copies of the book are already taken.");
                                    hasEnoughCopies = false;
                                    return;
                                }
                                else
                                {
                                    paperbook.decreaseCopiesNumber();
                                }
                            }
                            subscribers[id].setNumberOfLoanPaperBooks(subscribers[id].getNumberOfLoanPaperBooks() + 1);
                            if (subscribers[id].subscriberBooks != null)
                            {
                                subscribers[id].subscriberBooks.Add(foundBooks[0]);
                            }
                            else
                            {
                                subscribers[id].subscriberBooks = new List<Book>();
                                subscribers[id].subscriberBooks.Add(foundBooks[0]);
                            }
                        }
                        else
                        {
                            throw new Exception("There is no book that has this title name.");
                        }
                    }
                    else
                    {
                        throw new Exception("\nInput is wrong. Please enter I for searching a book by its ISBN or T for searching by its title name.");
                    }
                    if (hasEnoughCopies) //if there is still book's copies available
                    {
                        Console.WriteLine("\nSuccess. You'd loan a book.");
                    }
                    if (subscribers[id].getNumberOfLoanPaperBooks() == 3)//if it equals 3 after we added last book then we reached the limit
                    {
                        Console.WriteLine("\nSubscriber reached limit");
                    }
                    DataSet dataSet = PopulateDataSet(id, bookISBN); //retrieves the data from the tables
                    DataRow[] subscriberRows = dataSet.Tables["Subscribers"].Select("idNumber = " + id); //gets an array of data rows matching this id (in fact just one row will be catched in the array)
                    if (subscriberRows.Length == 0) //if in any case the array is empty 
                    {
                        Console.WriteLine("Subscriber with idNumber " + id + " does not exist");
                        return;
                    }
                    DataRow subscriberRow = subscriberRows[0]; //takes the only row of data populated 
                    while (true)
                    {
                        if (string.IsNullOrEmpty(subscriberRow["firstBook_id"].ToString())) //if there is no book loanned 
                        {
                            subscriberRow["firstBook_id"] = bookISBN; //insert the isbn of the book
                            break;
                        }
                        if (string.IsNullOrEmpty(subscriberRow["secondBook_id"].ToString()))
                        {
                            subscriberRow["secondBook_id"] = bookISBN;
                            break;
                        }
                        if (string.IsNullOrEmpty(subscriberRow["thirdBook_id"].ToString()))
                        {
                            subscriberRow["thirdBook_id"] = bookISBN;
                            break;
                        }
                        break; //for a case it didn't catch the exception of 3 books exist. 
                    }
                    DataRow[] bookRows = dataSet.Tables["Books"].Select("ISBN_Key = " + bookISBN);
                    if (bookRows.Length == 0)
                    {
                        Console.WriteLine("Book with ISBN_Key " + bookISBN + " does not exist");
                        return;
                    }
                    DataRow bookRow = bookRows[0];
                    if ((string)bookRow["bookType"] == "Paper")
                    {
                        bookRow["copiesNumber"] = (int)bookRow["copiesNumber"] - 1; //decrease number of copies in the library
                    }
                    UpdateDataSet(dataSet, id, bookISBN);//update the tables
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("\nWhat would you like to do?\n1. Try again\n2. Go back to main menu\n"); //so client will had a choice and won't get stuck in the loop
                    if (!int.TryParse(Console.ReadLine(), out int userChoice) || (userChoice < 1) || (userChoice > 2))
                    {
                        Console.WriteLine("Invalid input. Please enter a valid choice next time.");
                        break;
                    }
                    if (userChoice == 1)
                    {
                        continue;
                    }
                    else if (userChoice == 2)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("No input received. Please try again.");
                        break;
                    }
                }
            }          
        }

        public void ReturnBook() //Manage the returning system of the library
        {
            while (true)
            {
                try
                {
                    if ((subscribers == null) || (books == null))
                    {
                        Console.WriteLine("Dictionaries are null.");
                        return;
                    }
                    Console.WriteLine("Please enter your id number: ");
                    string id = Console.ReadLine();
                    bool isIDint = int.TryParse(id, out _);
                    if ((!isIDint) || (id == "") || (id.Length != 9))
                    {
                        throw new Exception("Id is not valid. Please check your id has 9 digits.");
                    }
                    Console.WriteLine("Please enter the book's ISBN: ");
                    string bookISBN = Console.ReadLine();
                    bool isBookISBN = int.TryParse(bookISBN, out _);
                    if ((!isBookISBN) || (bookISBN == "") || (bookISBN.Length > 5))
                    {
                        throw new Exception("ISBN is not valid. Please check your input is 5 digits max.");
                    }
                    if (!subscribers.ContainsKey(id))
                    {
                        throw new Exception("Subscriber was not found. Please check you'd put only 9 digits."); 
                    }
                    if (!books.ContainsKey(bookISBN))
                    {
                        throw new Exception("Book was not found. Please check you put the right 5 digits ISBN number.");                       
                    }
                    Subscriber subscriber = subscribers[id];
                    if (subscriber.getNumberOfLoanPaperBooks() == 0)
                    {
                        Console.WriteLine("Subscriber doesn't have any loan books. To loan a book press 4");
                        return;
                    }
                    Book book = books[bookISBN];
                    if (!subscriber.subscriberBooks.Contains(book))
                    {
                        throw new Exception("Book was not found in subscriber's list.");
                    }
                    if (book is PaperBook)
                    {
                        PaperBook paperbook = (PaperBook)book;
                        paperbook.increaseCopiesNumber();
                        subscribers[id].setNumberOfLoanPaperBooks(subscribers[id].getNumberOfLoanPaperBooks() - 1); // number of loan books of subscriber will decrease 
                    }
                    subscriber.subscriberBooks.Remove(book);
                    DataSet dataSet= PopulateDataSet(id, bookISBN);
                    DataRow[] subscriberRows = dataSet.Tables["Subscribers"].Select("idNumber = " + id);
                    if (subscriberRows.Length == 0)
                    {
                        Console.WriteLine("Subscriber with idNumber " + id + " does not exist");
                        return;
                    }
                    DataRow subscriberRow = subscriberRows[0];
                    if ((subscriberRow["thirdBook_id"] != DBNull.Value) && ((string)subscriberRow["thirdBook_id"] == bookISBN))
                    {
                        //I checked that the value is not null to avoid error of casting null to string  
                        subscriberRow["thirdBook_id"] = DBNull.Value; //sets the third book to null
                    }
                    else if ((subscriberRow["secondBook_id"] != DBNull.Value) && ((string)subscriberRow["secondBook_id"] == bookISBN))
                    {
                        subscriberRow["secondBook_id"] = DBNull.Value; 
                    }
                    else if ((subscriberRow["firstBook_id"] != DBNull.Value) && ((string)subscriberRow["firstBook_id"] == bookISBN))
                    {
                        subscriberRow["firstBook_id"] = DBNull.Value;
                    }
                    //the order of books gets meaning when subscriber loan the same book twice or more 
                    DataRow[] bookRows = dataSet.Tables["Books"].Select("ISBN_Key = " + bookISBN);
                    if (bookRows.Length == 0)
                    {
                        Console.WriteLine("Book with ISBN_Key " + bookISBN + " does not exist");
                        return;
                    }
                    DataRow bookRow = bookRows[0];
                    if ((string)bookRow["bookType"] == "Paper")
                    {
                        bookRow["copiesNumber"] = (int)bookRow["copiesNumber"] + 1;
                    }
                    UpdateDataSet(dataSet, id, bookISBN); 
                    Console.WriteLine("Success. You'd return the book.");
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("\nWhat would you like to do?\n1. Try again\n2. Go back to main menu\n"); //so client will had a choice and won't get stuck in the loop
                    if (!int.TryParse(Console.ReadLine(), out int userChoice) || (userChoice < 1) || (userChoice > 2))
                    {
                        Console.WriteLine("Invalid input. Please enter a valid choice next time.");
                        break;
                    }
                    if (userChoice == 1)
                    {
                        continue;
                    }
                    else if (userChoice == 2)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("No input received. Please try again.");
                        break;
                    }
                }
            }
        } 

        public DataSet PopulateDataSet(string idNumber, string ISBN_Key)
        {
            //This dataSet retrieves data from the tables and will be used in the loan & return books functions.
            DataSet dataSet = new DataSet();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //The sql command gets the row of the current subscriber and current book only
                SqlCommand subscriberCommand = new SqlCommand("SELECT * FROM Subscribers WHERE idNumber = @idNumber", connection);
                subscriberCommand.Parameters.AddWithValue("@idNumber", idNumber);
                SqlDataAdapter subscriberAdapter = new SqlDataAdapter(subscriberCommand);
                //data adpter execute the sqlcommand
                subscriberAdapter.Fill(dataSet, "Subscribers");
                SqlCommand bookCommand = new SqlCommand("SELECT * FROM Books WHERE ISBN_Key = @ISBN_Key", connection);
                bookCommand.Parameters.AddWithValue("@ISBN_Key", ISBN_Key);
                SqlDataAdapter bookAdapter = new SqlDataAdapter(bookCommand);
                bookAdapter.Fill(dataSet, "Books");
                connection.Close();
            }
            return dataSet;
        }

        public void UpdateDataSet(DataSet dataSet, string idNumber, string ISBN_Key)
        {
            //This function will also be used in the loan & return books functions
            if (dataSet == null)
            {
                Console.WriteLine("DataSet is empty, nothing to update");
                return;
            }
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlDataAdapter subscriberAdapter = new SqlDataAdapter("SELECT * FROM Subscribers WHERE idNumber = @idNumber", connection);
                subscriberAdapter.SelectCommand.Parameters.AddWithValue("@idNumber", idNumber);
                SqlCommandBuilder subscriberCommandBuilder = new SqlCommandBuilder(subscriberAdapter);
                subscriberAdapter.Update(dataSet, "Subscribers");
                SqlDataAdapter BookAdapter = new SqlDataAdapter("SELECT * FROM Books WHERE ISBN_Key = @ISBN_Key", connection);
                BookAdapter.SelectCommand.Parameters.AddWithValue("@ISBN_Key", ISBN_Key);
                SqlCommandBuilder paperBookCommandBuilder = new SqlCommandBuilder(BookAdapter);
                BookAdapter.Update(dataSet, "Books");
                connection.Close();
            }
        }

        public void PrintDetailsOfBooks(string part_of_title) //Display the details of books due to client's pattern of title
        {
            while (true)
            {
                try
                {
                    if (books == null)
                    {
                        Console.WriteLine("Books dictionary is null.");
                        return;
                    }
                    if (part_of_title == "")
                    {
                        throw new Exception("Please enter a vaild title for the book. It's mandatory.");
                    }
                    //As I declared, title could contain digits and letters, and that's why this is the only verification.
                    string pattern = @"^" + part_of_title;
                    /*
                    The pattern will match any title that has the client's input as its first word. 
                    */
                    List<Book> matchedBooks = new List<Book>();
                    foreach (KeyValuePair<string, Book> book in books)
                    {
                        if (Regex.IsMatch(book.Value.getTitle(), pattern, RegexOptions.IgnoreCase)) //If the expressions is true (also for case insensative), details of book will be printed.
                        {
                            matchedBooks.Add(book.Value);
                        }
                    }
                    if (matchedBooks.Count() == 0)
                    {
                        throw new Exception("There are no books with this pattern you'd insert, try another pattern");
                    }
                    else if (matchedBooks.Count() == 1)
                    {
                        Console.WriteLine("The details of the book are as follows: \n" + matchedBooks[0].ToString());
                    }
                    else
                    {
                        Console.WriteLine("There are multiple books that match the pattern. The details of them are as follows: \n");
                        foreach (Book book in matchedBooks)
                        {
                            Console.WriteLine(book.ToString() + "\n");
                        }
                    }
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nAn error occurred: " + ex.Message);
                    Console.WriteLine("\nWhat would you like to do?\n1. Try again\n2. Go back to main menu\n"); //so client will had a choice and won't get stuck in the loop
                    if (!int.TryParse(Console.ReadLine(), out int userChoice) || (userChoice < 1) || (userChoice > 2))
                    {
                        Console.WriteLine("Invalid input. Please enter a valid choice next time.");
                        break;
                    }
                    if (userChoice == 1)
                    {
                        continue;
                    }
                    else if (userChoice == 2)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("No input received. Please try again.");
                        break;
                    }
                }
            }
        }

        public void PrintDetailsOfBooksByGenre() //Display the details of all books found by their genre
        {
            while (true)
            {
                if (books == null)
                {
                    Console.WriteLine("Books dictionary is null.");
                    return;
                }
                Console.WriteLine("Please enter book genre: ");
                string bookGenre = Console.ReadLine();
                bool isBookGenreNumeric = bookGenre.Any(char.IsDigit);
                try
                {
                    if ((!isBookGenreNumeric) && (bookGenre != ""))
                    {
                        string relevantBooks = "";
                        int numOfRelevantBooks = 0;
                        foreach (KeyValuePair<string, Book> element in books)
                        {
                            if (bookGenre.Equals(element.Value.getbookGenre(), StringComparison.CurrentCultureIgnoreCase)) //for case-insensitive manner
                            {
                                relevantBooks += "(" + (numOfRelevantBooks + 1) + ") " + element.Value.ToString() + ", \n"; //NumOfRelevantBooks is a variable that index on the number of books in the list
                                numOfRelevantBooks++;
                            }
                        }
                        if (relevantBooks != "")
                        {
                            if (relevantBooks.Length > 2) //For the exception of a case relevantBooks string has less than 2 characters in it
                            {
                                relevantBooks = relevantBooks.Substring(0, relevantBooks.Length - 3); //So it prints the sliced string without the last comma.
                            }
                            Console.WriteLine("\nThe " + bookGenre +" books we hold: \n");
                            Console.WriteLine(relevantBooks);
                        }
                        else
                        {
                            throw new Exception("No books from genre " + bookGenre);
                        }
                        break; 
                    }
                    else
                    {
                        throw new Exception("Input is wrong. Please check the input isn't empty and book's genre isn't a number.\n");
                    }
                }
                catch (ArgumentOutOfRangeException ex) //Invalid access to string 
                {
                    Console.WriteLine("An error occurred: The start index or length of the Substring method is invalid.");
                }
                catch (Exception ex) //For other exceptions
                {
                    Console.WriteLine("\n"+ ex.Message);
                    Console.WriteLine("\nWhat would you like to do?\n1. Try again\n2. Go back to main menu\n"); //so client will had a choice and won't get stuck in the loop
                    if (!int.TryParse(Console.ReadLine(), out int userChoice) || (userChoice < 1) || (userChoice > 2))
                    {
                        Console.WriteLine("Invalid input. Please enter a valid choice next time.");
                        break;
                    }
                    if (userChoice == 1)
                    {
                        continue;
                    }
                    else if (userChoice == 2)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("No input received. Please try again.");
                        break;
                    }
                }
            }
            //I used this website: https://www.completecsharptutorial.com/basic/complete-system-exception.php to search the whole list of exceptions and find what can be connected.
        }

        public void PrintListOfSubscriberBooks() //Display the books subscriber has
        {
            while (true)
            {
                Console.WriteLine("Please enter your id number: ");
                string id = Console.ReadLine();
                bool isIDint = int.TryParse(id, out _);
                try
                {
                    if (subscribers == null)
                    {
                        Console.WriteLine("Subscribers dictionary is null.");
                        return;
                    }
                    if ((!isIDint) || (id == "") || (id.Length != 9)) //Checks whether the ID input contains only digits
                    {
                        throw new Exception("Input is wrong. Please check you'd insert your id and that it contains only 9 digits.");
                    }
                    Subscriber subscriber = subscribers[id]; //Access the subscriber with this id
                    if (subscriber.subscriberBooks == null || subscriber.subscriberBooks.Count == 0) //If there are no books in the list and list isn't null
                    {
                        Console.WriteLine("There are no books borrowing. To loan a book please press 4.");
                        return;
                    }
                    Console.WriteLine("Subscriber's list of books: \n");
                    int count = 1;
                    foreach (Book value in subscriber.subscriberBooks)
                    {
                        if (value != null)
                        {
                            Console.Write("("+ count+ ")");
                            Console.WriteLine(value.ToString());
                            count++;
                        }
                    }
                    break;
                }
                catch (KeyNotFoundException) //If the subscriber is not found, the exception of KeyNotFound is caught
                {
                    Console.WriteLine("Subscriber with the ID: {0} was not found. To create account please press 2.", id);
                    return;
                }
                catch (Exception ex) //For other exceptions
                {
                    Console.WriteLine("\n" + ex.Message);
                    Console.WriteLine("\nWhat would you like to do?\n1. Try again\n2. Go back to main menu\n"); //so client will had a choice and won't get stuck in the loop
                    if (!int.TryParse(Console.ReadLine(), out int userChoice) || (userChoice < 1) || (userChoice > 2))
                    {
                        Console.WriteLine("Invalid input. Please enter a valid choice next time.");
                        break;
                    }
                    if (userChoice == 1)
                    {
                        continue;
                    }
                    else if (userChoice == 2)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("No input received. Please try again.");
                        break;
                    }
                }
            }
        }
    }
}

