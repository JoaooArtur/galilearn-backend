
namespace Student.Shared.Response
{
    public sealed record StudentDetailResponse(
            Guid Id,
            string Name,
            string Phone,
            string Email,
            string Status,
            int Level,
            int Xp,
            int NextLevelXPNeeded,
            int DaysStreak,
            int? FriendsCount,
            DateTimeOffset DateOfBirth,
            DateTimeOffset? LastLessonAnswered,
            DateTimeOffset CreatedAt);
}
