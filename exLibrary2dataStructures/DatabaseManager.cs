using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace exLibrary2dataStructures
{
    class DatabaseManager
    {
        private string _masterConnectionString;
        private string _connectionString;
        public DatabaseManager(string masterConnectionString, string connectionString)
        {
            _masterConnectionString = masterConnectionString; //gets the connection to the master db from the Main.cs page
            _connectionString = connectionString; //gets the connection to my created db 
        }
        public void CreateDatabase()
        {
            string[] dbCreation = { //create an array of commands 
            "DROP DATABASE IF EXISTS GuetaPazLibrary",
            "CREATE DATABASE GuetaPazLibrary"
            };
            using (SqlConnection connection = new SqlConnection(_masterConnectionString))
            {
                try
                {
                    connection.Open();
                    foreach (string db in dbCreation)
                    {
                        try
                        {
                            using (SqlCommand command = new SqlCommand(db, connection))
                            {
                                command.ExecuteNonQuery();
                            }
                        }
                        catch (SqlException ex) //so it catches the current command
                        {
                            throw new Exception("An error occurred while executing database creation: ");
                        }
                        catch (System.Exception ex)
                        {
                            throw new Exception("An error occurred while executing database creation: ");
                        }
                    }
                    Console.WriteLine("Database created successfully!");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("\n"+ ex.Message);
                }
                finally //to force closing the connection 
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }
        public void CreateTables()
        {
            // Creates array of strings for all commands of creating the tables, so it will be easier to iterate over them
            string[] tableCreations = {
            "DROP TABLE IF EXISTS Subscribers",
            "DROP TABLE IF EXISTS Books",
            "CREATE TABLE Subscribers (idNumber CHAR(9) PRIMARY KEY, firstName VARCHAR(15) NOT NULL, lastName VARCHAR(40) NOT NULL, firstBook_id VARCHAR(5) REFERENCES Books(ISBN_Key), secondBook_id VARCHAR(5) REFERENCES Books(ISBN_Key), thirdBook_id VARCHAR(5) REFERENCES Books(ISBN_Key))",
            "CREATE TABLE Books (ISBN_Key VARCHAR(5) PRIMARY KEY, bookType VARCHAR(10) NOT NULL, bookTitle VARCHAR(50) NOT NULL, author VARCHAR(50) NOT NULL, genre VARCHAR(20) NOT NULL, fileSize FLOAT, copiesNumber INT)" };
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    foreach (string table in tableCreations)
                    {
                        try
                        {
                            using (SqlCommand command = new SqlCommand(table, connection))
                            {
                                command.ExecuteNonQuery();
                            }
                        }
                        catch (SqlException ex) //so it catches the exception to the current command
                        {
                            throw new Exception("An error occurred while executing table: " + table + "\n");
                        } 
                        catch (System.Exception ex)
                        {
                            throw new Exception("An error occurred while executing table: " + table + "\n");
                        }
                    }
                    Console.WriteLine("Tables created successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            } //in both functions of creation I refer the exectuing of the commands as if one of the commands made an error, the rest of the commands won't be exectuted also
        }

        //Insert records to the tables
        public void InsertRecords()
        {
            string[] BooksRecords = {
            "INSERT INTO Books (ISBN_Key, bookType, bookTitle, author, genre, fileSize, copiesNumber) VALUES ('12367', 'Digital', 'Harry Potter and the chamber of secrets', 'J.K Rowling', 'Science Fiction', 114, null)",
            "INSERT INTO Books (ISBN_Key, bookType, bookTitle, author, genre, fileSize, copiesNumber) VALUES('84523', 'Paper', 'The clown', 'Peter George', 'Comedy', null, 32)",
            "INSERT INTO Books (ISBN_Key, bookType, bookTitle, author, genre, fileSize, copiesNumber) VALUES('25310', 'Paper', 'Harry Potter and the order of the phoenix 5', 'J.K Rowling', 'Science Fiction', null, 12)",
            "INSERT INTO Books (ISBN_Key, bookType, bookTitle, author, genre, fileSize, copiesNumber) VALUES('35821', 'Digital', 'Harry Potter and the deathly hallows 7- part 1', 'J.K Rowling', 'Science Fiction', 455.2, null)",
            "INSERT INTO Books (ISBN_Key, bookType, bookTitle, author, genre, fileSize, copiesNumber) VALUES('113', 'Paper', 'Harry Potter and the deathly hallows 7- part 2', 'J.K Rowling', 'Science Fiction', null, 176)",
            "INSERT INTO Books (ISBN_Key, bookType, bookTitle, author, genre, fileSize, copiesNumber) VALUES('17', 'Paper', 'The chesnut man', 'Soren Sveistrup', 'Drama', null, 12)",
            "INSERT INTO Books (ISBN_Key, bookType, bookTitle, author, genre, fileSize, copiesNumber) VALUES('3456', 'Digital', 'Who Killed Sara', 'Yoni Paz', 'Comedy Romance', 783, null)",
            "INSERT INTO Books (ISBN_Key, bookType, bookTitle, author, genre, fileSize, copiesNumber) VALUES('654', 'Paper', 'The innocents', 'Taylor Gemirov', 'Drama', null, 129)",
            "INSERT INTO Books (ISBN_Key, bookType, bookTitle, author, genre, fileSize, copiesNumber) VALUES ('7896', 'Digital', 'TheCrown1', 'Peter Morgan', 'Horror', 197, null)",
            "INSERT INTO Books (ISBN_Key, bookType, bookTitle, author, genre, fileSize, copiesNumber) VALUES('17643', 'Digital', 'The clown 2', 'Peter George', 'Comedy', 1267, null)",
            "INSERT INTO Books (ISBN_Key, bookType, bookTitle, author, genre, fileSize, copiesNumber) VALUES('25322', 'Paper', 'Breaking bad', 'Michael Slovis', 'Mystery', null, 1)",
            "INSERT INTO Books (ISBN_Key, bookType, bookTitle, author, genre, fileSize, copiesNumber) VALUES('115', 'Paper', 'The stranger', 'Harlan Coben', 'Fantasy', null, 7)",
            "INSERT INTO Books (ISBN_Key, bookType, bookTitle, author, genre, fileSize, copiesNumber) VALUES('163', 'Digital', 'Gone for good', 'Harlan Coben', 'Fantasy', 157, null)",
            "INSERT INTO Books (ISBN_Key, bookType, bookTitle, author, genre, fileSize, copiesNumber) VALUES('1789', 'Paper', 'The woods', 'Soren Sveistrup', 'Drama', null, 3)",
            "INSERT INTO Books (ISBN_Key, bookType, bookTitle, author, genre, fileSize, copiesNumber) VALUES('22', 'Digital', 'Hold tight', 'Yoni Paz', 'Comedy Romance', 322, null)",
            "INSERT INTO Books (ISBN_Key, bookType, bookTitle, author, genre, fileSize, copiesNumber) VALUES('1268', 'Paper', 'The innocents', 'Taylor Gemirov', 'Mystery', null, 9)"
            };

            string[] SubscribersRecords = {
            "INSERT INTO Subscribers (idNumber, firstName, lastName, firstBook_id, secondBook_id, thirdBook_id) VALUES('209456561', 'Paz', 'Gueta', '84523', '113', '17')",
            "INSERT INTO Subscribers (idNumber, firstName, lastName, firstBook_id, secondBook_id, thirdBook_id) VALUES('317223561','Harel', 'Skaat', null, null, null)",
            "INSERT INTO Subscribers (idNumber, firstName, lastName, firstBook_id, secondBook_id, thirdBook_id) VALUES('203548932','Lior', 'Menashe', '25310', '3456', '654')",
            "INSERT INTO Subscribers (idNumber, firstName, lastName, firstBook_id, secondBook_id, thirdBook_id) VALUES('307689324','Shiri', 'Maymon', '115', '17', null)",
            "INSERT INTO Subscribers (idNumber, firstName, lastName, firstBook_id, secondBook_id, thirdBook_id) VALUES('276943766','Ninet', 'Tayeb', '25310', '25310', '25310')",
            "INSERT INTO Subscribers (idNumber, firstName, lastName, firstBook_id, secondBook_id, thirdBook_id) VALUES('301258673','Shay', 'Gabso', '115', null, null)",
            // Create foreign key constraints for Books table
            "ALTER TABLE Subscribers ADD FOREIGN KEY (firstBook_id) REFERENCES Books(ISBN_Key)",
            "ALTER TABLE Subscribers ADD FOREIGN KEY (secondBook_id) REFERENCES Books(ISBN_Key)",
            "ALTER TABLE Subscribers ADD FOREIGN KEY (thirdBook_id) REFERENCES Books(ISBN_Key)" };
        
            //Execute the insert books commands
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    foreach (string record in BooksRecords)
                    {
                        using (SqlCommand cmd = new SqlCommand(record, connection))
                        {
                            try
                            {
                                cmd.ExecuteNonQuery();
                            }
                            catch (SqlException ex)
                            {
                                Console.WriteLine("An error occurred while inserting a record into the Books table:\n");
                                continue; //so even if one of the record made an error, the rest of the commands will be executed
                            }
                            catch (System.Exception ex)
                            {
                                Console.WriteLine("An error occurred while inserting a record into the Books table:\n");
                                continue;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
            //Execute the insert subscribers commands
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    foreach (string record in SubscribersRecords)
                    {
                        using (SqlCommand cmd = new SqlCommand(record, connection))
                        {
                            try
                            {
                                cmd.ExecuteNonQuery();
                            }
                            catch (SqlException ex)
                            {
                                Console.WriteLine("An error occurred while inserting a record into the Subscribers table:\n");
                                continue;
                            }
                            catch (System.Exception ex)
                            {
                                Console.WriteLine("An error occurred while inserting a record into the Subscribers table:\n");
                                continue;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
            Console.WriteLine("Records inserted successfully");
        }  
    }
}

