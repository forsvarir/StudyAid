using Moq;
using NUnit.Framework;
using Prism.Regions;
using StudyAid.Gui.ViewModels;
using System;

namespace StudyAid.Gui.Tests.ViewModels
{
    [TestFixture]
    public class NavigableViewModelBaseTests
    {
        [Test]
        public void ConstructorShouldThrowOnNullRegionManager()
        {
            var thrown = Assert.Throws<ArgumentNullException>(()=>new NavigableViewModelBase(null));
            Assert.AreEqual("regionManager", thrown.ParamName);
        }

        [TestCase(null)]
        [TestCase("  ")]
        [TestCase("")]
        public void NavigateToEmptyUrlShouldThrow(string emptyUri)
        {
            var regionManagerMock = new Mock<IRegionManager>();

            var sut = new NavigableViewModelBase(regionManagerMock.Object);

            var thrown = Assert.Throws<ArgumentException>(() => sut.NavigateCommand.Execute(emptyUri));

            Assert.AreEqual("uri", thrown.ParamName);
        }

        [Test]
        public void NavigateToUriShouldNavigateContentRegionViaManager()
        {
            const string expectedUri = "Test Uri";

            var regionManagerMock = new Mock<IRegionManager>();

            var sut = new NavigableViewModelBase(regionManagerMock.Object);

            sut.NavigateCommand.Execute(expectedUri);

            regionManagerMock.Verify(rm=>rm.RequestNavigate(ContentRegions.MainContentRegion, expectedUri));
        }
    }
}
