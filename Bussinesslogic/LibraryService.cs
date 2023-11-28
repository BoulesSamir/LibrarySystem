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

    public List<Book> SearchBooks(string BookTitle , string BookAuthor, string ISBN)
    {

        // Call the repository method to search for books
        return repository.SearchBooks(BookTitle, BookAuthor, ISBN);
    }

    public Book BorrowBook( int bookId)
    {
        // Basic input validation
        if (bookId <= 0)
        {
            throw new ArgumentException("Invalid user ID or book ID.");
        }

        // Call the repository method to borrow a book
        return repository.BorrowBook(bookId);
    }

    public Book ReturnBook(int BookId)
    {
        // Basic input validation
        if (BookId <= 0)
        {
            throw new ArgumentException("Invalid borrowing ID.");
        }

        // Call the repository method to return a book
        return repository.ReturnBook(BookId);
    }
}
}
