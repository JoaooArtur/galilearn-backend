using Core.Domain.Primitives;

namespace Student.Persistence.Projections
{
    public class StudentProjection<TProjection>(IStudentProjectionDbContext context) : Core.Persistence.Projection.Projection<TProjection>(context), IStudentProjection<TProjection>
        where TProjection : IProjection
    {
    }
}
