using Moq;
using NUnit.Framework;
using Prism.Regions;
using StudyAid.Contracts;
using StudyAid.Gui.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace StudyAid.Gui.Tests.ViewModels
{
    [TestFixture]
    public class AddAuthorToBookViewModelTests
    {
        [Test]
        public void NameChangeShouldUpdateSubscribers()
        {
            const string initialValue = "Default name";
            const string expectedValue = "New name";

            int propertyChangedNotificationCount = 0;
            var viewModel = CreatePopulatedViewModel((vm) => vm.Name = initialValue);

            viewModel.PropertyChanged += (send, pcea) => {
                propertyChangedNotificationCount++;
                Assert.AreEqual(pcea.PropertyName, "Name");
            };

            viewModel.Name = expectedValue;

            Assert.AreEqual(1, propertyChangedNotificationCount);
            Assert.AreEqual(expectedValue, viewModel.Name);
        }

        const string DefaultAuthorName = "Unknown Individual";

        private AddAuthorToBookViewModel CreatePopulatedViewModel(Action<AddAuthorToBookViewModel> postCreationAction = null, ObservableCollection<Author> authors = null)
        {
            var vm = new AddAuthorToBookViewModel(new Moq.Mock<Prism.Regions.IRegionManager>().Object) { Name = DefaultAuthorName };

            postCreationAction?.Invoke(vm);

            var populatedNavigationContext = CreateNavigationContext(true, authors ?? new ObservableCollection<Author>());

            vm.OnNavigatedTo(populatedNavigationContext);


            return vm;
        }

        [Test]
        public void AddAuthorCommandShouldBeDisabledWithEmptyName()
        {
            var viewModel = CreatePopulatedViewModel((vm) => vm.Name = "");

            Assert.AreEqual(false, viewModel.AddAuthorCommand.CanExecute());
        }

        [Test]
        public void AddAuthorCommandShouldNoticeWhenNameChanges()
        {
            int canExecuteChangedCount = 0;
            var viewModel = CreatePopulatedViewModel();

            viewModel.AddAuthorCommand.CanExecuteChanged += (send, pcea) => canExecuteChangedCount++;

            viewModel.Name = "";

            Assert.AreEqual(1, canExecuteChangedCount);
        }

        [Test]
        public void AddAuthorCommandShouldBeEnabledWithAuthorFieldsPopulated()
        {
            var viewModel = CreatePopulatedViewModel();

            Assert.AreEqual(true, viewModel.AddAuthorCommand.CanExecute());
        }

        [Test]
        public void AddAuthorCommandShouldBeDisabledWhenThereIsNoCollectionToAddTo()
        {
            var viewModel = CreatePopulatedViewModel();

            var emptyNavigationContext = CreateNavigationContext(false);

            viewModel.OnNavigatedTo(emptyNavigationContext);

            Assert.AreEqual(false, viewModel.AddAuthorCommand.CanExecute());
        }

        [Test]
        public void AddAuthorCommandShouldBeDisabledWhenCollectionIsNull()
        {
            var viewModel = CreatePopulatedViewModel();

            var emptyNavigationContext = CreateNavigationContext(true);

            viewModel.OnNavigatedTo(emptyNavigationContext);

            Assert.AreEqual(false, viewModel.AddAuthorCommand.CanExecute());
        }

        [Test]
        public void AddAuthorCommandShouldInsertAuthorIntoCollection()
        {
            ObservableCollection<Author> authors = new ObservableCollection<Author>();
            var viewModel = CreatePopulatedViewModel(authors: authors);

            viewModel.AddAuthorCommand.Execute();

            Assert.AreEqual(1, authors.Count);
            Assert.AreEqual(DefaultAuthorName, authors[0].Name);
            Assert.AreEqual(0, authors[0].AuthorId);
        }

        private NavigationContext CreateNavigationContext(bool passParameters, ObservableCollection<Author> authors = null)
        {
            // Mocks are required, otherwise NavigationContext fails to construct properly and the
            // navigation parameters aren't copied across.
            var regionMock = new Mock<IRegion>();
            var regionNavigationServiceMock = new Mock<IRegionNavigationService>();
            regionNavigationServiceMock.SetupGet<IRegion>(x => x.Region).Returns(regionMock.Object);


            var np = new NavigationParameters
            {
                { "authors", authors }
            };
            var populatedNavigationContext = new NavigationContext(regionNavigationServiceMock.Object, new Uri("AddAuthorToBook", UriKind.Relative), passParameters ? np : null);

            return populatedNavigationContext;
        }


    }
}
