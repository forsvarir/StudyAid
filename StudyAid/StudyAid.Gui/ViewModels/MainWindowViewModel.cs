using Prism.Commands;
using Prism.Mvvm;
using System.Windows;

namespace StudyAid.Gui.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public DelegateCommand ExitCommand => new DelegateCommand(Application.Current.Shutdown);
    }
}
