using BussinessObject;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Text;

namespace DataAccess
{
    public class LibraryRepository
    {
        private string connectionString;

        public LibraryRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public List<Book> SearchBooks(string BookTitle, string BookAuthor, string ISBN)
        {
            List<Book> books = new List<Book>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Books WHERE Title LIKE @Title and Author LIKE @Author and ISBN LIKE @ISBN";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", "%" + BookTitle + "%");
                    command.Parameters.AddWithValue("@Author", "%" + BookAuthor + "%");
                    command.Parameters.AddWithValue("@ISBN", "%" + ISBN + "%");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Book book = new Book
                            {
                                BookID = Convert.ToInt32(reader["BookID"]),
                                Title = reader["Title"].ToString(),
                                Author = reader["Author"].ToString(),
                                ISBN = reader["ISBN"].ToString(),
                                Availability = Convert.ToBoolean(reader["Availability"])
                            };
                            books.Add(book);
                        }
                    }
                }
            }

            return books;
        }

        public Book BorrowBook( int bookId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var book = getBook(bookId, connection);
                if (book != null && book.Availability == true)
                {
          
                            string updateAvailabilityQuery = "UPDATE Books SET Availability = 0 WHERE BookID = @BookID";
                            using (SqlCommand updateAvailabilityCommand = new SqlCommand(updateAvailabilityQuery, connection))
                            {
                                updateAvailabilityCommand.Parameters.AddWithValue("@BookID", bookId);
                                updateAvailabilityCommand.ExecuteNonQuery();
                            }
                        connection.Close();
                    book.Availability = false;
                        return book; // Borrowing successful
                      //  }
                    }
                    else
                    {
                        connection.Close();
                        return book; // Book is not available
                    }
            }
            
        }

        public Book ReturnBook(int bookId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var book = getBook(bookId, connection);
                if (book != null && book.Availability == false)
                {
                    string updateAvailabilityQuery = "UPDATE Books SET Availability = 1 WHERE BookID = @BookID";
                    using (SqlCommand updateAvailabilityCommand = new SqlCommand(updateAvailabilityQuery, connection))
                    {
                        updateAvailabilityCommand.Parameters.AddWithValue("@BookID", bookId);
                        updateAvailabilityCommand.ExecuteNonQuery();

                    }

                    connection.Close();
                    book.Availability = true;
                    return book; // Return successful
                }
                else
                {
                    connection.Close();
                    return book;
                }
            }
        }
    private Book getBook(int bookId,SqlConnection connection)
        {
            Book book=new Book();
            string availabilityQuery = "SELECT * FROM Books WHERE BookID = @BookID";
            using (SqlCommand command = new SqlCommand(availabilityQuery, connection))
            {
                command.Parameters.AddWithValue("@BookID", bookId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            // Assuming you have columns like BookId, Title, Author in your Books table
                            book.BookID = reader.GetInt32(reader.GetOrdinal("BookId"));
                            book.Title = reader.GetString(reader.GetOrdinal("Title"));
                            book.Author = reader.GetString(reader.GetOrdinal("Author"));
                            book.ISBN = reader.GetString(reader.GetOrdinal("ISBN"));
                            book.Availability = Convert.ToBoolean(reader["Availability"]);
                            return book;
                            
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return null;
        }   
    }
}
