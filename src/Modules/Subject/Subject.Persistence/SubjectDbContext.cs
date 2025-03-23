using Microsoft.EntityFrameworkCore;
using Subject.Persistence.Constants;

namespace Subject.Persistence
{
    /// <summary>
    /// Represents the users module database context.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="SubjectDbContext"/> class.
    /// </remarks>
    /// <param name="options">The database context options.</param>
    public sealed class SubjectDbContext(DbContextOptions<SubjectDbContext> options) : DbContext(options)
    {

        /// <inheritdoc />
        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schemas.Subjects);

            modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
        }
    }
}
