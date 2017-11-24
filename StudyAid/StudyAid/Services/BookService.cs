using StudyAid.Contracts;
using StudyAid.Data;
using System.Linq;
using System;
using System.Collections.Generic;

namespace StudyAid.Services
{
    public class BookService : IBookService
    {
        IBookContext _bookContext = new BookContext();

        public BookService(IBookContext bookContext = null)
        {
            _bookContext = bookContext ?? new BookContext();

        }

        public Book AddBook(Book newBook)
        {
            if(null == newBook)
            {
                throw new ArgumentNullException("newBook");
            }
            if(string.IsNullOrEmpty(newBook.Title))
            {
                throw new ArgumentException("Title is a required field.", "newBook");
            }
            if(string.IsNullOrEmpty(newBook.ISBN))
            {
                throw new ArgumentException("ISBN is a required field.", "newBook");
            }
            if(null == newBook.Authors || 0 >= newBook.Authors.Count)
            {
                throw new ArgumentException("At least one author should be provided", "newBook");
            }


            _bookContext.Books.Add(newBook);
            _bookContext.SaveChanges();

            return newBook;
        }

        public IEnumerable<Book> FindBooks(string searchString)
        {
            if(null == searchString)
            {
                throw new ArgumentNullException("searchString");
            }
            return _bookContext.Books.Where(book => searchString == String.Empty || book.Title.ToUpper().Contains(searchString.ToUpper())).AsEnumerable();
        }
    }
}
