

namespace Subject.Shared.Response
{
    public sealed record CheckCorrectAnswerResponse(Guid correctAnswerId, bool IsCorrectAnswer);
}
