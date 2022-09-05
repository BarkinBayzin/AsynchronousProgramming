using AsynchronousProgramming.Models.Entities.Abstract;

namespace AsynchronousProgramming.Models.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public Status Status { get; set; }
        public decimal UnitPrice { get; set; }
        public string Image { get; set; }
        public string CategoryName { get; set; }

    }
}
