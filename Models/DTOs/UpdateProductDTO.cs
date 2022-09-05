using AsynchronousProgramming.Models.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsynchronousProgramming.Models.DTOs
{
    public class UpdateProductDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please type into product name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please type into product content")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please type into product price")]
        [Column(TypeName = "decimal(4,2)")]
        public decimal UnitPrice { get; set; }
        public string Image { get; set; }
        public IFormFile UploadImage { get; set; }

        [Required(ErrorMessage = "Must to choose category")]
        public int CategoryId { get; set; }
        public List<Category> Categories { get; set; }
        public string Slug => Name.ToLower().Replace(' ', '-');
    }
}
