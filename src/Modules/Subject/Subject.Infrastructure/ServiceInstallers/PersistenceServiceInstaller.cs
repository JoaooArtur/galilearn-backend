using Core.Application;
using Core.Application.EventStore;
using Core.Infrastructure.Configuration;
using Core.Persistence;
using Core.Persistence.EventStore;
using Core.Persistence.Extensions;
using Core.Persistence.Options;
using Subject.Persistence;
using Subject.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Subject.Persistence.Projections;

namespace Subject.Infrastructure.ServiceInstallers
{
    /// <summary>
    /// Represents the users module persistence service installer.
    /// </summary>
    internal sealed class PersistenceServiceInstaller : IServiceInstaller
    {
        /// <inheritdoc />
        public void Install(IServiceCollection services, IConfiguration configuration) =>
            services
                .AddScoped(typeof(ISubjectProjection<>), typeof(SubjectProjection<>))
                .AddScoped(typeof(IEventStore<SubjectDbContext>), typeof(EventStore<SubjectDbContext>))
                .AddScoped(typeof(IUnitOfWork<SubjectDbContext>), typeof(UnitOfWork<SubjectDbContext>))
                .AddScoped<ISubjectProjectionDbContext>(provider =>
                {
                    var configuration = provider.GetRequiredService<IConfiguration>();
                    string connectionString = configuration.GetSection("Projections").GetValue<string>("Subject");

                    return new SubjectProjectionDbContext(connectionString);
                })
                .AddDbContext<SubjectDbContext>((provider, builder) =>
                {
                    ConnectionStringOptions connectionString = provider.GetService<IOptions<ConnectionStringOptions>>()!.Value;

                    builder
                        .UseNpgsql(
                            connectionString: connectionString,
                            dbContextOptionsBuilder => dbContextOptionsBuilder.WithMigrationHistoryTableInSchema(Schemas.Subjects));
                });
    }
}
