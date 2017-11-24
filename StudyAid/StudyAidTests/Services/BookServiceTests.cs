using NUnit.Framework;
using StudyAid.Contracts;
using StudyAid.Data;
using StudyAid.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using StudyAidTests.Helpers;
using System.Linq;

namespace StudyAidTests.Services
{
    [TestFixture]
    public class BookServiceTests
    {
        private Book CreateBook(Action<Book> bookAction = null)
        {
            var book = new Book
            {
                Title = "Default Test Title",
                ISBN = "123456789",
                BookId = 0,
                Authors = new List<Author>()
                                            {
                                                new Author { Name = "Default Test Author"}
                                            }
            };

            bookAction?.Invoke(book);

            return book;
        }

        [Test]
        public void BookServiceShouldBeConstructable()
        {
            var service = CreateBookService();
            Assert.IsNotNull(service);
        }


        [Test]
        public void InsertBookShouldFailWhenBookIsNull()
        {
            var service = new BookService();

            Assert.That(() => service.AddBook(null),
            Throws.Exception
              .TypeOf<ArgumentNullException>());
        }


        [TestCase("")]
        [TestCase(null)]
        public void InsertBookShouldFailWhenTitleIsNotPresent(string titleToTest)
        {
            var newBook = CreateBook((book) => book.Title = titleToTest);

            var service = new BookService();

            Assert.That(() => service.AddBook(newBook),
            Throws.Exception
              .TypeOf<ArgumentException>()
              .With.Message.Contains("Title"));
        }

        [TestCase("")]
        [TestCase(null)]
        public void InsertBookShouldFailWhenISBNIsNotPresent(string isbnToTest)
        {
            // We are assuming that all books have an ISBN.  This may require revisited if that's not the case.
            var newBook = CreateBook((book) => book.ISBN = isbnToTest);

            var service = new BookService();

            Assert.That(() => service.AddBook(newBook),
            Throws.Exception
              .TypeOf<ArgumentException>()
              .With.Message.Contains("ISBN"));
        }

        [Test]
        public void InsertBookShouldFailWhenNoAuthorsProvided()
        {
            var newBook = CreateBook((book) => book.Authors.Clear());

            var service = new BookService();

            Assert.That(() => service.AddBook(newBook),
            Throws.Exception
              .TypeOf<ArgumentException>()
              .With.Message.Contains("author"));
        }

        [Test]
        public void InsertBookShouldFailWhenNullAuthorsProvided()
        {
            var newBook = CreateBook((book) => book.Authors = null);

            var service = new BookService();

            Assert.That(() => service.AddBook(newBook),
            Throws.Exception
              .TypeOf<ArgumentException>()
              .With.Message.Contains("author"));
        }

        [Test]
        public void ShouldBeAbleToFindBookByExactTitle()
        {
            const string expectedBookTitle = searchableBookTitle;
            var service = CreateBookService();

            var books = service.FindBooks(expectedBookTitle);
            Assert.IsNotNull(books);

            var booksList = books.ToList();

            Assert.AreEqual(1, booksList.Count);
            Assert.AreEqual(expectedBookTitle, booksList[0].Title);
        }

        [Test]
        public void SearchByExactTitleShouldFindMatchingBook()
        {
            const string textToSearchWith = "Book to Find";
            const string expectedBookTitle = "Book to Find";
            var service = CreateBookService();

            var books = service.FindBooks(textToSearchWith);
            Assert.IsNotNull(books);

            var booksList = books.ToList();

            Assert.AreEqual(1, booksList.Count);
            Assert.AreEqual(expectedBookTitle, booksList[0].Title);
        }

        [Test]
        public void CaseMissmatchShouldFindMatchingBook()
        {
            const string textToSearchWith = "bOOK TO find";
            const string expectedBookTitle = "Book to Find";
            var service = CreateBookService();

            var books = service.FindBooks(textToSearchWith);
            Assert.IsNotNull(books);

            var booksList = books.ToList();

            Assert.AreEqual(1, booksList.Count);
            Assert.AreEqual(expectedBookTitle, booksList[0].Title);
        }

        [Test]
        public void SearchByPartialTitleShouldFindMatchingBooks()
        {
            const string textToSearchWith = "ook";
            var service = CreateBookService();

            var books = service.FindBooks(textToSearchWith);
            Assert.IsNotNull(books);

            var booksList = books.ToList();

            AssertContainsAllBooks(booksList);
        }

        [Test]
        public void EmptySearchTextShouldFindAllBooks()
        {
            const string textToSearchWith = "";
            var service = CreateBookService();

            var books = service.FindBooks(textToSearchWith);
            Assert.IsNotNull(books);

            var booksList = books.ToList();

            AssertContainsAllBooks(booksList);
        }

        [Test]
        public void NullSearchTextShouldThrow()
        {
            var service = CreateBookService();

            var exception = Assert.Throws<ArgumentNullException>(()=>service.FindBooks(null));
            Assert.AreEqual("searchString", exception.ParamName);
        }


        private static IBookService CreateBookService()
        {
            List<Book> initialBookList = new List<Book> { new Book { Title = searchableBookTitle, ISBN = searchableBookISBN, BookId = searchableBookId },
                                                          new Book { Title = searchableBook2Title, ISBN = searchableBook2ISBN, BookId = searchableBook2Id } };

            return new BookService(new StubbedBookContext(initialBookList));
        }

        private void AssertContainsAllBooks(List<Book> booksList)
        {
            Assert.AreEqual(2, booksList.Count);
            Assert.IsTrue(booksList.Where(x => x.Title == searchableBookTitle).Count() == 1);
            Assert.IsTrue(booksList.Where(x => x.Title == searchableBook2Title).Count() == 1);
        }

        const string searchableBookTitle = "Book to Find";
        const string searchableBookISBN = "1234567";
        const int searchableBookId = 5;
        const string searchableBook2Title = "Unique Title for book";
        const string searchableBook2ISBN = "223456";
        const int searchableBook2Id = 3;


    }

    class StubbedBookContext : IBookContext
    {
        public DbSet<Book> Books { get; }

        public DbSet<Author> Authors { get; }

        public StubbedBookContext(IEnumerable<Book> initialBookList = null, IEnumerable<Author> initialAuthorList = null)
        {
            Books = new TestDbSet<Book>(initialBookList);
            Authors = new TestDbSet<Author>(initialAuthorList);
        }

        public int SaveChanges()
        {
            return 0;
        }
    }
}
