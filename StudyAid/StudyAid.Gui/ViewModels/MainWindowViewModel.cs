using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Windows;

namespace StudyAid.Gui.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        public DelegateCommand<string> NavigateCommand { get; private set; }

        public DelegateCommand ExitCommand { get; private set; }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            NavigateCommand =  new DelegateCommand<string>(Navigate);
            ExitCommand = new DelegateCommand(Application.Current != null ? Application.Current.Shutdown : new System.Action(() => { }));
        }

        private void Navigate(string uri)
        {
            _regionManager.RequestNavigate("ContentRegion", uri);
        }
    }
}
