
namespace Student.Shared.Response
{
    public sealed record CorrectAnswerResponse(Guid AttemptId, Guid QuestionId, Guid AnswerId, Guid CorrectAnswerId, bool IsCorrect);
}
