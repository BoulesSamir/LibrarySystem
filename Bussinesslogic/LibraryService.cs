using BussinessObject;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bussinesslogic
{
    public class LibraryService 
    { 
      private LibraryRepository repository;

    public LibraryService(string connectionString)
    {
        this.repository = new LibraryRepository(connectionString);
    }

    public List<Book> SearchBooks(string searchTerm)
    {
        // Basic input validation
        if (string.IsNullOrEmpty(searchTerm))
        {
            throw new ArgumentException("Search term cannot be empty.");
        }

        // Call the repository method to search for books
        return repository.SearchBooks(searchTerm);
    }

    public bool BorrowBook(int userId, int bookId)
    {
        // Basic input validation
        if (userId <= 0 || bookId <= 0)
        {
            throw new ArgumentException("Invalid user ID or book ID.");
        }

        // Call the repository method to borrow a book
        return repository.BorrowBook(userId, bookId);
    }

    public bool ReturnBook(int borrowingId)
    {
        // Basic input validation
        if (borrowingId <= 0)
        {
            throw new ArgumentException("Invalid borrowing ID.");
        }

        // Call the repository method to return a book
        return repository.ReturnBook(borrowingId);
    }
}
}
