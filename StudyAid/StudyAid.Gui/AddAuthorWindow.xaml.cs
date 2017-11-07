using StudyAid.DataAccess;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for AddAuthorWindow.xaml
    /// </summary>
    public partial class AddAuthorWindow : Window
    {
        BookContext _bookContext = new BookContext();
        private Author _author;

        public Author Author => _author;

        public AddAuthorWindow()
        {
            InitializeComponent();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            var existingAuthor = _bookContext.Authors.Where(author => author.Name.ToUpper().Contains(AuthorName.Text.ToUpper())).FirstOrDefault();
            if(existingAuthor != null)
            {
                AuthorName.Text = existingAuthor.Name;
                _author = existingAuthor;
            }
        }

        private void AuthorChanged(object sender, TextChangedEventArgs e)
        {
            _author = null;
        }

        private void Add_Clicked(object sender, RoutedEventArgs e)
        {
            if(_author == null)
            {
                _author = new Author { Name = AuthorName.Text };
            }
            this.Close();
        }
    }
}
