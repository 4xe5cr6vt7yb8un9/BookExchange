using System.Data.SqlClient;

namespace BookExchange
{
    public class SQLActions
    {
        private const String SQLDetails = "Data Source=" +
                                          "Provider=SQLOLEDB.1;" +
                                          "Integrated Security=SSPI;" +
                                          "Persist Security Info=False;" +
                                          "Initial Catalog=master;" +
                                          "Data Source=localhost\\SQLEXPRESS";

        public static void doSelectStatement(int ID)
        {
            String searchQuery = "SELECT Name, Author, Description, ReleasedDate, available where ISBN = " + ID;
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
                        ReleasedDate = sqlReader.GetDateTime(3),
                    };
                }
            }
            catch
            {
                Console.WriteLine("Error occurred while attempting SELECT.");
            }
            selectCommand.Connection.Close();
        }
    }
}
