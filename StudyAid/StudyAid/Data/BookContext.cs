using StudyAid.Contracts;
using System.Data.Entity;

namespace StudyAid.Data
{
    public class BookContext : DbContext
    {
        public BookContext() : base("BookConnection") { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

    }
}
