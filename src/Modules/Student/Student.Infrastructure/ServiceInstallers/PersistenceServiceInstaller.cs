using Core.Application;
using Core.Application.EventStore;
using Core.Infrastructure.Configuration;
using Core.Persistence;
using Core.Persistence.EventStore;
using Core.Persistence.Extensions;
using Core.Persistence.Options;
using Student.Persistence;
using Student.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Student.Persistence.Projections;

namespace Student.Infrastructure.ServiceInstallers
{
    /// <summary>
    /// Represents the users module persistence service installer.
    /// </summary>
    internal sealed class PersistenceServiceInstaller : IServiceInstaller
    {
        /// <inheritdoc />
        public void Install(IServiceCollection services, IConfiguration configuration) =>
            services
                .AddScoped(typeof(IStudentProjection<>), typeof(StudentProjection<>))
                .AddScoped(typeof(IEventStore<StudentDbContext>), typeof(EventStore<StudentDbContext>))
                .AddScoped(typeof(IUnitOfWork<StudentDbContext>), typeof(UnitOfWork<StudentDbContext>))
                .AddScoped<IStudentProjectionDbContext>(provider =>
                {
                    var configuration = provider.GetRequiredService<IConfiguration>();
                    string connectionString = configuration.GetSection("Projections").GetValue<string>("Student");

                    return new StudentProjectionDbContext(connectionString);
                })
                .AddDbContext<StudentDbContext>((provider, builder) =>
                {
                    ConnectionStringOptions connectionString = provider.GetService<IOptions<ConnectionStringOptions>>()!.Value;

                    builder
                        .UseNpgsql(
                            connectionString: connectionString,
                            dbContextOptionsBuilder => dbContextOptionsBuilder.WithMigrationHistoryTableInSchema(Schemas.Students));
                });
    }
}
