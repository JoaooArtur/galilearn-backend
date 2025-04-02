using Core.Application.Messaging;
using Student.Shared.Response;

namespace Student.Shared.Queries
{
    public sealed record ListFriendsRequestsByStudentIdQuery(Guid StudentId) : IQuery<List<ListFriendsRequestsByStudentIdResponse>>;
}
