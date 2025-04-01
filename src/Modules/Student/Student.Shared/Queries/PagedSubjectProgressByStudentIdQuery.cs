using Core.Application.Messaging;
using Core.Domain.Primitives;
using Student.Shared.Response;

namespace Student.Shared.Queries
{
    public sealed record PagedSubjectProgressByStudentIdQuery(Guid StudentId, Paging Paging) : IQuery<IPagedResult<SubjectsProgressByStudentIdResponse>>;
}
