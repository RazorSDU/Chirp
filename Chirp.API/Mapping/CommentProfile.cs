using AutoMapper;
using Chirp.API.DTOs.Comment;
using Chirp.Core.Domain.Entities;

namespace Chirp.API.Mapping;

public sealed class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<Post, CommentDto>()
            .ForMember(d => d.Username, o => o.MapFrom(s => s.User.Username));
    }
}