using AutoMapper;
using PersonalBlog.Api.Models.Dtos.BlogDtos;
using PersonalBlog.Api.Models.Entities;

namespace PersonalBlog.Api.Profile
{
    public class BlogProfile : AutoMapper.Profile
    {
        public BlogProfile()
        {
            CreateMap<CreateBlogDto, Blog>().ReverseMap();
            CreateMap<UpdateBlogDto, Blog>().ReverseMap();
            CreateMap<BlogDto, Blog>().ReverseMap();
        }
    }
}
