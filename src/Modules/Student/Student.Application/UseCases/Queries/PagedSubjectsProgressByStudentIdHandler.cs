using Core.Application.Messaging;
using Core.Application.Pagination;
using Core.Domain.Primitives;
using Core.Shared.Results;
using MediatR;
using Student.Domain;
using Student.Domain.Enumerations;
using Student.Persistence.Projections;
using Student.Shared.Queries;
using Student.Shared.Response;
using Subject.Shared.Queries;
using Subject.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student.Application.UseCases.Queries
{
    public class PagedSubjectsProgressByStudentIdHandler(
        IStudentProjection<Projection.LessonProgress> projectionGateway,
        ISender sender) : IQueryHandler<PagedSubjectProgressByStudentIdQuery, IPagedResult<SubjectsProgressByStudentIdResponse>>
    {
        public async Task<Result<IPagedResult<SubjectsProgressByStudentIdResponse>>> Handle(PagedSubjectProgressByStudentIdQuery query, CancellationToken cancellationToken)
        {
            var subjectsResult = await sender.Send(new ListSubjectsQuery(), cancellationToken);

            if (subjectsResult.IsFailure)
                return Result.Failure<IPagedResult<SubjectsProgressByStudentIdResponse>>(subjectsResult.Error);

            var subjects = subjectsResult.Value;

            var response = new List<SubjectsProgressByStudentIdResponse>();

            foreach (var subject in subjects)
            {
                var lessonCountResult = await sender.Send(new GetLessonCountBySubjectIdQuery(subject.Id), cancellationToken);

                if (lessonCountResult.IsFailure)
                    return Result.Failure<IPagedResult<SubjectsProgressByStudentIdResponse>>(lessonCountResult.Error);

                var lessonCount = lessonCountResult.Value;

                var finishedLessons = await projectionGateway.ListAsync(lessonProgress =>
                lessonProgress.StudentId == query.StudentId &&
                lessonProgress.SubjectId == subject.Id &&
                lessonProgress.Status == LessonStatus.Finished, cancellationToken);

                response.Add(new SubjectsProgressByStudentIdResponse(subject, new(finishedLessons.Count, lessonCount.Count)));
            }
            return response is not null
                ? Result.Success(PagedResult<SubjectsProgressByStudentIdResponse>.Create(query.Paging, response.AsQueryable()))
                : Result.Success<IPagedResult<SubjectsProgressByStudentIdResponse>>(default);
        }
    }
}
