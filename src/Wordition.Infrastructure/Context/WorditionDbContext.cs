using Microsoft.EntityFrameworkCore;
using Wordition.Domain.Entities;

namespace Wordition.Infrastructure.Context;

public class WorditionDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<Text> Texts { get; set; }
    public DbSet<Word> Words { get; set; }
    public DbSet<WordTranslation> WordTranslations { get; set; }

    public WorditionDbContext(DbContextOptions<WorditionDbContext> options) : base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .OwnsOne(u => u.Email)
            .Property(e => e.Value)
            .HasColumnName("Email");
    }
}