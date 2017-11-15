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

    public class AddBookViewModel : NavigableViewModelBase
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

        public DelegateCommand AddBookCommand { get;} 
        public DelegateCommand DiscardBookCommand { get; }

        public AddBookViewModel(IBookService bookService, IRegionManager regionManager) : base(regionManager)
        {
            _bookService = bookService;

            AddBookCommand = new DelegateCommand(AddBook, CanAddBook).ObservesProperty(() => Title)
                                                                     .ObservesProperty(() => ISBN);
            DiscardBookCommand = new DelegateCommand(DiscardBook);
            _authors.CollectionChanged += (send, pcea) => AddBookCommand.RaiseCanExecuteChanged();
        }

        private bool CanAddBook()
        {
            return !string.IsNullOrWhiteSpace(_title) && !string.IsNullOrWhiteSpace(_isbn) && _authors.Count> 0;
        }

        private void AddBook()
        {
            _bookService.AddBook(new Book() { Title = _title, ISBN = _isbn, Authors=_authors });
        }

        private void DiscardBook()
        {
            Title = String.Empty;
            ISBN = String.Empty;
            Authors.Clear();
        }
    }
}
