using Core.Infrastructure.Configuration;
using Core.Infrastructure.Extensions;
using Core.Persistence.Options;
using Core.Shared.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

namespace Core.Persistence
{
    /// <summary>
    /// Represents the persistence service installer.
    /// </summary>
    internal sealed class PersistenceServiceInstaller : IServiceInstaller
    {
        /// <inheritdoc />
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services
                .ConfigureOptions<ConnectionStringSetup>()
                .AddTransientAsMatchingInterfaces(AssemblyReference.Assembly)
                .Tap(() => Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true);

            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        }
    }
}
