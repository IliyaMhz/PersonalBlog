using PersonalBlog.Api.Models.Dtos.BlogDtos;

namespace PersonalBlog.Api.Service.Interfaces
{
    public interface IBlogService
    {
        Task<IEnumerable<BlogDto>> GetAllBlogsAsync();
        Task<BlogDto> GetBlogByIdAsync(int id);
        Task<BlogDto> CreateBlogAsync(CreateBlogDto createBlogDto);
        Task<BlogDto> UpdateBlogAsync(int id, UpdateBlogDto updateBlogDto);
        Task<bool> DeleteBlogAsync(int id);
        Task<IEnumerable<BlogDto>> GetPublishedBlogsAsync();
        Task<IEnumerable<BlogDto>> GetUnPublishedBlogsAsync();
        Task<IEnumerable<BlogDto>> SearchBlogsAsync(string searchTerm);
    }
}
