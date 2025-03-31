using Core.Application.Messaging;
using Core.Shared.Results;
using Subject.Domain;
using Subject.Persistence.Projections;
using Subject.Shared.Queries;
using Subject.Shared.Response;
namespace Subject.Application.UseCases.Queries
{
    public class GetLessonCountBySubjectIdHandler(ISubjectProjection<Projection.Lesson> projectionGateway) : IQueryHandler<GetLessonCountBySubjectIdQuery, CountResponse>
    {
        public async Task<Result<CountResponse>> Handle(GetLessonCountBySubjectIdQuery query, CancellationToken cancellationToken)
        {
            var lessons = await projectionGateway.ListAsync(lesson => lesson.SubjectId == query.SubjectId, cancellationToken);

            if (lessons is null)
                return Result.Success<CountResponse>(new(0));

            return Result.Success<CountResponse>(
                new(lessons.Count));
        }
    }
}
