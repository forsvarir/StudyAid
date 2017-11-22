using Moq;
using NUnit.Framework;
using Prism.Regions;
using StudyAid.Contracts;
using StudyAid.Gui.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyAid.Gui.Tests.ViewModels
{
    [TestFixture]
    public class FindBookViewModelTests
    {
        [Test]
        public void TitleChangeShouldUpdateSubscribers()
        {
            const string initialValue = "Text To find";
            const string expectedValue = "New text to find";

            int propertyChangedNotificationCount = 0;
            var viewModel = CreatePopulatedViewModel((vm) => vm.TextToFind= initialValue);

            viewModel.PropertyChanged += (send, pcea) => {
                propertyChangedNotificationCount++;
                Assert.AreEqual(pcea.PropertyName, "TextToFind");
            };

            viewModel.TextToFind = expectedValue;

            Assert.AreEqual(1, propertyChangedNotificationCount);
            Assert.AreEqual(expectedValue, viewModel.TextToFind);
        }

        [Test]
        public void SearchCommandShouldPassTextToFindAndUpdateResults()
        {
            const string textToFind = "text to find";
            var searchResults = new List<Book>
            {
                new Book{},
                new Book{},
                new Book{}
            };

            var serviceMock = new Mock<IBookService>();
            serviceMock.Setup(x => x.FindBooks(textToFind)).Returns(searchResults).Verifiable();

            var viewModel = CreatePopulatedViewModel((vm) => vm.TextToFind = textToFind, bookService: serviceMock.Object);

            viewModel.SearchForBooksCommand.Execute();

            Assert.AreEqual(3, viewModel.SearchResults.Count);
            serviceMock.Verify();
        }

        private FindBookViewModel CreatePopulatedViewModel(Action<FindBookViewModel> postCreationAction = null, 
                                                           IBookService bookService = null,
                                                           IRegionManager regionManager = null)
        {
            var viewModel = new FindBookViewModel(bookService ?? new Mock<IBookService>().Object, regionManager ?? new Mock<IRegionManager>().Object)
                                {
                                    TextToFind = "A book title"
                                };

            postCreationAction?.Invoke(viewModel);

            return viewModel;
        }
    }
}
