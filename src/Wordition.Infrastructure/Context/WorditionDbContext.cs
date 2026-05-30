using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Wordition.Domain.Entities;

namespace Wordition.Infrastructure.Context;

public class WorditionDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<WorditionCard> Cards { get; set; }
    public DbSet<CardReviewLog>  CardReviewLogs { get; set; }
    public DbSet<Text> Texts { get; set; }
    public DbSet<Word> Words { get; set; }
    public DbSet<WordTranslation> WordTranslations { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public WorditionDbContext(DbContextOptions<WorditionDbContext> options) : base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .OwnsOne(u => u.Email)
            .Property(e => e.Value)
            .HasColumnName("Email");
        
        modelBuilder.Entity<Text>()
            .Property(t => t.CreatedAt)
            .ValueGeneratedOnAdd()
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        modelBuilder.Entity<WorditionCard>()
            .OwnsOne(c => c.Intervals);
    }
}