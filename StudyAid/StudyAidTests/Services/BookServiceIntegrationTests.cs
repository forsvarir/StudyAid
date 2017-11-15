using NUnit.Framework;
using StudyAid.Contracts;
using StudyAid.Services;
using System;
using System.Collections.Generic;

namespace StudyAidTests.Services
{
    [TestFixture, Category("Integration")]
    public class BookServiceIntegrationTests
    {
        private Book CreateBook(Action<Book> bookAction = null)
        {
            var book = new Book { Title = "Default Test Title",
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

        private static IBookService CreateBookService()
        {
            return new BookService();
        }

        [Test]
        public void InsertBookShouldUpdateBookId()
        {
            var newBook = CreateBook((book)=>book.BookId = 0);

            var service = new BookService();

            var insertedBook = service.AddBook(newBook);

            Assert.That(newBook.BookId != 0, ()=>"BookId should be updated by AddBook");
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

        // TODO: Validate that inserted book has actually been inserted
        // TODO: Validate that inserting book with new author, also inserts the author
        // TODO: Validate that inserting book with existing authors links to author
        // TODO: Validate that inserting an existing book fails

    }
}
