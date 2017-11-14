using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using StudyAid.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyAid.Gui.ViewModels
{

    public class AddBookViewModel : BindableBase
    {
        private string _title = null;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _isbn = null;
        public string ISBN {
            get => _isbn;
            set => SetProperty(ref _isbn, value);
        }

        ObservableCollection<Author> _authors = new ObservableCollection<Author>();

        public ObservableCollection<Author> Authors {
            get =>_authors;
            set => SetProperty(ref _authors, value);
        }

        private readonly IBookService _bookService;
        private readonly IRegionManager _regionManager;

        public DelegateCommand AddBookCommand { get; set; }

        public AddBookViewModel(IBookService bookService, IRegionManager regionManager)
        {
            _bookService = bookService;
            _regionManager = regionManager;

            AddBookCommand = new DelegateCommand(Execute, CanExecute).ObservesProperty(() => Title)
                                                                     .ObservesProperty(() => ISBN);
            _authors.CollectionChanged += (send, pcea) => AddBookCommand.RaiseCanExecuteChanged();

            NavigateCommand = new DelegateCommand<string>(Navigate);

            _authors.Add(new Author { Name = "My Temp Author" });
        }

        private bool CanExecute()
        {
            return !string.IsNullOrWhiteSpace(_title) && !string.IsNullOrWhiteSpace(_isbn) && _authors.Count> 0;
        }

        private void Execute()
        {
            _bookService.AddBook(new Book() { Title = _title, ISBN = _isbn, Authors=_authors });
        }

        public DelegateCommand<string> NavigateCommand { get; private set; }
        private void Navigate(string uri)
        {
            _regionManager.RequestNavigate("ContentRegion", uri);
        }


    }
}
