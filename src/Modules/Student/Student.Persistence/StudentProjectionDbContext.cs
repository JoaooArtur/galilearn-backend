using Core.Persistence.Projection.Abstractions;

namespace Student.Persistence
{
    public interface IStudentProjectionDbContext : IMongoDbContext { }

    public class StudentProjectionDbContext(string connectionString) : MongoDbContext(connectionString), IStudentProjectionDbContext;
}
