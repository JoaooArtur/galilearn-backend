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
    public class ListLessonProgressBySubjectIdHandler(
        IStudentProjection<Projection.LessonProgress> projectionGateway,
        ISender sender) : IQueryHandler<ListLessonProgressBySubjectIdQuery, List<LessonsProgressBySubjectIdResponse>>
    {
        public async Task<Result<List<LessonsProgressBySubjectIdResponse>>> Handle(ListLessonProgressBySubjectIdQuery query, CancellationToken cancellationToken)
        {
            var lessonsResult = await sender.Send(new ListLessonsBySubjectIdQuery(query.SubjectId), cancellationToken);

            if (lessonsResult.IsFailure)
                return Result.Failure<List<LessonsProgressBySubjectIdResponse>>(lessonsResult.Error);

            var lessons = lessonsResult.Value;

            var response = new List<LessonsProgressBySubjectIdResponse>();

            foreach (var lesson in lessons)
            {
                var status = LessonStatus.Pending;
                var lessonStatus = await projectionGateway.FindAsync(x => x.StudentId == query.StudentId && x.LessonId == lesson.Id, cancellationToken);

                if (lessonStatus is not null)
                    status = lessonStatus.Status;


                response.Add(new LessonsProgressBySubjectIdResponse(lesson, status));
            }

            return Result.Success<List<LessonsProgressBySubjectIdResponse>>(response.OrderByDescending(x => x.Status).ToList());
        }
    }
}
