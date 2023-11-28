using Bussinesslogic;
using BussinessObject;
using Library.CustomFilter;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    [CustomExceptionHandlerFilter]
    public class HomeController : Controller
    {
        [NonAction]
        public string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }
        public ActionResult Index(string BookTitle="",string BookAuthor = "",string ISBN = "")
        {
           
            var libraryService = new LibraryService(GetConnectionString());
            List<Book> books =libraryService.SearchBooks(BookTitle,BookAuthor,ISBN);
            return View(books);
        }
        public ActionResult BorrowBook(int id)
        {
            var libraryService = new LibraryService(GetConnectionString());
            var book=libraryService.BorrowBook(bookId: id);
            return PartialView("_ViewBook", book);

        }
        public ActionResult ReturnBook(int id)
        {
            var libraryService = new LibraryService(GetConnectionString());
           var book= libraryService.ReturnBook(id);
            return PartialView("_ViewBook",book);
        }
       
    }
}