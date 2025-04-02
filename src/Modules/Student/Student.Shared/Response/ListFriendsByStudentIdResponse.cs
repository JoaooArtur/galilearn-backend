
namespace Student.Shared.Response
{
    public sealed record ListFriendsByStudentIdResponse(
            Guid Id,
            string Name,
            int Level,
            int DaysStreak);
}
