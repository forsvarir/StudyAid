using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Windows;

namespace StudyAid.Gui.ViewModels
{
    public class MainWindowViewModel : NavigableViewModelBase
    {
        public DelegateCommand ExitCommand { get; }

        public MainWindowViewModel(IRegionManager regionManager) : base(regionManager)
        {
            ExitCommand = new DelegateCommand(Application.Current != null ? Application.Current.Shutdown : new System.Action(() => { }));
        }
    }
}
