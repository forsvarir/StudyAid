using Prism.Commands;
using Prism.Regions;

namespace StudyAid.Gui.ViewModels
{
    public class AddAuthorToBookViewModel : NavigableViewModelBase
    {
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public DelegateCommand AddAuthorCommand { get; }


        public AddAuthorToBookViewModel(IRegionManager regionManager) : base(regionManager)
        {
            AddAuthorCommand = new DelegateCommand(AddAuthor, CanAddAuthor).ObservesProperty(() => Name);
        }

        private bool CanAddAuthor()
        {
            return !string.IsNullOrEmpty(_name);
        }

        private void AddAuthor()
        {
            return;
        }
    }
}
