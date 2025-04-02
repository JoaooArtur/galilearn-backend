
namespace Student.Shared.Response
{
    public sealed record ListFriendsRequestsByStudentIdResponse(
            Guid Id,
            Guid StudentId,
            Guid FriendId,
            string Status,
            DateTimeOffset CreatedAt);
}
