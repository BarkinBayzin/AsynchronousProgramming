using AsynchronousProgramming.Models.Entities.Abstract;

namespace AsynchronousProgramming.Models.Entities.Concrete
{
    public class Page : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Slug { get; set; }
    }
}
