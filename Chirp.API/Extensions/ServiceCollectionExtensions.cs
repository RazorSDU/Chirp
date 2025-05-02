using AutoMapper;
using Chirp.API.Mapping;
using Chirp.Core.Domain.Interfaces.Repositories;
using Chirp.Core.Domain.Interfaces.Services;
using Chirp.Core.Services;
using Chirp.Database;
using Chirp.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Chirp.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddChirpApplication(this IServiceCollection services, IConfiguration config)
    {
        // EF Core
        services.AddDbContext<ChirpContext>(opts =>
            opts.UseSqlServer(config.GetConnectionString("Default")));

        // AutoMapper – scan the assembly that contains PostProfile
        services.AddAutoMapper(typeof(PostProfile).Assembly);

        // repositories
        services.AddScoped<IUserRepository, UserRepository>();   //  ←  keep this
        services.AddScoped<IPostRepository, PostRepository>();

        // domain services
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<ICommentService, CommentService>();

        return services;
    }
}