using Chirp.Database;          // ChirpContext
using Chirp.Database.Seed;     // Seeder
using Microsoft.EntityFrameworkCore;
using Chirp.API.Mapping;              // MappingProfile

var builder = WebApplication.CreateBuilder(args);

// ? Services -------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddDbContext<ChirpContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddAutoMapper(typeof(MappingProfile));   // <- see section 2
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();      // ‹-- build first

// ? Pipeline -------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

// ? Migrate + seed ------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ChirpContext>();
    await db.Database.MigrateAsync();
    await Seeder.SeedAsync(db);
}

app.Run();
