using Core.Application.Messaging;
using Student.Shared.Response;

namespace Student.Shared.Queries
{
    public sealed record ListFriendsByStudentIdQuery(Guid StudentId) : IQuery<List<ListFriendsByStudentIdResponse>>;
}
