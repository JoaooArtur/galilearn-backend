using Core.Domain.Primitives;
using Core.Domain.Projection;

namespace Subject.Persistence.Projections
{
    public interface ISubjectProjection<TProjection> : IProjection<TProjection>
        where TProjection : IProjection
    { }
}
