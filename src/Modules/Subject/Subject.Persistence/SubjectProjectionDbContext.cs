using Core.Persistence.Projection.Abstractions;

namespace Subject.Persistence
{
    public interface ISubjectProjectionDbContext : IMongoDbContext { }

    public class SubjectProjectionDbContext(string connectionString) : MongoDbContext(connectionString), ISubjectProjectionDbContext;
}
