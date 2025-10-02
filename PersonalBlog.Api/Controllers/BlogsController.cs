using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Api.Models.Dtos.BlogDtos;
using PersonalBlog.Api.Models.Entities;
using PersonalBlog.Api.Service.Interfaces;

namespace PersonalBlog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly ILogger<BlogsController> _logger;

        public BlogsController(IBlogService blogService, ILogger<BlogsController> logger)
        {
            _blogService = blogService;
            _logger = logger;
        }

        /// <summary>
        /// دریافت تمامی وبلاگ‌ها
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogDto>>> GetAllBlogs()
        {
            try
            {
                var blogs = await _blogService.GetAllBlogsAsync();
                return Ok(blogs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all blogs");
                return StatusCode(500, "An error occurred while fetching blogs");
            }
        }

        /// <summary>
        /// دریافت وبلاگ بر اساس شناسه
        /// </summary>
        /// <param name="id">شناسه وبلاگ</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<BlogDto>> GetBlogById(int id)
        {
            try
            {
                var blog = await _blogService.GetBlogByIdAsync(id);
                return Ok(blog);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Blog with ID {BlogId} not found", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching blog with ID: {BlogId}", id);
                return StatusCode(500, "An error occurred while fetching the blog");
            }
        }

        /// <summary>
        /// ایجاد وبلاگ جدید
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<BlogDto>> CreateBlog([FromBody] CreateBlogDto createBlogDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdBlog = await _blogService.CreateBlogAsync(createBlogDto);
                return CreatedAtAction(nameof(GetBlogById), new { id = createdBlog.Id }, createdBlog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating new blog");
                return StatusCode(500, "An error occurred while creating the blog");
            }
        }

        /// <summary>
        /// به‌روزرسانی وبلاگ
        /// </summary>
        /// <param name="id">شناسه وبلاگ</param>
        [HttpPut("{id}")]
        public async Task<ActionResult<BlogDto>> UpdateBlog(int id, [FromBody] UpdateBlogDto updateBlogDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedBlog = await _blogService.UpdateBlogAsync(id, updateBlogDto);
                return Ok(updatedBlog);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Blog with ID {BlogId} not found for update", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating blog with ID: {BlogId}", id);
                return StatusCode(500, "An error occurred while updating the blog");
            }
        }

        /// <summary>
        /// حذف وبلاگ
        /// </summary>
        /// <param name="id">شناسه وبلاگ</param>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBlog(int id)
        {
            try
            {
                var result = await _blogService.DeleteBlogAsync(id);
                if (result)
                {
                    return NoContent();
                }
                return BadRequest("Failed to delete the blog");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Blog with ID {BlogId} not found for deletion", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting blog with ID: {BlogId}", id);
                return StatusCode(500, "An error occurred while deleting the blog");
            }
        }

        /// <summary>
        /// دریافت وبلاگ‌های منتشر شده
        /// </summary>
        [HttpGet("published")]
        public async Task<ActionResult<IEnumerable<BlogDto>>> GetPublishedBlogs()
        {
            try
            {
                var blogs = await _blogService.GetPublishedBlogsAsync();
                return Ok(blogs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching published blogs");
                return StatusCode(500, "An error occurred while fetching published blogs");
            }
        }

        /// <summary>
        /// دریافت وبلاگ‌های منتشر نشده
        /// </summary>
        [HttpGet("unpublished")]
        public async Task<ActionResult<IEnumerable<BlogDto>>> GetUnPublishedBlogs()
        {
            try
            {
                var blogs = await _blogService.GetUnPublishedBlogsAsync();
                return Ok(blogs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching unpublished blogs");
                return StatusCode(500, "An error occurred while fetching unpublished blogs");
            }
        }

        /// <summary>
        /// جستجو در وبلاگ‌ها
        /// </summary>
        /// <param name="searchTerm">عبارت جستجو</param>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<BlogDto>>> SearchBlogs([FromQuery] string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return BadRequest("Search term is required");
                }

                var blogs = await _blogService.SearchBlogsAsync(searchTerm);
                return Ok(blogs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching blogs with term: {SearchTerm}", searchTerm);
                return StatusCode(500, "An error occurred while searching blogs");
            }
        }
    }
}
