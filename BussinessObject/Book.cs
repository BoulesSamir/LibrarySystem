using System;
using System.Collections.Generic;
using System.Text;

namespace BussinessObject
{
    public class Book
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public bool Availability { get; set; }
    }
}
