using System.Windows;
using Microsoft.Practices.Unity;
using Prism.Unity;
using StudyAid.Gui.Views;

namespace StudyAid.Gui
{

    public class ApplicationBootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterTypeForNavigation<AddBook>();
            Container.RegisterTypeForNavigation<AddAuthor>();
            Container.RegisterTypeForNavigation<FindBook>();
        }
    }
}
