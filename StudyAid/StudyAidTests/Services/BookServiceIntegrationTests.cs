﻿using NUnit.Framework;
using StudyAid.Contracts;
using StudyAid.Services;
using System;
using System.Collections.Generic;
using System.Linq;

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

        private static IBookService CreateBookService()
        {
            return new BookService();
        }

        // BookId is updated automatically by the EF when an item is inserted.
        // This integration test confirms that no errors have been thrown.
        [Test]
        public void InsertBookShouldUpdateBookId()
        {
            var newBook = CreateBook((book)=>book.BookId = 0);

            var service = new BookService();

            var insertedBook = service.AddBook(newBook);

            Assert.That(newBook.BookId != 0, ()=>"BookId should be updated by AddBook");
        }

        // Note: This test will fail if it is run against a clean database.
        [Test]
        public void FindBooksShouldReturnAtLeastOneBook()
        {
            var service = new BookService();

            var foundBooks = service.FindBooks("").ToList();

            Assert.Greater(foundBooks.Count, 0, "Expected to find at least one book");
            Assert.Greater(foundBooks[0].BookId, 0, "Books fetched should have a book id");
        }

        // TODO: Validate that inserted book has actually been inserted
        // TODO: Validate that inserting book with new author, also inserts the author
        // TODO: Validate that inserting book with existing authors links to author
        // TODO: Validate that inserting an existing book fails

    }
}
