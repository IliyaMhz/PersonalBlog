using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PersonalBlog.Api.Models.Dtos.BlogDtos
{
    public class BlogDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 200 characters")]
        [Display(Name = "Blog Title")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Content is required")]
        [MinLength(50, ErrorMessage = "Content must be at least 50 characters long")]
        [MaxLength(10000, ErrorMessage = "Content cannot exceed 10,000 characters")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Blog Content")]
        public required string Content { get; set; }

        [StringLength(500, ErrorMessage = "Summary cannot exceed 500 characters")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Blog Summary")]
        public string? Summary { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Last Updated")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", NullDisplayText = "Never updated")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Published Status")]
        public bool IsPublished { get; set; }
    }
}
