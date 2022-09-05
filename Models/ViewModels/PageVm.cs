using AsynchronousProgramming.Models.Entities.Abstract;
using AsynchronousProgramming.Models.Entities.Concrete;
using System.Collections.Generic;

namespace AsynchronousProgramming.Models.ViewModels
{
    public class PageVm
    {
        public PageVm()
        {
            Pages = new List<Page>();
        }
        public List<Page> Pages{ get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Slug { get; set; }
        public Status Status { get; set; }
    }
}
