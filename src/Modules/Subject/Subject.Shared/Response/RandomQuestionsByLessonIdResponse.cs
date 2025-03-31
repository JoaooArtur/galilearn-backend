
namespace Subject.Shared.Response
{
    public sealed record RandomQuestionsByLessonIdResponse(
            Guid Id,
            Guid LessonId,
            string Text,
            string Level,
            List<AnswerResponse> Answers,
            DateTimeOffset CreatedAt);

    public sealed record AnswerResponse(
        Guid Id,
        string Text);
}
