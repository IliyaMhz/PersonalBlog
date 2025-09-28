using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PersonalBlog.Api.Models.DataBaseContext;
using PersonalBlog.Api.Models.Dtos.BlogDtos;
using PersonalBlog.Api.Models.Entities;
using PersonalBlog.Api.Service.Interfaces;

namespace PersonalBlog.Api.Service.Services
{
    public class BlogService : IBlogService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<BlogService> _logger;

        public BlogService(AppDbContext context, IMapper mapper, ILogger<BlogService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<BlogDto>> GetAllBlogsAsync()
        {
            _logger.LogInformation("Fetching all blogs from database");

            var Blogs = await _context.Blogs
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            _logger.LogInformation("Successfully retrieved {BlogCount} blogs", Blogs.Count);
            return _mapper.Map<IEnumerable<BlogDto>>(Blogs);
        }

        public async Task<BlogDto> GetBlogByIdAsync(int id)
        {

            _logger.LogInformation("Fetching blog with ID: {BlogId}", id);

            var Blog = await _context.Blogs.FirstOrDefaultAsync(B => B.Id == id);
            if (Blog == null)
            {
                _logger.LogWarning("Blog with ID {BlogId} not found", id);
                throw new KeyNotFoundException($"Blog with ID {id} not found");
            }
            _logger.LogInformation("Successfully retrieved blog with ID: {BlogId}", id);
            return _mapper.Map<BlogDto>(Blog);
        }

        public async Task<BlogDto> CreateBlogAsync(CreateBlogDto createBlogDto)
        {
            try
            {
                _logger.LogInformation("Creating new blog");

                var Blog = _mapper.Map<Blog>(createBlogDto);

                Blog.CreatedAt = DateTime.UtcNow;

                if (string.IsNullOrEmpty(Blog.Summary))
                {
                    _logger.LogInformation("Generating summary for blog from content");
                    Blog.Summary = CreateSummary(Blog.Content);
                }

                _context.Blogs.Add(Blog);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully created blog with ID: {BlogId}", Blog.Id);
                return _mapper.Map<BlogDto>(Blog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating new blog");
                throw;
            }
        }
        public async Task<BlogDto> UpdateBlogAsync(int id, UpdateBlogDto updateBlogDto)
        {
            try
            {
                _logger.LogInformation("Updating blog with ID: {BlogId}", id);

                var Blog = await _context.Blogs.FindAsync(id);

                if (Blog == null)
                {
                    _logger.LogWarning("Blog with ID {BlogId} not found for update", id);
                    throw new KeyNotFoundException($"Blog with ID {id} not found");
                }

                _mapper.Map(updateBlogDto, Blog);
                Blog.UpdatedAt = DateTime.UtcNow;

                if (string.IsNullOrEmpty(Blog.Summary) || updateBlogDto.Content != Blog.Content)
                {
                    _logger.LogInformation("Generating new summary for blog with ID: {BlogId}", id);
                    Blog.Summary = CreateSummary(Blog.Content);
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully updated blog with ID: {BlogId}", id);
                return _mapper.Map<BlogDto>(Blog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating blog with ID: {BlogId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteBlogAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting blog with ID: {BlogId}", id);

                var blog = await _context.Blogs.FindAsync(id);

                if (blog == null)
                {
                    _logger.LogWarning("Blog with ID {BlogId} not found for deletion", id);
                    throw new KeyNotFoundException($"Blog with ID {id} not found");
                }

                _context.Blogs.Remove(blog);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully deleted blog with ID: {BlogId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting blog with ID: {BlogId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<BlogDto>> GetPublishedBlogsAsync()
        {
            _logger.LogInformation("Fetching published blogs");

            var blogs = await _context.Blogs
                .Where(b => b.IsPublished)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            _logger.LogInformation("Successfully retrieved {PublishedBlogCount} published blogs", blogs.Count);
            return _mapper.Map<IEnumerable<BlogDto>>(blogs);
        }

        public async Task<IEnumerable<BlogDto>> GetUnPublishedBlogsAsync()
        {
            _logger.LogInformation("Fetching published blogs");

            var blogs = await _context.Blogs
                .Where(b => !b.IsPublished)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            _logger.LogInformation("Successfully retrieved {PublishedBlogCount} published blogs", blogs.Count);
            return _mapper.Map<IEnumerable<BlogDto>>(blogs);
        }

        public async Task<IEnumerable<BlogDto>> SearchBlogsAsync(string searchTerm)
        {
            _logger.LogInformation("Searching blogs with term: {SearchTerm}", searchTerm);

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                _logger.LogInformation("Search term is empty, returning all blogs");
                return await GetAllBlogsAsync();
            }

            var blogs = await _context.Blogs
                .Where(b => b.Title.Contains(searchTerm) ||
                           b.Content.Contains(searchTerm) ||
                           b.Summary.Contains(searchTerm))
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            _logger.LogInformation("Search completed. Found {SearchResultCount} blogs for term: {SearchTerm}",
                blogs.Count, searchTerm);
            return _mapper.Map<IEnumerable<BlogDto>>(blogs);
        }

        private string CreateSummary(string content, int maxLength = 150)
        {
            if (string.IsNullOrEmpty(content))
                return string.Empty;

            var plainText = System.Text.RegularExpressions.Regex.Replace(
                content, "<.*?>", string.Empty);
            if (plainText.Length <= maxLength)
                return plainText;

            return plainText.Substring(0, maxLength) + "...";
        }
    }
}
