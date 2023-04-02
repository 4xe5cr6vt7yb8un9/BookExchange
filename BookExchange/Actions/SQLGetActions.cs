using System.Data.SqlClient;
using System.Diagnostics;

namespace BookExchange.Actions
{
    public static class SQLGetActions
    {
        private const string SQLDetails = "Data Source=" +
                                          "Provider=SQLOLEDB.1;" +
                                          "Integrated Security=SSPI;" +
                                          "Persist Security Info=False;" +
                                          "Initial Catalog=BookExchange;" +
                                          "Data Source=localhost\\SQLEXPRESS";

        public static Book getBookByQuery(string query)
        {
            string searchQuery = "SELECT * FROM Books WHERE " + query;
            Book selectBook;

            using SqlConnection newConnection = new(SQLDetails);
            SqlCommand selectCommand = new(searchQuery, newConnection);
            selectCommand.Connection.Open();

            SqlDataReader sqlReader;
            try
            {
                sqlReader = selectCommand.ExecuteReader();

                if (sqlReader.Read())
                {
                    selectBook = new Book
                    {
                        Title = sqlReader.GetString(1),
                        Author = sqlReader.GetString(2),
                        Description = sqlReader.GetString(3),
                        Published = sqlReader.GetString(4),
                        Available = sqlReader.GetInt32(5),
                        ISBN = sqlReader.GetString(0)
                    };
                    return selectBook;
                }
            }
            catch
            {
                Console.WriteLine("Error occurred while attempting SELECT.");
            }
            selectCommand.Connection.Close();
            return new Book();
        }

        public static bool verifyISBN(string ISBN)
        {
            string searchQuery = "SELECT * FROM Books WHERE ISBN = " + ISBN;

            using SqlConnection newConnection = new(SQLDetails);
            SqlCommand selectCommand = new(searchQuery, newConnection);
            selectCommand.Connection.Open();

            SqlDataReader sqlReader;
            try
            {
                sqlReader = selectCommand.ExecuteReader();

                if (sqlReader.Read())
                {
                    selectCommand.Connection.Close();
                    return true;
                }
            }
            catch
            {
                selectCommand.Connection.Close();
                return false;
            }
            selectCommand.Connection.Close();
            return true;
        }

        public static ExchangeUser getUserByQuery(string query)
        {
            string searchQuery = "SELECT * FROM Users WHERE " + query;
            ExchangeUser selectUser;

            using SqlConnection newConnection = new(SQLDetails);
            SqlCommand selectCommand = new(searchQuery, newConnection);
            selectCommand.Connection.Open();

            SqlDataReader sqlReader;
            try
            {
                sqlReader = selectCommand.ExecuteReader();

                if (sqlReader.Read())
                {
                    selectUser = new ExchangeUser
                    {
                        UserID = sqlReader.GetGuid(0),
                        Name = sqlReader.GetString(1),
                        Email = sqlReader.GetString(2),
                    };
                    return selectUser;
                }
            }
            catch
            {
                Console.WriteLine("Error occurred while attempting SELECT.");
            }
            selectCommand.Connection.Close();
            return new ExchangeUser();
        }

        public static List<string> getBorrowByUser(string userID)
        {
            string searchQuery = "SELECT BookISBN FROM Borrowers WHERE UserID = " + userID;
            List<string> books = new();

            using SqlConnection newConnection = new(SQLDetails);
            SqlCommand selectCommand = new(searchQuery, newConnection);
            selectCommand.Connection.Open();

            SqlDataReader sqlReader;
            try
            {
                sqlReader = selectCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    _ = books.Append(sqlReader.GetString(0));
                }
            }
            catch
            {
                Console.WriteLine("Error occurred while attempting SELECT.");
            }
            selectCommand.Connection.Close();
            return books;
        }

        public static List<string> getLoansByUser(string userID)
        {
            string searchQuery = "SELECT BookISBN FROM Loaners WHERE UserID = " + userID;
            List<string> books = new();

            using SqlConnection newConnection = new(SQLDetails);
            SqlCommand selectCommand = new(searchQuery, newConnection);
            selectCommand.Connection.Open();

            SqlDataReader sqlReader;
            try
            {
                sqlReader = selectCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    _ = books.Append(sqlReader.GetString(0));
                }
            }
            catch
            {
                Console.WriteLine("Error occurred while attempting SELECT.");
            }
            selectCommand.Connection.Close();
            return books;
        }
    }
}
