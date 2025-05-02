using Chirp.API.Extensions;
using Chirp.API.Filters;
using Chirp.Database;
using Chirp.Database.Seed;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// ── services ──────────────────────────────────────────────
// controllers + global validation filter
builder.Services.AddControllers(o => o.Filters.Add<ValidationFilter>());

// domain + infrastructure + AutoMapper profile assembly (scans whole assembly)
builder.Services.AddChirpApplication(builder.Configuration);

// swagger / open‑api
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

var app = builder.Build();

// ── middleware pipeline ───────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();     // global error handler
app.UseRateLimiter();                         // fixed window limiter
app.UseAuthorization();
app.MapControllers();

// ── database migrate + seed ───────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ChirpContext>();
    await db.Database.MigrateAsync();
    await Seeder.SeedAsync(db);
}

app.Run();