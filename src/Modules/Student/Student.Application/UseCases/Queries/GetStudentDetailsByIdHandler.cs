using Core.Application.Messaging;
using Core.Shared.Results;
using MediatR;
using Student.Domain;
using Student.Persistence.Projections;
using Student.Shared.Queries;
using Student.Shared.Response;

namespace Student.Application.UseCases.Queries
{
    public class GetStudentDetailsByIdHandler(
        IStudentProjection<Projection.Student> projectionGateway) : IQueryHandler<GetStudentDetailByIdQuery, StudentDetailResponse>
    {
        public async Task<Result<StudentDetailResponse>> Handle(GetStudentDetailByIdQuery query, CancellationToken cancellationToken)
        {
            var student = await projectionGateway.GetAsync(query.StudentId, cancellationToken);

            return Result.Success(new StudentDetailResponse(
                student.Id,
                student.Name,
                student.Phone,
                student.Email,
                student.Status,
                student.Level,
                student.Xp,
                student.NextLevelXPNeeded,
                student.DaysStreak,
                student.Friends?.Count,
                student.DateOfBirth,
                student.LastLessonAnswered,
                student.CreatedAt));
        }
    }
}
