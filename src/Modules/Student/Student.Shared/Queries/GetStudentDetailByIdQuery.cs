using Core.Application.Messaging;
using Student.Shared.Response;

namespace Student.Shared.Queries
{
    public sealed record GetStudentDetailByIdQuery(Guid StudentId) : IQuery<StudentDetailResponse>;
}
