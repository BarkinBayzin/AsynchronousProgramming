using AsynchronousProgramming.Models.Entities.Abstract;
using AsynchronousProgramming.Models.Entities.Concrete;
using System;
using System.Collections.Generic;

namespace AsynchronousProgramming.Models.ViewModels
{
    public class CategoryVM
    {
        public CategoryVM()
        {
            Categories = new List<Category>();
        }
        public List<Category> Categories{ get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public Status Status { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
