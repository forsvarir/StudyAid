using System.Collections.Generic;

namespace StudyAid.Contracts
{
    public class Book
    {
        public int BookId { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }

        public ICollection<Author> Authors { get; set; }
    }
}
