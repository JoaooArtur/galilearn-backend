using Microsoft.EntityFrameworkCore;

namespace Core.Application
{
    public interface IUnitOfWork<TContext> where TContext : DbContext
    {
        Task ExecuteAsync(Func<CancellationToken, Task> operationAsync, CancellationToken cancellationToken);
    }
}
