using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Regions;
using StudyAid.Contracts;
using System.Collections.ObjectModel;
using Prism.Commands;

namespace StudyAid.Gui.ViewModels
{
    /*        <Button Content="_Search" HorizontalAlignment="Left" Margin="10,80,0,0" VerticalAlignment="Top" Width="75" Command="{Binding SearchForBooksCommand}"/>
            <DataGrid Height="400" Margin="10,110,10,0" VerticalAlignment="Top" Width="auto" HorizontalAlignment="Stretch" AutoGenerateColumns="False" ItemsSource="{Binding MatchingBooks}">
    */

    public class FindBookViewModel : NavigableViewModelBase
    {
        private string _textToFind = null;
        private IBookService _bookService;

        public string TextToFind
        {
            get => _textToFind;
            set => SetProperty(ref _textToFind, value);
        }

        ObservableCollection<Book> _searchResults = new ObservableCollection<Book>();

        public ObservableCollection<Book> SearchResults
        {
            get => _searchResults;
            set => SetProperty(ref _searchResults, value);
        }

        public DelegateCommand SearchForBooksCommand { get; }



        public FindBookViewModel(IBookService bookService, IRegionManager regionManager) : base(regionManager)
        {
            _bookService = bookService;
            SearchForBooksCommand = new DelegateCommand(SearchForBooks);
        }

        private void SearchForBooks()
        {
            var found = _bookService.FindBooks(_textToFind);
            SearchResults.Clear();
            SearchResults.AddRange(found);
        }
    }
}
