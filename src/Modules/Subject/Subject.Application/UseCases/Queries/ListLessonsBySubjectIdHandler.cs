using Core.Application.Messaging;
using Core.Shared.Results;
using Subject.Domain;
using Subject.Persistence.Projections;
using Subject.Shared.Queries;
using Subject.Shared.Response;

namespace Subject.Application.UseCases.Queries
{
    public class ListLessonsBySubjectIdHandler(ISubjectProjection<Projection.Lesson> projectionGateway,
        ISubjectProjection<Projection.Question> questionProjectionGateway) : IQueryHandler<ListLessonsBySubjectIdQuery, List<PagedLessonBySubjectIdResponse>>
    {
        public async Task<Result<List<PagedLessonBySubjectIdResponse>>> Handle(ListLessonsBySubjectIdQuery query, CancellationToken cancellationToken)
        {
            var lessons = await projectionGateway.ListAsync(lesson => lesson.SubjectId == query.SubjectId, cancellationToken);

            var response = new List<PagedLessonBySubjectIdResponse>();

            foreach (var lesson in lessons)
            {
                var questionCount = await questionProjectionGateway.ListAsync(x => x.LessonId == lesson.Id);

                response.Add(new PagedLessonBySubjectIdResponse(
                lesson.Id, lesson.SubjectId, lesson.Title, lesson.Content, questionCount.Count, lesson.Index, lesson.CreatedAt));
            }
            return lessons is not null
                ? Result.Success(response)
                : Result.Success<List<PagedLessonBySubjectIdResponse>>(default);
        }
    }
}
