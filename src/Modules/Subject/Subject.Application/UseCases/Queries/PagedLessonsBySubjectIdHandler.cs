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
    public class PagedLessonsBySubjectIdHandler(
    ISubjectProjection<Projection.Lesson> projectionGateway,
    ISubjectProjection<Projection.Question> questionProjectionGateway) : IQueryHandler<PagedLessonsBySubjectIdQuery, IPagedResult<PagedLessonBySubjectIdResponse>>
    {
        public async Task<Result<IPagedResult<PagedLessonBySubjectIdResponse>>> Handle(PagedLessonsBySubjectIdQuery query, CancellationToken cancellationToken)
        {
            var lessons = await projectionGateway.ListAsync(lesson => lesson.SubjectId == query.SubjectId, cancellationToken);

            var response = new List<PagedLessonBySubjectIdResponse>();
            if (lessons is null)
                return Result.Success(PagedResult<PagedLessonBySubjectIdResponse>.Create(query.Paging, response.AsQueryable()));

            foreach (var lesson in lessons)
            {
                var questionCount = await questionProjectionGateway.ListAsync(x => x.LessonId == lesson.Id);

                response.Add(new PagedLessonBySubjectIdResponse(
                    lesson.Id,
                    lesson.SubjectId,
                    lesson.Title,
                    lesson.Content,
                    questionCount.Count,
                    lesson.Index,
                    lesson.CreatedAt));
            }

            return Result.Success(PagedResult<PagedLessonBySubjectIdResponse>.Create(query.Paging, response.AsQueryable()));
        }
    }
}
