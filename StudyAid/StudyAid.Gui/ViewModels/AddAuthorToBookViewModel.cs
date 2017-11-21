using Prism.Commands;
using Prism.Regions;
using StudyAid.Contracts;
using System.Collections.ObjectModel;

namespace StudyAid.Gui.ViewModels
{
    public class AddAuthorToBookViewModel : NavigableViewModelBase, INavigationAware
    {
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        ObservableCollection<Author> _authors;
        private IRegionNavigationJournal _rnj = null;

        public DelegateCommand AddAuthorCommand { get; }


        public AddAuthorToBookViewModel(IRegionManager regionManager) : base(regionManager)
        {
            AddAuthorCommand = new DelegateCommand(AddAuthor, CanAddAuthor).ObservesProperty(() => Name);
        }

        private bool CanAddAuthor()
        {
            return !string.IsNullOrEmpty(_name) && null != _authors;
        }

        private void AddAuthor()
        {
            _authors.Add(new Author { Name = _name });
            _rnj?.GoBack();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _authors = navigationContext.Parameters["authors"] as ObservableCollection<Author>;
            _rnj = navigationContext?.NavigationService.Journal;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
