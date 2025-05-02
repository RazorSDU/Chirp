using AutoMapper;
using Chirp.API.DTOs.Post;
using Chirp.Core.Domain.Entities;

namespace Chirp.API.Mapping;

public sealed class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<Post, PostDto>()
            .ForMember(d => d.Username,
               o => o.MapFrom(s => s.User == null ? string.Empty : s.User.Username))
            .ForMember(d => d.ReplyCount, 
               o => o.MapFrom(s => s.Replies == null ? 0 : s.Replies.Count));

    }
}