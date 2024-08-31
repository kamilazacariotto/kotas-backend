using Microsoft.EntityFrameworkCore;
using PokemonInfra.Repository.Entities;

namespace PokemonInfra.Repository
{
    public class PokemonDbContext : DbContext
    {
        public PokemonDbContext(DbContextOptions<PokemonDbContext> options)
            : base(options)
        {
        }

        public DbSet<Mestre> Mestres { get; set; }
        public DbSet<Pokemon> Pokemons { get; set; }
        public DbSet<MestrePokemon> MestrePokemons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Mestre>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100); 
                entity.Property(e => e.CPF).IsRequired().HasMaxLength(11);
            });

            modelBuilder.Entity<Pokemon>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired() 
                    .HasMaxLength(100);

                entity.Property(e => e.AdditionalInfo)
                    .IsRequired()
                    .HasColumnType("TEXT"); 
            });

            modelBuilder.Entity<MestrePokemon>()
                .HasKey(mp => new { mp.MestreId, mp.PokemonId });

            modelBuilder.Entity<MestrePokemon>()
               .HasOne(mp => mp.Mestre)
               .WithMany(m => m.MestrePokemons)
               .HasForeignKey(mp => mp.MestreId);

            modelBuilder.Entity<MestrePokemon>()
                .HasOne(mp => mp.Pokemon)
                .WithMany(p => p.MestrePokemons)
                .HasForeignKey(mp => mp.PokemonId);
        }
    }
}
