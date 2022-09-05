using AsynchronousProgramming.Models.Entities.Abstract;
using System.Collections.Generic;

namespace AsynchronousProgramming.Models.Entities.Concrete
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string Slug { get; set; }

        //Relational Properties Begin
        public List<Product> Products { get; set; }
    }
}
