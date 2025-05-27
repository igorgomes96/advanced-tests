namespace AdvancedTests.ECommerce.Domain;

public interface IUnitOfWork
{
    Task BeginTransactionAsync();
    Task CommitAsync();
}