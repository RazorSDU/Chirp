using Chirp.API.Extensions;
using Chirp.API.Filters;
using Chirp.Database;
using Chirp.Database.Seed;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Chirp.API.Mapping;
using Chirp.Core.Domain.Interfaces.Repositories;
using Chirp.Core.Domain.Interfaces.Services;
using Chirp.Core.Services; // MappingProfile
using Chirp.Database.Repositories;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ── services ──────────────────────────────────────────────
// controllers + global validation filter
builder.Services.AddControllers(o => o.Filters.Add<ValidationFilter>());

// domain + infrastructure + AutoMapper profile assembly (scans whole assembly)
builder.Services.AddChirpApplication(builder.Configuration);

// swagger / open‑api
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() { Title = "Chirp.API", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer",
            In = ParameterLocation.Header,
            Description = "Enter 'Bearer' [space] and then your valid JWT token.",
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                new List<string>()
            }
        });
    });

// rate‑limiting (ASP.NET Core 8)
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", o =>
    {
        o.Window = TimeSpan.FromMinutes(1);
        o.PermitLimit = 100;                 // 100 req/min per IP
        o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        o.QueueLimit = 50;
    });
});

//── Authentication───────────────────────────────────
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
builder.Services.AddAuthorization();



var app = builder.Build();

// ── middleware pipeline ───────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();     // global error handler
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// ── database migrate + seed ───────────────────────────────
/*using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ChirpContext>();
    await db.Database.MigrateAsync();
    await Seeder.SeedAsync(db);
}*/

app.Run();