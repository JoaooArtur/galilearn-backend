using Core.Application.Messaging;
using Core.Application.Pagination;
using Core.Domain.Primitives;
using Core.Shared.Results;
using Subject.Domain;
using Subject.Persistence.Projections;
using Subject.Shared.Queries;
using Subject.Shared.Response;

namespace Subject.Application.UseCases.Queries
{
    public class PagedLessonsBySubjectIdHandler(ISubjectProjection<Projection.Lesson> projectionGateway) : IQueryHandler<PagedLessonsBySubjectIdQuery, IPagedResult<PagedLessonBySubjectIdResponse>>
    {
        public async Task<Result<IPagedResult<PagedLessonBySubjectIdResponse>>> Handle(PagedLessonsBySubjectIdQuery query, CancellationToken cancellationToken)
        {
            var lessons = await projectionGateway.ListAsync(lesson => lesson.SubjectId == query.SubjectId, cancellationToken);

            return lessons is not null
                ? Result.Success(PagedResult<PagedLessonBySubjectIdResponse>.Create(query.Paging, lessons.Select(lesson => new PagedLessonBySubjectIdResponse(
                lesson.Id, lesson.SubjectId, lesson.Title, lesson.Content, lesson.Index, lesson.CreatedAt)).AsQueryable()))
                : Result.Success<IPagedResult<PagedLessonBySubjectIdResponse>>(default);
        }
    }
}
