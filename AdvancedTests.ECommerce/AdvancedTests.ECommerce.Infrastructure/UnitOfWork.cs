using System.Data;
using AdvancedTests.ECommerce.Domain;

namespace AdvancedTests.ECommerce.Infrastructure;

public class UnitOfWork(IDbConnection connection) : IUnitOfWork
{
    private IDbTransaction? _transaction;
    public Task BeginTransactionAsync()
    {
        _transaction = connection.BeginTransaction();
        return Task.CompletedTask;
    }

    public Task CommitAsync()
    {
        _transaction?.Commit();
        return Task.CompletedTask;
    }
}