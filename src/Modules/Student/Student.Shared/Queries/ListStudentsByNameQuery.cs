using Core.Application.Messaging;
using Student.Shared.Response;

namespace Student.Shared.Queries
{
    public sealed record ListStudentsByNameQuery(string Name) : IQuery<List<StudentsByNameResponse>>;
}
