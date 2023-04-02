using System.Data.SqlClient;

namespace BookExchange
{
    public static class SQLGetActions
    {
        private const String SQLDetails = "Data Source=" +
                                          "Provider=SQLOLEDB.1;" +
                                          "Integrated Security=SSPI;" +
                                          "Persist Security Info=False;" +
                                          "Initial Catalog=BookExchange;" +
                                          "Data Source=localhost\\SQLEXPRESS";

        public static Book getBookByQuery(String query)
        {
            String searchQuery = "SELECT * FROM Books WHERE " + query;
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
                        Name = sqlReader.GetString(0),
                        Author = sqlReader.GetString(1),
                        Description = sqlReader.GetString(2),
                        Published = sqlReader.GetString(3),
                        available = Int32.Parse(sqlReader.GetString(4)),
                        ISBN = sqlReader.GetString(5)
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

        public static ExchangeUser getUserByQuery(String query)
        {
            String searchQuery = "SELECT * FROM Users WHERE " + query;
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
                        UserID = sqlReader.GetString(0),
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

        public static List<string> getBorrowByUser(String userID)
        {
            String searchQuery = "SELECT Book FROM Borrowers WHERE UserID = " + userID;
            List<String> books = new();

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

        public static List<string> getLoansByUser(String userID)
        {
            String searchQuery = "SELECT Book FROM Loaners WHERE UserID = " + userID;
            List<String> books = new();

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
