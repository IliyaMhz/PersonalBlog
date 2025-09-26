using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.Api.Models.Dtos.BlogDtos
{
    public class CreateBlogDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 200 characters")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Content is required")]
        [MinLength(50, ErrorMessage = "Content must be at least 50 characters long")]
        public required string Content { get; set; }

        [StringLength(500, ErrorMessage = "Summary cannot exceed 500 characters")]
        public string? Summary { get; set; }

        [Display(Name = "Published Status")]
        public bool IsPublished { get; set; } = false;
    }
}
