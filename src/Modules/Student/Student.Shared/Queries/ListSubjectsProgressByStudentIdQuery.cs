using Core.Application.Messaging;
using Student.Shared.Response;

namespace Student.Shared.Queries
{
    public sealed record ListSubjectsProgressByStudentIdQuery(Guid StudentId) : IQuery<List<SubjectsProgressByStudentIdResponse>>;
}
