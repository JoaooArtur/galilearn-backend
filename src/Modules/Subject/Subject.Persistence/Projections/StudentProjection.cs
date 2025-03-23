using Core.Domain.Primitives;

namespace Subject.Persistence.Projections
{
    public class SubjectProjection<TProjection>(ISubjectProjectionDbContext context) : Core.Persistence.Projection.Projection<TProjection>(context), ISubjectProjection<TProjection>
        where TProjection : IProjection
    {
    }
}
