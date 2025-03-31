

using Core.Application.Messaging;
using Subject.Shared.Response;

namespace Subject.Shared.Queries
{
    public sealed record ListRandomQuestionsByLessonIdQuery(Guid LessonId) : IQuery<List<RandomQuestionsByLessonIdResponse>>;
}
