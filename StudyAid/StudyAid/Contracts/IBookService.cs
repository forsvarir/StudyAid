using System.Collections.Generic;

namespace StudyAid.Contracts
{
    public interface IBookService
    {
        Book AddBook(Book newBook);
        IEnumerable<Book> FindBooks(string searchString);
    }
}
