using StudyAid.Contracts;
using System.Data.Entity;

namespace StudyAid.Data
{
    public interface IBookContext
    {
        DbSet<Book> Books { get; }
        DbSet<Author> Authors { get; }

        int SaveChanges();
    }

    public class BookContext : DbContext, IBookContext
    {
        public BookContext() : base("BookConnection") { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

    }
}
