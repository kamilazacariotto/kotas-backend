using Microsoft.EntityFrameworkCore;
using PokemonDomain.Interfaces;
using PokemonDomain.Services;
using PokemonInfra.Repository.Entities;
using PokemonInfra.Repository;
using PokemonInfra.Repository.Interfaces;
using PokemonInfra.Repository.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<PokemonService>();

builder.Services.AddDbContext<PokemonDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IMestresRepository, MestresRepository>();
builder.Services.AddScoped<IPokemonsRepository, PokemonsRepository>();
builder.Services.AddScoped<IMestrePokemonRepository, MestrePokemonRepository>();

builder.Services.AddScoped<SeedService>();
builder.Services.AddScoped<IPokemonService, PokemonService>();
builder.Services.AddScoped<IMestreService, MestreService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PokemonDbContext>();
    dbContext.Database.Migrate();
    var seedService = scope.ServiceProvider.GetRequiredService<SeedService>();
    await seedService.SeedPokemonsAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
