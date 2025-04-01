using Core.Application.Messaging;
using Core.Shared.Results;
using MediatR;
using Student.Domain;
using Student.Domain.Enumerations;
using Student.Persistence.Projections;
using Student.Shared.Queries;
using Student.Shared.Response;
using Subject.Shared.Queries;

namespace Student.Application.UseCases.Queries
{
    public class ListSubjectsProgressByStudentIdHandler(
        IStudentProjection<Projection.LessonProgress> projectionGateway,
        ISender sender) : IQueryHandler<ListSubjectsProgressByStudentIdQuery, List<SubjectsProgressByStudentIdResponse>>
    {
        public async Task<Result<List<SubjectsProgressByStudentIdResponse>>> Handle(ListSubjectsProgressByStudentIdQuery query, CancellationToken cancellationToken)
        {
            var subjectsResult = await sender.Send(new ListSubjectsQuery(), cancellationToken);

            if (subjectsResult.IsFailure)
                return Result.Failure<List<SubjectsProgressByStudentIdResponse>>(subjectsResult.Error);

            var subjects = subjectsResult.Value;

            var response = new List<SubjectsProgressByStudentIdResponse>();

            foreach (var subject in subjects) 
            {
                var lessonCountResult = await sender.Send(new GetLessonCountBySubjectIdQuery(subject.Id), cancellationToken);

                if (lessonCountResult.IsFailure)
                    return Result.Failure<List<SubjectsProgressByStudentIdResponse>>(lessonCountResult.Error);

                var lessonCount = lessonCountResult.Value;

                var finishedLessons = await projectionGateway.ListAsync(lessonProgress => 
                lessonProgress.StudentId == query.StudentId &&
                lessonProgress.SubjectId == subject.Id && 
                lessonProgress.Status == LessonStatus.Finished, cancellationToken);

                response.Add(new SubjectsProgressByStudentIdResponse(subject, new(finishedLessons.Count, lessonCount.Count)));
            }

            return Result.Success<List<SubjectsProgressByStudentIdResponse>>(response.OrderByDescending(x => x.Lessons.FinishedLessons).ToList());
        }
    }
}
