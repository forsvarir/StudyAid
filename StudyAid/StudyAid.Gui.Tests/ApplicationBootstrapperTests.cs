using NUnit.Framework;
using Prism.Regions;
using StudyAid.Gui.ViewModels;
using StudyAid.Gui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StudyAid.Gui.Tests
{
    [TestFixture, Apartment(ApartmentState.STA)]
    public class ApplicationBootstrapperTests
    {
        private static TestableBootstrapper _bootStrapper;

        [OneTimeSetUp]
        public static void SetupFixture()
        {
            _bootStrapper = new TestableBootstrapper();
            _bootStrapper.Run();
        }

        [OneTimeTearDown]
        public static void TeardownFixture()
        {
            _bootStrapper = null;
        }

        [Test]
        public void AllNonWindowViewsShouldBeNavigable()
        {
            var container = _bootStrapper.Container;

            IRegionManager regionManager = (IRegionManager)container.Resolve(typeof(IRegionManager), "");

            Assembly guiAssembly = typeof(MainWindow).GetTypeInfo().Assembly;

            Type[] nonWindowViews = guiAssembly.GetTypes().Where(t => String.Equals(t.Namespace, "StudyAid.Gui.Views") 
                                                       && !t.Name.Contains("Window")).ToArray();

            foreach(var view in nonWindowViews)
            {
                regionManager.RequestNavigate("ContentRegion", view.Name);

                Assert.IsTrue(regionManager.Regions.First().Views.Where(x => x.GetType() == view).Count()==1, $"{view.Name} wasn't registered for navigation, check ConfigureContainer.  Did you add any missing dependencies?");
            }
        }
    }

    public class TestableBootstrapper : ApplicationBootstrapper
    {
        protected override void InitializeShell()
        {
            // Don't invoke initialize on base, to prevent window being displayed
        }
    }
}
