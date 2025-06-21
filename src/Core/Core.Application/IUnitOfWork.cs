using Microsoft.EntityFrameworkCore;

namespace Core.Application
{
    public interface IUnitOfWork<TContext> where TContext : DbContext //NOSONAR
    {
        Task ExecuteAsync(Func<CancellationToken, Task> operationAsync, CancellationToken cancellationToken);
    }
}
