using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Windows;

namespace StudyAid.Gui.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        public DelegateCommand<string> NavigateCommand => new DelegateCommand<string>(Navigate);

        public DelegateCommand ExitCommand => new DelegateCommand(Application.Current != null ? Application.Current.Shutdown : new System.Action(() => { }));

        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        private void Navigate(string uri)
        {
            _regionManager.RequestNavigate("ContentRegion", uri);
        }
    }
}
