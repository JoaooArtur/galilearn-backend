using Core.Application.Messaging;
using Core.Shared.Errors;
using Core.Shared.Results;
using Subject.Domain;
using Subject.Persistence.Projections;
using Subject.Shared.Queries;
using Subject.Shared.Response;

namespace Lesson.Application.UseCases.Queries
{
    public class GetLessonByIdHandler(ISubjectProjection<Projection.Lesson> projectionGateway) : IQueryHandler<GetLessonByIdQuery, LessonResponse>
    {
        public async Task<Result<LessonResponse>> Handle(GetLessonByIdQuery query, CancellationToken cancellationToken)
        {
            var lesson = await projectionGateway.GetAsync(query.LessonId, cancellationToken);

            if (lesson is null)
                return Result.Failure<LessonResponse>(new NotFoundError(new/*DomainError.BillNotFound*/("LessonNotFound", "Lesson não encontrado")));

            return Result.Success<LessonResponse>(
                new(lesson.Id,
                    lesson.SubjectId,
                    lesson.Title,
                    lesson.Content,
                    lesson.Index,
                    lesson.CreatedAt));
        }
    }
}
