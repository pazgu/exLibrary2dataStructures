using exLibrary2dataStructures;
using System.Data.SqlClient;
using System.Data;
enum Actions
    {
        // items of the enum- each describes the name of the action the client is about to do. 
        AddBook = 1, //so the enum will start its counting from 1
        AddSubscriber,
        DeleteSubscriber,
        LoanBook,
        ReturnBook,
        PrintDetailsOfBook,
        PrintDetailsOfBookByGenre,
        PrintListOfSubscriberBooks,
        Goodbye
    }
    class Program
    {
        static void Main(string[] args)
        {
            string masterConnectionString = @"Server=localhost\SQLEXPRESS;Database=master;Trusted_Connection=True;"; 
            string connectionString = @"Server=localhost\SQLEXPRESS;Database=GuetaPazLibrary;Trusted_Connection=True;";       
            SqlConnection con = new SqlConnection(masterConnectionString);
            con.Open();
            DatabaseManager dbManager = new DatabaseManager(masterConnectionString, connectionString);
            dbManager.CreateDatabase();
            dbManager.CreateTables();
            dbManager.InsertRecords();
            con.Close();

            //The valus I inserted are exactly the same as the first records I insert to the database
            //I did it in order to do the exact actions ,in each function, both to data inside the objects and data in the database.
            Book book1 = new PaperBook("84523", 32, "The clown", "Peter George", "Comedy");
            Book book2 = new DigitalBook("12367", 114, "Harry Potter and the chamber of secrets", "J.K Rowling", "Science Fiction");
            Book book3 = new PaperBook("25310", 12, "Harry Potter and the order of the phoenix 5", "J.K Rowling", "Science Fiction");
            Book book4 = new DigitalBook("35821", 455.2, "Harry Potter and the deathly hallows 7- part 1", "J.K Rowling", "Science Fiction");
            Book book5 = new PaperBook("113", 176, "Harry Potter and the deathly hallows 7- part 2", "J.K Rowling", "Science Fiction");
            Book book6 = new PaperBook("17", 12, "The chesnut man", "Soren Sveistrup", "Drama");
            Book book7 = new DigitalBook("3456", 783, "Who Killed Sara", "Yoni Paz", "Comedy Romance");
            Book book8 = new PaperBook("654", 129, "The innocents", "Taylor Gemirov", "Drama");
            Book book9 = new DigitalBook("7896", 197, "TheCrown1", "Peter Morgan", "Horror");
            Book book10 = new DigitalBook("17643", 1267, "The clown 2", "Peter George", "Comedy");
            Book book11 = new PaperBook("25322", 1, "Breaking bad", "Michael Slovis", "Mystery");
            Book book12 = new PaperBook("115", 7, "The stranger", "Harlan Coben", "Fantasy");
            Book book13 = new DigitalBook("163", 157, "Gone for good", "Harlan Coben", "Fantasy"); 
            Book book14= new PaperBook("1789", 3, "The woods", "Soren Sveistrup", "Drama");
            Book book15 = new DigitalBook("22", 322, "Hold tight", "Yoni Paz", "Comedy Romance");
            Book book16 = new PaperBook("1268", 9, "The innocents", "Taylor Gemirov", "Mystery");
            Dictionary<string, Book> books = new Dictionary<string, Book>(); //Dictionary of all books in library.

            books.Add(book1.getISBNkey(), book1); //add to dictionary book's key and book object.
            books.Add(book2.getISBNkey(), book2);
            books.Add(book3.getISBNkey(), book3); 
            books.Add(book4.getISBNkey(), book4);   
            books.Add(book5.getISBNkey(), book5);
            books.Add(book6.getISBNkey(), book6);
            books.Add(book7.getISBNkey(), book7);
            books.Add(book8.getISBNkey(), book8);
            books.Add(book9.getISBNkey(), book9);
            books.Add(book10.getISBNkey(), book10);
            books.Add(book11.getISBNkey(), book11); 
            books.Add(book12.getISBNkey(), book12);
            books.Add(book13.getISBNkey(), book13);
            books.Add(book14.getISBNkey(), book14); 
            books.Add(book15.getISBNkey(), book15);
            books.Add(book16.getISBNkey(), book16);

            Dictionary<string, Subscriber> subscribers = new Dictionary<string, Subscriber>(); //Dictionary of all subscribers in library.

            List<Book> subscriber1Books = new List<Book>(3); //List of books subscriber1 have
            List<Book> subscriber2Books = new List<Book>(3);
            List<Book> subscriber3Books = new List<Book>(3);
            List<Book> subscriber4Books = new List<Book>(3);
            List<Book> subscriber5Books = new List<Book>(3);
            List<Book> subscriber6Books = new List<Book>(3);

            subscriber1Books.Add(book1);
            subscriber1Books.Add(book5);
            subscriber1Books.Add(book6);

            Subscriber subscriber1 = new Subscriber("209456561", "Paz", "Gueta", subscriber1Books);
            subscriber1.setNumberOfLoanPaperBooks(3);
            subscribers.Add("209456561", subscriber1);

            Subscriber subscriber2 = new Subscriber("317223561", "Harel", "Skaat", subscriber2Books);
            subscriber2.setNumberOfLoanPaperBooks(0);
            subscribers.Add("317223561", subscriber2);

            subscriber3Books.Add(book3);
            subscriber3Books.Add(book7);
            subscriber3Books.Add(book8);

            Subscriber subscriber3 = new Subscriber("203548932" ,"Lior", "Menashe", subscriber3Books);
            subscriber3.setNumberOfLoanPaperBooks(3);
            subscribers.Add("203548932", subscriber3);

            subscriber4Books.Add(book12);
            subscriber4Books.Add(book6);

            Subscriber subscriber4 = new Subscriber("307689324", "Shiri", "Maymon", subscriber4Books);
            subscriber4.setNumberOfLoanPaperBooks(2);
            subscribers.Add("307689324", subscriber4);

            subscriber5Books.Add(book3);
            subscriber5Books.Add(book3);
            subscriber5Books.Add(book3);

            Subscriber subscriber5 = new Subscriber("276943766", "Ninet", "Tayeb", subscriber5Books);
            subscriber5.setNumberOfLoanPaperBooks(3);
            subscribers.Add("276943766", subscriber5);

            subscriber6Books.Add(book12);

            Subscriber subscriber6 = new Subscriber("301258673", "Shay", "Gabso", subscriber6Books);
            subscriber6.setNumberOfLoanPaperBooks(1);
            subscribers.Add("301258673", subscriber6);

            Library library = new Library(books, subscribers, connectionString);
            bool b = true;
            while (b == true)
            {
                try
                {
                    Console.WriteLine("Welcome to Kotar Rishon Lezion library \n\n Press button 1-9 according to the following: \n (1) Add a new book \n (2) Add a new subscriber \n (3) Delete subscriber \n (4) Loan a prefered book \n (5) Return a book \n (6) Print book's details \n (7) Print books from the same genre type \n (8) Print subscriber's list of books \n (9) Exit");
                    string buttonNumber = Console.ReadLine();
                    if (Enum.TryParse(buttonNumber, out Actions action))
                    {
                        switch (action)
                        {
                            case Actions.AddBook:
                                library.AddBook();
                                Console.WriteLine("\nOur collection of books:\n");
                                foreach (KeyValuePair<string, Book> element in books)
                                {
                                    if (element.Value == null)
                                    {
                                        break;
                                    }
                                    Console.WriteLine(element.Value.ToString());
                                    Console.WriteLine("\n");
                                }
                                Console.WriteLine("\n");
                                break;
                            case Actions.AddSubscriber:
                                library.AddSubscriber();
                            Console.WriteLine("\n");
                                break;
                            case Actions.DeleteSubscriber:
                                library.DeleteSubscriber();
                                Console.WriteLine("\n");
                                break;
                        case Actions.LoanBook:
                                library.LoanBook();
                                Console.WriteLine("\n");
                                break;
                            case Actions.ReturnBook:
                                library.ReturnBook();
                                Console.WriteLine("\nOur collection of books, in case you want to loan a new book:\n");
                                foreach (KeyValuePair<string, Book> element in books)
                                {
                                    if (element.Value == null)
                                    {
                                        break;
                                    }
                                    Console.WriteLine(element.Value.ToString());
                                    Console.WriteLine("\n");
                                }
                                Console.WriteLine("\n");
                                break;
                            case Actions.PrintDetailsOfBook:
                                Console.WriteLine("\nPlease enter the title's pattern you're requesting to search: ");
                                string partOfTitle= Console.ReadLine();
                                library.PrintDetailsOfBooks(partOfTitle);
                                Console.WriteLine("\n");
                                break;
                            case Actions.PrintDetailsOfBookByGenre:
                                library.PrintDetailsOfBooksByGenre();
                                Console.WriteLine("\n");
                                break;
                            case Actions.PrintListOfSubscriberBooks:
                                library.PrintListOfSubscriberBooks();
                                Console.WriteLine("\n");
                                break;
                            case Actions.Goodbye:
                                Console.WriteLine("Goodbye");
                                b = false;
                                break;
                            default:
                                Console.WriteLine("\nInput is wrong and there is no such action. Please enter number from 1 to 9.\n");
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "\n");
                }
            }
        }
    }





