using Dapper;
using Lab9Startup.Models;
using MySqlConnector;

namespace Lab9Startup.Services
{
    public class BookDbAccessor
    {
        protected MySqlConnection connection;

        public BookDbAccessor()
        {
            // get environemnt variable
            string dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            string dbUser = Environment.GetEnvironmentVariable("DB_USER");
            string dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

            var builder = new MySqlConnectionStringBuilder
            {
                Server = dbHost,
                UserID = dbUser,
                Password = dbPassword,
                Database = "library", // Use maria db to create a database called library
            };

            connection = new MySqlConnection(builder.ConnectionString);
        }

        /// <summary>
        /// Initialize the database and create the books table
        /// </summary>
        public void InitializeDatabase()
        {
            connection.Open();

            var sql = @"CREATE TABLE IF NOT EXISTS books (
                BookId VARCHAR(36) PRIMARY KEY,
                Title VARCHAR(255) NOT NULL,
                Author VARCHAR(255) NOT NULL,
                Description TEXT,
                Category VARCHAR(255)
            )";

            connection.Execute(sql);

            connection.Close();
        }

        public void AddBook(Book book)
        {
            connection.Open();

            string sql = "INSERT INTO books (BookId, Title, Author, Description, Category) VALUES (@BookId, @Title, @Author, @Description, @Category)";

            connection.Execute(sql, book);
            connection.Close();
        }

        public List<Book> GetBooks()
        {
            connection.Open();

            string sql = "SELECT * FROM books";

            var books = connection.Query<Book>(sql);

            connection.Close();
            return books.ToList();
        }

        public Book GetBook(string bookId)
        {
            connection.Open();

            string sql = "SELECT * FROM books WHERE BookId = @BookId";

            Book book = connection.QueryFirstOrDefault<Book>(sql, new { BookId = bookId });
            connection.Close();

            return book;
        }

        public void UpdateBook(Book book)
        {
            connection.Open();

            string sql = "UPDATE books SET Title = @Title, Author = @Author, Description = @Description WHERE BookId = @BookId";

            connection.Execute(sql, book);
            connection.Close();
        }

        public void DeleteBook(string bookId)
        {
            connection.Open();

            string sql = "DELETE FROM books WHERE BookId = @BookId";

            connection.Execute(sql, new { BookId = bookId });
            connection.Close();
        }
    }
}
