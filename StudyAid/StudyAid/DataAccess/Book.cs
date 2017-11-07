using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyAid.DataAccess
{
    public class Book
    {
        public int BookId { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }

        public ICollection<Author> Authors {get;set;}
    }
}
