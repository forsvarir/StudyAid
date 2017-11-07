using StudyAid.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyAid.TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new BookContext())
            {
                if (context.Books.Count() < 1)
                {
                    context.Books.Add(new Book() { Title = "My First Book", ISBN = "123456", Authors = new List<Author> { new Author { Name = "My Author" } } });
                }

                context.SaveChanges();

                foreach (var book in context.Books)
                {
                    Console.WriteLine($"ISBN: {book.ISBN} - {book.Title}");
                }

                foreach(var author in context.Authors)
                {
                    Console.WriteLine($"Author: {author.Name}");
                }
            }
        }
    }
}
