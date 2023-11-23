using BussinessObject;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        public List<Book> SearchBooks(string searchTerm)
        {
            List<Book> books = new List<Book>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Books WHERE Title LIKE @SearchTerm OR Author LIKE @SearchTerm OR ISBN LIKE @SearchTerm";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");

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

        public bool BorrowBook(int userId, int bookId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Check if the book is available
                string availabilityQuery = "SELECT Availability FROM Books WHERE BookID = @BookID";
                using (SqlCommand availabilityCommand = new SqlCommand(availabilityQuery, connection))
                {
                    availabilityCommand.Parameters.AddWithValue("@BookID", bookId);

                    object result = availabilityCommand.ExecuteScalar();
                    if (result != null && Convert.ToBoolean(result))
                    {
                        // Book is available, proceed with borrowing
                        string borrowQuery = "INSERT INTO Borrowings (UserID, BookID, BorrowDate) VALUES (@UserID, @BookID, @BorrowDate)";
                        using (SqlCommand borrowCommand = new SqlCommand(borrowQuery, connection))
                        {
                            borrowCommand.Parameters.AddWithValue("@UserID", userId);
                            borrowCommand.Parameters.AddWithValue("@BookID", bookId);
                            borrowCommand.Parameters.AddWithValue("@BorrowDate", DateTime.Now);

                            borrowCommand.ExecuteNonQuery();

                            // Update book availability status
                            string updateAvailabilityQuery = "UPDATE Books SET Availability = 0 WHERE BookID = @BookID";
                            using (SqlCommand updateAvailabilityCommand = new SqlCommand(updateAvailabilityQuery, connection))
                            {
                                updateAvailabilityCommand.Parameters.AddWithValue("@BookID", bookId);
                                updateAvailabilityCommand.ExecuteNonQuery();
                            }

                            return true; // Borrowing successful
                        }
                    }
                    else
                    {
                        return false; // Book is not available
                    }
                }
            }
        }

        public bool ReturnBook(int borrowingId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Update return date for the borrowing
                string returnQuery = "UPDATE Borrowings SET ReturnDate = @ReturnDate WHERE BorrowingID = @BorrowingID";
                using (SqlCommand returnCommand = new SqlCommand(returnQuery, connection))
                {
                    returnCommand.Parameters.AddWithValue("@ReturnDate", DateTime.Now);
                    returnCommand.Parameters.AddWithValue("@BorrowingID", borrowingId);

                    returnCommand.ExecuteNonQuery();

                    // Update book availability status
                    string updateAvailabilityQuery = "UPDATE Books SET Availability = 1 WHERE BookID IN (SELECT BookID FROM Borrowings WHERE BorrowingID = @BorrowingID)";
                    using (SqlCommand updateAvailabilityCommand = new SqlCommand(updateAvailabilityQuery, connection))
                    {
                        updateAvailabilityCommand.Parameters.AddWithValue("@BorrowingID", borrowingId);
                        updateAvailabilityCommand.ExecuteNonQuery();
                    }

                    return true; // Return successful
                }
            }
        }
    }
}
