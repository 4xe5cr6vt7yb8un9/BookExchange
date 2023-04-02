using System.Data.SqlClient;
using System.Diagnostics;

namespace BookExchange.Actions
{
    public class SQLSetActions
    {
        private const string SQLDetails = "Data Source=" +
                                          "Provider=SQLOLEDB.1;" +
                                          "Integrated Security=SSPI;" +
                                          "Persist Security Info=False;" +
                                          "Initial Catalog=BookExchange;" +
                                          "Data Source=localhost\\SQLEXPRESS";

        public static void addBook(Book newBook)
        {
            string searchQuery = "INSERT INTO Books (ISBN, Title, Author, Descr, Published, Stock) " +
                                 "VALUES ('" +
                                    newBook.ISBN + "','" +
                                    newBook.Title + "','" +
                                    newBook.Author + "','" +
                                    newBook.Description + "','" +
                                    newBook.Published + "'," +
                                    newBook.Available + ")";
            Debug.Print(searchQuery);
            using SqlConnection newConnection = new(SQLDetails);
            SqlCommand selectCommand = new(searchQuery, newConnection);
            selectCommand.Connection.Open();

            try
            {
                selectCommand.ExecuteNonQuery();
            }
            catch
            {
                Console.WriteLine("Error occurred while attempting INSERT.");
            }
            selectCommand.Connection.Close();
        }

        public static void addUser(ExchangeUser newUser)
        {
            string searchQuery = "INSERT INTO Users (UserID, Name, Email) " +
                                 "VALUES ('" +
                                    newUser.UserID + "','" +
                                    newUser.Name + "','" +
                                    newUser.Email + "')";

            using SqlConnection newConnection = new(SQLDetails);
            SqlCommand selectCommand = new(searchQuery, newConnection);
            selectCommand.Connection.Open();

            try
            {
                selectCommand.ExecuteNonQuery();
            }
            catch
            {
                Console.WriteLine("Error occurred while attempting INSERT.");
            }
            selectCommand.Connection.Close();
        }

        public static void addBorrowedBook(string userID, string book)
        {
            string searchQuery = "INSERT INTO Borrowers (UserID, BookISBN) " +
                                 "VALUES ('" +
                                    userID + "','" +
                                    book + "')";

            using SqlConnection newConnection = new(SQLDetails);
            SqlCommand selectCommand = new(searchQuery, newConnection);
            selectCommand.Connection.Open();

            try
            {
                selectCommand.ExecuteNonQuery();
            }
            catch
            {
                Console.WriteLine("Error occurred while attempting INSERT.");
            }
            selectCommand.Connection.Close();
        }

        public static void addLoanedBook(string userID, string book)
        {
            string searchQuery = "INSERT INTO Loaners (UserID, BookISBN) " +
                                 "VALUES ('" +
                                    userID + "','" +
                                    book + "')";

            using SqlConnection newConnection = new(SQLDetails);
            SqlCommand selectCommand = new(searchQuery, newConnection);
            selectCommand.Connection.Open();

            try
            {
                selectCommand.ExecuteNonQuery();
            }
            catch
            {
                Console.WriteLine("Error occurred while attempting INSERT.");
            }
            selectCommand.Connection.Close();
        }
    }
}
