using Core.Application;
using Core.Application.ServiceLifetimes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Core.Persistence
{
    public class UnitOfWork<TContext>(TContext dbContext) : IUnitOfWork<TContext>, IScoped
        where TContext : DbContext
    {
        private readonly DatabaseFacade _database = dbContext.Database;

        public Task ExecuteAsync(Func<CancellationToken, Task> operationAsync, CancellationToken cancellationToken)
            => _database.CreateExecutionStrategy().ExecuteAsync(ct => ExecuteTransactionAsync(operationAsync, ct), cancellationToken);

        private async Task ExecuteTransactionAsync(Func<CancellationToken, Task> operationAsync, CancellationToken cancellationToken)
        {
            await using var transaction = await _database.BeginTransactionAsync(cancellationToken);
            await operationAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
    }
}
