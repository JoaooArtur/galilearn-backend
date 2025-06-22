
namespace Student.Shared.Response
{
    public sealed record ListFriendsRequestsByStudentIdResponse(
            Guid Id,
            Guid StudentId,
            Guid FriendId,
            string Name,
            int Level,
            string Status,
            DateTimeOffset CreatedAt);
}
