using NUnit.Framework;
using StudyAid.Gui.ViewModels;
using System;

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

        private AddAuthorToBookViewModel CreatePopulatedViewModel(Action<AddAuthorToBookViewModel> postCreationAction = null)
        {
            var vm = new AddAuthorToBookViewModel(new Moq.Mock<Prism.Regions.IRegionManager>().Object) { Name = DefaultAuthorName };

            postCreationAction?.Invoke(vm);

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
    }
}
