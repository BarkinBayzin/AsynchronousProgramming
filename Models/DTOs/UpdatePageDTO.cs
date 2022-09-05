using System.ComponentModel.DataAnnotations;

namespace AsynchronousProgramming.Models.DTOs
{
    public class UpdatePageDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please type into page title")]
        [MinLength(3, ErrorMessage = "Minimum lenght is 3")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please type into page content")]
        [MinLength(3, ErrorMessage = "Minimum lenght is 3")]
        public string Content { get; set; }

        public string Slug => Title.ToLower().Replace(' ', '-');
    }
}
