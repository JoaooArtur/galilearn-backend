using Core.Domain.Primitives;
using Core.Domain.Projection;

namespace Student.Persistence.Projections
{
    public interface IStudentProjection<TProjection> : IProjection<TProjection>
        where TProjection : IProjection
    { }
}
