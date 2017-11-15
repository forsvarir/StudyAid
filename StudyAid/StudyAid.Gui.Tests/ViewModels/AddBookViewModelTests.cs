using Moq;
using NUnit.Framework;
using StudyAid.Contracts;
using StudyAid.Gui.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyAid.Gui.Tests.ViewModels
{
    [TestFixture]
    public class AddBookViewModelTests
    {
        [Test]
        public void TitleChangeShouldUpdateSubscribers()
        {
            const string initialValue = "Default title";
            const string expectedValue = "New title";

            int propertyChangedNotificationCount = 0;
            var viewModel = CreatePopulatedViewModel((vm)=>vm.Title = initialValue);

            viewModel.PropertyChanged += (send, pcea)=> {
                propertyChangedNotificationCount++;
                Assert.AreEqual(pcea.PropertyName, "Title");
            };

            viewModel.Title = expectedValue;

            Assert.AreEqual(1, propertyChangedNotificationCount);
            Assert.AreEqual(expectedValue, viewModel.Title);
        }

        [Test]
        public void ISBNChangeShouldUpdateSubscribers()
        {
            const string initialValue = "Default ISBN";
            const string expectedValue = "New ISBN";

            int propertyChangedNotificationCount = 0;
            var viewModel = CreatePopulatedViewModel((vm)=>vm.ISBN = initialValue);

            viewModel.PropertyChanged += (send, pcea) => {
                propertyChangedNotificationCount++;
                Assert.AreEqual(pcea.PropertyName, "ISBN");
            };

            viewModel.ISBN = expectedValue;

            Assert.AreEqual(1, propertyChangedNotificationCount);
            Assert.AreEqual(expectedValue, viewModel.ISBN);
        }

        [Test]
        public void AddBookCommandShouldBeDisabledWithEmptyTitle()
        {
            var viewModel = CreatePopulatedViewModel((vm) => vm.Title = "");

            Assert.AreEqual(false, viewModel.AddBookCommand.CanExecute());
        }

        [Test]
        public void AddBookCommandShouldBeDisabledWithEmptyISBN()
        {
            var viewModel = CreatePopulatedViewModel((vm) => vm.ISBN = "");

            Assert.AreEqual(false, viewModel.AddBookCommand.CanExecute());
        }

        [Test]
        public void AddBookCommandShouldBeDisabledWithNoAuthors()
        {
            var viewModel = CreatePopulatedViewModel((vm) => vm.Authors.Clear());

            Assert.AreEqual(false, viewModel.AddBookCommand.CanExecute());
        }

        [Test]
        public void AddBookCommandShouldNoticeWhenISBNChanges()
        {
            int canExecuteChangedCount = 0;
            var viewModel = CreatePopulatedViewModel();

            viewModel.AddBookCommand.CanExecuteChanged += (send, pcea) => canExecuteChangedCount++;

            viewModel.ISBN = "";

            Assert.AreEqual(1, canExecuteChangedCount);
        }

        [Test]
        public void AddBookCommandShouldNoticeWhenTitleChanges()
        {
            int canExecuteChangedCount = 0;
            var viewModel = CreatePopulatedViewModel();

            viewModel.AddBookCommand.CanExecuteChanged += (send, pcea) => canExecuteChangedCount++;

            viewModel.Title = "";

            Assert.AreEqual(1, canExecuteChangedCount);
        }

        [Test]
        public void AddBookCommandShouldNoticeWhenAuthorsChanges()
        {
            int canExecuteChangedCount = 0;
            var viewModel = CreatePopulatedViewModel();

            viewModel.AddBookCommand.CanExecuteChanged += (send, pcea) => canExecuteChangedCount++;

            viewModel.Authors.Clear();

            Assert.AreEqual(1, canExecuteChangedCount);
        }

        [Test]
        public void AddBookCommandShouldBeEnabledWithBookFieldsPopulated()
        {
            var viewModel = CreatePopulatedViewModel();

            Assert.AreEqual(true, viewModel.AddBookCommand.CanExecute());
        }

        [Test]
        public void AddBookCommandShouldPassCorrectInformationToBookService()
        {
            var expectedBook = new Book { Authors = new List<Author> { new Author { Name=DefaultAuthorName, AuthorId=DefaultAuthorId } }, ISBN = DefaultISBN, Title = DefaultTitle, BookId = 0 };
            Book actualBook = null;
            var serviceMock = new Mock<IBookService>();

            serviceMock.Setup(x => x.AddBook(It.IsAny<Book>()))
                       .Callback<Book>((book)=>actualBook = book);

            var viewModel = CreatePopulatedViewModel(null, serviceMock.Object);

            viewModel.AddBookCommand.Execute();

            Assert.AreEqual(expectedBook.Title, actualBook.Title);
            Assert.AreEqual(expectedBook.ISBN, actualBook.ISBN);
            Assert.AreEqual(expectedBook.BookId, actualBook.BookId);
            Assert.AreEqual(expectedBook.Authors.Count, actualBook.Authors.Count, "Unexpected number of authors!");

            foreach(var expectedAuthor in expectedBook.Authors)
            {
                Assert.AreEqual(1, actualBook.Authors.Where(a => a.Name == expectedAuthor.Name).Count());
            }
        }
        const int DefaultAuthorId = 1;
        const string DefaultAuthorName = "Deefault Test Author";
        const string DefaultISBN = "123456789";
        const string DefaultTitle = "Default Test Title";


        private AddBookViewModel CreatePopulatedViewModel(Action<AddBookViewModel> postCreationAction = null, IBookService bookService = null)
        {
            var bookVM = new AddBookViewModel (bookService, null)
            {
                Title = DefaultTitle,
                ISBN = DefaultISBN,
                Authors ={
                            new Author { Name = DefaultAuthorName, AuthorId = DefaultAuthorId}
                         }
            };

            postCreationAction?.Invoke(bookVM);

            return bookVM;
        }


    }
}
