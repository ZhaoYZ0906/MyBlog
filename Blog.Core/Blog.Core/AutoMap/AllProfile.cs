using AutoMapper;
using Blog.Core.Model.Dto;
using Blog.Core.Model.Models;

namespace Blog.Core.Api.AutoMap
{
    public class AllProfile : Profile
    {
        public AllProfile()
        {
            CreateMap<BlogArticle, BlogDto>().ReverseMap();
        }
    }
}
