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
    /// Interaction logic for AddBook.xaml
    /// </summary>
    public partial class AddBookWindow : Window
    {
        BookContext _bookContext = new BookContext();
        public AddBookWindow()
        {
            InitializeComponent();
            AuthorGrid.ItemsSource = Authors;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if(Authors.Count > 0 && !string.IsNullOrEmpty(BookTitle.Text))
            {
                var book = new Book { Title = BookTitle.Text, Authors = Authors };

                foreach (var author in Authors)
                {
                    if(author.AuthorId > 0)
                    {
                        _bookContext.Set(typeof(Author)).Attach(author);
                    }
                }

                _bookContext.Books.Add(book);
                _bookContext.SaveChanges();
            }
            this.Close();
        }

        public ObservableCollection<Author> Authors { get; set; } = new ObservableCollection<Author> { };

        private void AddAuthor_Click(object sender, RoutedEventArgs e)
        {
            var addAuthorWindow = new AddAuthorWindow();
            addAuthorWindow.ShowDialog();
            if(addAuthorWindow.Author != null)
            {
                Authors.Add(addAuthorWindow.Author);
            }
        }
    }
}
