using System.Collections.Generic;

namespace StudyAid.Contracts
{
    public class Book
    {
        public int BookId { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }

        public ICollection<Author> Authors { get; set; }
    }

    public class Author
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }

        public ICollection<Book> Books { get; set; }
    }


    public interface IBookService
    {
        Book AddBook(Book newBook);
    }
}
