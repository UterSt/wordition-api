namespace Wordition.Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    public ICardRepository Card { get; }
    public ITextRepository Text { get; }
    public IUserRepository User { get; }
    
    public Task SaveAsync(CancellationToken cancellationToken = default);
}