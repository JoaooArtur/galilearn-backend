using Microsoft.EntityFrameworkCore;
using Student.Persistence.Constants;

namespace Student.Persistence
{
     /// <summary>
     /// Represents the users module database context.
     /// </summary>
     /// <remarks>
     /// Initializes a new instance of the <see cref="StudentDbContext"/> class.
     /// </remarks>
     /// <param name="options">The database context options.</param>
    public sealed class StudentDbContext(DbContextOptions<StudentDbContext> options) : DbContext(options)
    {

        /// <inheritdoc />
        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schemas.Students);

            modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
        }
    }
}
