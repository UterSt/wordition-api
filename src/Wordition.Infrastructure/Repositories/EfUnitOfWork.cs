using Wordition.Application.Interfaces.Repositories;
using Wordition.Infrastructure.Context;

namespace Wordition.Infrastructure.Repositories;

public class EfUnitOfWork : IUnitOfWork
{
    private WorditionDbContext _db;

    public EfUnitOfWork(WorditionDbContext db)
    {
        _db = db;
        Card = new CardRepository(db);
        Text = new TextRepository(db);
        User = new UserRepository(db);
    }
    
    public ICardRepository Card { get; }
    public ITextRepository Text { get; }
    public IUserRepository User { get; }
    
    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await _db.SaveChangesAsync(cancellationToken);
    }
}