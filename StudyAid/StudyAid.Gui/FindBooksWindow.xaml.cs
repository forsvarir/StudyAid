using StudyAid.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StudyAid.Gui
{
    /// <summary>
    /// Interaction logic for FindBooksWindow.xaml
    /// </summary>
    public partial class FindBooksWindow : Window
    {
        BookContext _bookContext = new BookContext();
        ObservableCollection<Book> Books { get; set; } = new ObservableCollection<Book>();

        public FindBooksWindow()
        {
            InitializeComponent();
            BookGrid.ItemsSource = Books;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Books.Clear();            
            var books = _bookContext.Books.Where(book => book.Title.ToUpper().Contains(SearchText.Text.ToUpper())).ToList();
            if(null != books)
            {
                books.ForEach(book => Books.Add(book));
            }
        }
    }
}
